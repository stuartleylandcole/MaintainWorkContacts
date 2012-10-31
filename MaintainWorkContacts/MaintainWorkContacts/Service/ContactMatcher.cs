using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Contacts;

namespace MaintainWorkContacts.Service
{
    public class ContactMatcher
    {
        private List<Contact> _googleContacts;

        public ContactMatcher(List<Contact> googleContacts)
        {
            _googleContacts = googleContacts;
        }

        public Contact Match(WorkContact contact)
        {
            Contact matchedContact = FindContactFirstNameSurnameAndInitials(contact);
            if (matchedContact != null)
            {
                return matchedContact;
            }

            matchedContact = FindContactFirstNameAndInitials(contact);
            if (matchedContact != null)
            {
                return matchedContact;
            }

            matchedContact = FindContactFirstNameAndSurname(contact);
            if (matchedContact != null)
            {
                return matchedContact;
            }

            matchedContact = FindContactInitials(contact);
            if (matchedContact != null)
            {
                return matchedContact;
            }

            matchedContact = FindContactFirstNameAndPhoneNumber(contact);
            if (matchedContact != null)
            {
                return matchedContact;
            }

            return null;
        }

        private Contact FindContactFirstNameSurnameAndInitials(WorkContact contact)
        {
            return (from c in _googleContacts
                    where c.Name.GivenName == contact.Firstname
                    && c.GetSurname() == contact.Surname
                    && c.GetInitials() == contact.Initials
                    select c).FirstOrDefault();
        }

        private Contact FindContactFirstNameAndInitials(WorkContact contact)
        {
            return (from c in _googleContacts
                    where c.Name.GivenName == contact.Firstname
                    && c.GetInitials() == contact.Initials
                    select c).FirstOrDefault();
        }

        private Contact FindContactFirstNameAndSurname(WorkContact contact)
        {
            return (from c in _googleContacts
                    where c.Name.GivenName == contact.Firstname
                    && c.GetSurname() == contact.Surname
                    select c).FirstOrDefault();
        }

        private Contact FindContactInitials(WorkContact contact)
        {
            return (from c in _googleContacts
                    where c.GetInitials() == contact.Initials
                    select c).FirstOrDefault();
        }

        private Contact FindContactFirstNameAndPhoneNumber(WorkContact contact)
        {
            return (from c in _googleContacts
                    where c.Name.GivenName == contact.Firstname
                    && c.GetMobilePhoneNumberValue() == contact.Mobile
                    select c).FirstOrDefault();
        }
    }
}
