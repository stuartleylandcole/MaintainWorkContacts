using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Contacts;
using Google.GData.Extensions;
using MaintainWorkContacts.Utilities;
using Google.GData.Contacts;
using MaintainWorkContacts.Properties;

namespace MaintainWorkContacts.Service
{
    public class ContactComparer
    {
        private List<WorkContact> _oohSheetContacts;
        private List<Contact> _googleContacts;
        private GoogleContactsManager _contactsManager;

        public ContactComparer(List<WorkContact> oohSheetContacts, GoogleContactsManager contactsManager)
        {
            _oohSheetContacts = oohSheetContacts;
            _googleContacts = contactsManager.GetContacts();
            _contactsManager = contactsManager;
        }

        public List<ContactAction> Compare()
        {
            MatchContacts();

            List<ContactActionUpdate> updates = UpdateContacts();
            List<ContactActionAdd> newContacts = CreateNewContacts();
            List<ContactActionDelete> contactsToDelete = DeleteContacts();

            var actions = new List<ContactAction>();
            actions.AddRange(updates);
            actions.AddRange(newContacts);
            actions.AddRange(contactsToDelete);

            return actions;
        }

        private void MatchContacts()
        {
            ContactMatcher matcher = new ContactMatcher(_googleContacts);
            foreach (WorkContact contact in _oohSheetContacts)
            {
                Contact matchedContact = matcher.Match(contact);
                if (matchedContact != null)
                {
                    contact.MatchedContact = matchedContact;
                }
            }
        }

        private List<ContactActionUpdate> UpdateContacts()
        {
            List<ContactActionUpdate> updates = new List<ContactActionUpdate>();

            foreach (WorkContact contact in _oohSheetContacts.Where(c => c.MatchedContact != null))
            {
                updates.AddIfNotNull(UpdateMobile(contact));
                updates.AddIfNotNull(UpdateHomeNumber(contact));
                updates.AddIfNotNull(UpdateName(contact));
            }

            return updates;
        }

        private ContactActionUpdate UpdateMobile(WorkContact contact)
        {
            string oldMobileNumber = contact.MatchedContact.GetMobilePhoneNumberValue();
            if (oldMobileNumber == contact.Mobile)
            {
                return null;
            }

            PhoneNumber pn = contact.MatchedContact.GetMobilePhoneNumber();
            string previousPhoneNumber = GetPhoneNumberDescription(pn);
            UpdatePhoneNumber(pn, ContactsRelationships.IsMobile, contact.MatchedContact, contact.Mobile);

            return new ContactActionUpdate(_contactsManager.Request, contact.MatchedContact, "Will change " + contact.FullName + "'s mobile number from " + previousPhoneNumber + " to " + contact.Mobile);
        }

        private ContactActionUpdate UpdateHomeNumber(WorkContact contact)
        {
            string oldHomeNumber = contact.MatchedContact.GetHomePhoneNumberValue();
            if (oldHomeNumber == contact.HomePhoneNumber)
            {
                return null;
            }

            PhoneNumber pn = contact.MatchedContact.GetHomePhoneNumber();
            string previousPhoneNumber = GetPhoneNumberDescription(pn);
            UpdatePhoneNumber(pn, ContactsRelationships.IsHome, contact.MatchedContact, contact.HomePhoneNumber);

            return new ContactActionUpdate(_contactsManager.Request, contact.MatchedContact, "Will change " + contact.FullName + "'s home number from " + previousPhoneNumber + " to " + contact.HomePhoneNumber);
        }

        private void UpdatePhoneNumber(PhoneNumber existingPhoneNumber, string numberType, Contact googleContact, string newPhoneNumber)
        {
            if (existingPhoneNumber == null)
            {
                existingPhoneNumber = new PhoneNumber();
                existingPhoneNumber.Rel = numberType;
                googleContact.Phonenumbers.Add(existingPhoneNumber);
            }

            if (!string.IsNullOrWhiteSpace(newPhoneNumber))
            {
                existingPhoneNumber.Value = newPhoneNumber;
            }
            else
            {
                googleContact.Phonenumbers.Remove(existingPhoneNumber);
            }
        }

        private string GetPhoneNumberDescription(PhoneNumber pn)
        {
            if (pn == null)
            {
                return "(unset)";
            }
            return pn.Value;
        }

        private ContactActionUpdate UpdateName(WorkContact contact)
        {
            string oldInitials = contact.MatchedContact.GetInitials();
            string oldSurname = contact.MatchedContact.GetSurname();
            if (oldInitials == contact.Initials
                && oldSurname == contact.Surname)
            {
                return null;
            }

            string previousName = GetPreviousName(contact);

            contact.MatchedContact.Name.NameSuffix = "(" + contact.Initials + ")";
            contact.MatchedContact.Name.FamilyName = contact.Surname;
            contact.MatchedContact.Name.FullName = contact.FullName;

            return new ContactActionUpdate(_contactsManager.Request, contact.MatchedContact, "Will change " + previousName + "'s name to " + contact.FullName);
        }

        private string GetPreviousName(WorkContact contact)
        {
            string previousSurname = contact.MatchedContact.Name.FamilyName;
            string previousInitials = contact.MatchedContact.Name.NameSuffix;

            return contact.Firstname + " " + previousSurname + " " + previousInitials;
        }

        private List<ContactActionAdd> CreateNewContacts()
        {
            List<ContactActionAdd> newContacts = new List<ContactActionAdd>();
            foreach (WorkContact contact in _oohSheetContacts.Where(c => c.MatchedContact == null))
            {
                newContacts.AddIfNotNull(CreateGoogleContact(contact));
            }

            return newContacts;
        }

        private ContactActionAdd CreateGoogleContact(WorkContact contact)
        {
            if (ShouldIgnoreContact(contact))
            {
                return null;
            }

            Contact googleContact = new Contact();
            googleContact.Name.FullName = contact.FullName;
            googleContact.Name.GivenName = contact.Firstname;
            googleContact.Name.FamilyName = contact.Surname;
            googleContact.Name.NameSuffix = "(" + contact.Initials + ")";

            CreateAndAddPhoneNumber(contact.HomePhoneNumber, ContactsRelationships.IsHome, googleContact);
            CreateAndAddPhoneNumber(contact.Mobile, ContactsRelationships.IsMobile, googleContact);

            AddToGroups(googleContact);

            AddCompany(googleContact, contact);

            return new ContactActionAdd(_contactsManager.Request, googleContact);
        }

        private bool ShouldIgnoreContact(WorkContact contact)
        {
            return Settings.Default.InitialsToIgnore.Contains(contact.Initials);
        }

        private void CreateAndAddPhoneNumber(string number, string contactType, Contact googleContact)
        {
            if (!string.IsNullOrWhiteSpace(number))
            {
                googleContact.Phonenumbers.Add
                (
                    new PhoneNumber
                    {
                        Value = number,
                        Rel = contactType
                    }
                );
            }
        }

        private void AddToGroups(Contact googleContact)
        {
            CreateGroupMembershipAndAddToContact(googleContact, _contactsManager.WorkGroup);
            CreateGroupMembershipAndAddToContact(googleContact, _contactsManager.MyContactsGroup);
        }

        private void CreateGroupMembershipAndAddToContact(Contact googleContact, Group group)
        {
            GroupMembership gm = new GroupMembership();
            gm.HRef = group.Id;
            googleContact.GroupMembership.Add(gm);
        }

        private void AddCompany(Contact googleContact, WorkContact contact)
        {
            Organization organisation = new Organization();
            organisation.Name = "TPP";
            organisation.Rel = "http://schemas.google.com/g/2005#other";
            organisation.JobDescription = contact.Group.ToString();
            googleContact.Organizations.Add(organisation);
        }

        private List<ContactActionDelete> DeleteContacts()
        {
            List<ContactActionDelete> contactsToDelete = new List<ContactActionDelete>();

            foreach (Contact googleContact in _googleContacts)
            {
                bool foundMatch = false;
                foreach (WorkContact contact in _oohSheetContacts)
                {
                    if (contact.MatchedContact != null
                        && contact.MatchedContact.Equals(googleContact))
                    {
                        foundMatch = true;
                        break;
                    }
                }
                                                             
                if (!foundMatch)
                {
                    contactsToDelete.Add(new ContactActionDelete(_contactsManager.Request, googleContact));
                }
            }

            return contactsToDelete;
        }
    }
}
