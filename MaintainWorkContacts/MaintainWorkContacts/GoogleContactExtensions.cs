using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Contacts;
using Google.GData.Extensions;

namespace MaintainWorkContacts
{
    public static class GoogleContactExtensions
    {
        public static string GetSurname(this Contact contact)
        {
            string surname = contact.Name.FamilyName;
            if (string.IsNullOrWhiteSpace(surname))
            {
                return string.Empty;
            }
            return surname;
        }

        public static string GetInitials(this Contact contact)
        {
            string initialsWithBrackets = contact.Name.NameSuffix;
            if (string.IsNullOrWhiteSpace(initialsWithBrackets))
            {
                return string.Empty;
            }
            return initialsWithBrackets.Substring(1, initialsWithBrackets.Count() - 2);
        }

        public static PhoneNumber GetHomePhoneNumber(this Contact contact)
        {
            return contact.Phonenumbers.Where(n => !n.Value.StartsWith("07")).FirstOrDefault();
        }

        public static PhoneNumber GetMobilePhoneNumber(this Contact contact)
        {
            return contact.Phonenumbers.Where(n => n.Value.StartsWith("07")).FirstOrDefault();
        }

        public static string GetMobilePhoneNumberValue(this Contact contact)
        {
            PhoneNumber phoneNumber = GetMobilePhoneNumber(contact);
            if (phoneNumber == null)
            {
                return string.Empty;
            }
            return Utility.RemoveSpaces(phoneNumber.Value);
        }

        public static string GetHomePhoneNumberValue(this Contact contact)
        {
            PhoneNumber phoneNumber = GetHomePhoneNumber(contact);
            if (phoneNumber == null)
            {
                return string.Empty;
            }
            return Utility.RemoveSpaces(phoneNumber.Value);
        }
    }
}
