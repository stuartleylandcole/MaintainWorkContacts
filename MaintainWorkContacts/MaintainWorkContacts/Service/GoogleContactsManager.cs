using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Contacts;
using Google.GData.Contacts;
using Google.GData.Client;
using MaintainWorkContacts.Properties;

namespace MaintainWorkContacts.Service
{
    public class GoogleContactsManager
    {
        public ContactsRequest Request { get; private set; }
        public Group WorkGroup { get; private set; }
        public Group MyContactsGroup { get; private set; }

        private GoogleContactsManager()
        {
        }

        public List<Contact> GetContacts()
        {
            string uri = ContactsQuery.CreateContactsUri("default");
            ContactsQuery query = new ContactsQuery(uri);
            query.Group = WorkGroup.Id;

            return Request.Get<Contact>(query).Entries.ToList();
        }

        public void UpdateContacts(List<ContactAction> actions)
        {
            foreach (ContactAction action in actions)
            {
                try
                {
                    action.Action();
                }
                catch (GDataRequestException exception)
                {
                    Utility.LogGoogleException(exception);
                }
            }
        }

        public static GoogleContactsManager CreateContactsManager()
        {
            var settings = new RequestSettings("Maintain Work Contacts",
                                               Settings.Default.EmailAddressUsername,
                                               Utility.ToInsecureString(Utility.DecryptString(Settings.Default.EmailAddressPassword)));
            settings.AutoPaging = true;
            var request = new ContactsRequest(settings);

            IEnumerable<Group> groups;
            try
            {
                groups = request.GetGroups().Entries;

                string groupName = Settings.Default.GoogleWorkGroupName;
                var workGroup = groups.Where(g => g.Title.Contains(groupName)).FirstOrDefault();
                if (workGroup == null)
                {
                    throw new ArgumentNullException("WorkGroup", "There is no group called " + groupName + " in the Google account.");
                }

                var myContactsGroup = groups.Where(g => g.Title.Contains("My Contacts")).FirstOrDefault();
                if (myContactsGroup == null)
                {
                    throw new ArgumentNullException("MyContactsGroup", "There is no group called My Contacts in the Google account.");
                }

                GoogleContactsManager contactsManager = new GoogleContactsManager();
                contactsManager.Request = request;
                contactsManager.MyContactsGroup = myContactsGroup;
                contactsManager.WorkGroup = workGroup;
                return contactsManager;
            }
            catch (GDataRequestException exception)
            {
                throw new InvalidCredentialsException("Invalid username and password were provided.", exception);
            }
        }
    }
}
