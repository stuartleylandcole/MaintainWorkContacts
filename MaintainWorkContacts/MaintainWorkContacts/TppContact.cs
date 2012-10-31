using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Contacts;

namespace MaintainWorkContacts
{
    public class WorkContact
    {
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Initials { get; set; }
        public string Mobile { get; set; }
        public string HomePhoneNumber { get; set; }
        public WorkGroup Group { get; set; }
        public Contact MatchedContact { get; set; }

        private WorkContact()
        {
        }

        public override string ToString()
        {
            return Firstname + " " + Surname + "(" + Initials + "), home: " + HomePhoneNumber + ", mobile: " + Mobile;
        }

        public string FullName
        {
            get
            {
                string fullname = Firstname;
                if (!string.IsNullOrWhiteSpace(Surname))
                {
                    fullname += " " + Surname;
                }
                fullname += " (" + Initials + ")";

                return fullname;
            }
        }

        public static WorkContact FactoryCreate(string firstname, string surname, string initials, 
                                               string mobile, string homePhoneNumber, WorkGroup group)
        {
            return new WorkContact
            {
                Firstname = firstname,
                Surname = surname,
                Initials = initials,
                Mobile = mobile,
                HomePhoneNumber = homePhoneNumber,
                Group = group,
            };
        }
    }
}
