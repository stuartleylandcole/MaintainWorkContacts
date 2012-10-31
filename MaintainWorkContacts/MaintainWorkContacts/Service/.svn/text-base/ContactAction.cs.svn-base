using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Contacts;

namespace MaintainWorkContacts.Service
{
    public abstract class ContactAction
    {
        protected ContactAction(ContactsRequest request, Contact googleContact, string description)
        {
            Request = request;
            GoogleContact = googleContact;
            ChangeDescription = description;
        }

        public abstract void Action();

        public string ChangeDescription { get; protected set; }

        public Contact GoogleContact { get; protected set; }

        protected ContactsRequest Request { get; private set; }
    }
}
