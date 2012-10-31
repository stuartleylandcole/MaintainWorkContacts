using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Contacts;

namespace MaintainWorkContacts.Service
{
    public class ContactActionDelete : ContactAction
    {
        public ContactActionDelete(ContactsRequest request, Contact googleContact)
            : base(request, googleContact, googleContact.ToString() + " will be deleted.")
        {
        }

        public override void Action()
        {
            Request.Delete<Contact>(GoogleContact);
        }
    }
}
