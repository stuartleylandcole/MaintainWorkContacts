using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Contacts;
using Google.GData.Contacts;
using MaintainWorkContacts.Properties;

namespace MaintainWorkContacts.Service
{
    public class ContactActionAdd : ContactAction
    {
        public ContactActionAdd(ContactsRequest request, Contact googleContact)
            : base (request, googleContact, "A new contact will be created for " + googleContact.Name.FullName)
        {
        }

        public override void Action()
        {
            Uri feedUri = new Uri(ContactsQuery.CreateContactsUri(Settings.Default.EmailAddressUsername));
            Request.Insert<Contact>(feedUri, GoogleContact);
        }
    }
}
