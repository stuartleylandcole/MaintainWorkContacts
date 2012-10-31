using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Contacts;

namespace MaintainWorkContacts.Service
{
    class ContactActionUpdate : ContactAction
    {
        public ContactActionUpdate(ContactsRequest request, Contact googleContact, string description) 
            : base(request, googleContact, description)
        {
        }

        public override void Action()
        {
            Request.Update<Contact>(GoogleContact);
        }
    }
}
