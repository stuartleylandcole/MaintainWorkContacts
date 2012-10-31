using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Word = Microsoft.Office.Interop.Word;
using System.IO;

namespace MaintainWorkContacts.Service
{
    public class WordDocumentParser
    {
        private enum ColumnIndexes
        {
            NameAndInitials = 1,
            Mobile = 2,
            Home = 3
        }

        private string _fileLocation;

        public WordDocumentParser(string fileLocation)
        {
            if (!File.Exists(fileLocation))
            {
                throw new FileNotFoundException("The file could not be found", fileLocation);
            }

            _fileLocation = fileLocation;
        }

        public List<WorkContact> GetContacts()
        {
            Word.Application word = new Word.Application();
            try
            {
                return CreateContacts(word);
            }
            finally
            {
                ((Word._Application)word).Quit(SaveChanges: false, OriginalFormat: false, RouteDocument: false);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(word);
            }
        }

        private List<WorkContact> CreateContacts(Word.Application word)
        {
            List<WorkContact> contacts = new List<WorkContact>();

            Word.Table contactsTable = GetContactsTable(word);

            WorkGroup latestGroup = WorkGroup.Unknown;
            foreach (Word.Row row in contactsTable.Rows)
            {
                latestGroup = GetLatestGroup(row, latestGroup);

                WorkContact contact = CreateContact(row, latestGroup);
                if (contact != null)
                {
                    contacts.Add(contact);
                }
            }
            return contacts;
        }

        private Word.Table GetContactsTable(Word.Application word)
        {
            try
            {
                Word.Document document = word.Documents.Open(_fileLocation, ReadOnly: false, Visible: false);
                document.Activate();

                return document.Tables[2];
            }
            catch (Exception e)
            {
                Console.WriteLine("Had the following exception: " + e.Message);
                Console.WriteLine(e.StackTrace);
            }

            return null;
        }

        private WorkGroup GetLatestGroup(Word.Row row, WorkGroup currentGroup)
        {
            //if neither of the numbers are set then we probably have a heading ("probably" because some of the trainers don't have numbers)
            string mobile = GetMobileNumber(row);
            string home = GetHomeNumber(row);
            if (string.IsNullOrWhiteSpace(mobile) && string.IsNullOrWhiteSpace(home))
            {
                string group = GetNameAndInitials(row);
                WorkGroup newGroup = Utility.GetGroup(group);
                if (newGroup != WorkGroup.Unknown)
                {
                    //means that the group has changed.
                    return newGroup;
                }
            }

            //we're not on a group row so just return the current group
            return currentGroup;
        }

        private WorkContact CreateContact(Word.Row row, WorkGroup latestGroup)
        {
            try
            {
                //Trainers, Misc and Office have strange formats so don't do them
                if (latestGroup != WorkGroup.Trainer && latestGroup != WorkGroup.Misc && latestGroup != WorkGroup.Office)
                {
                    string nameAndInitials = GetNameAndInitials(row);
                    string[] nameFields = nameAndInitials.Split(' ');
                    string firstname = GetFirstName(nameFields);
                    string surname = GetSurname(nameFields);
                    string initials = GetInitials(nameFields);
                    string mobile = GetMobileNumber(row);
                    string home = GetHomeNumber(row);

                    //if there is at least one number then we can reasonably assume it's a contact
                    if (!string.IsNullOrWhiteSpace(mobile) || !string.IsNullOrWhiteSpace(home))
                    {
                        return WorkContact.FactoryCreate(firstname, surname, initials, mobile, home, latestGroup);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Had the following exception: " + e.Message);
                Console.WriteLine(e.StackTrace);
            }
            return null;
        }

        private string GetNameAndInitials(Word.Row row)
        {
            string nameAndInitials = row.Cells[(int)ColumnIndexes.NameAndInitials].Range.Text;
            return RemoveEmptyRowCharacters(nameAndInitials);
        }

        private string GetMobileNumber(Word.Row row)
        {
            return GetPhoneNumber(row, ColumnIndexes.Mobile);
        }

        private string GetHomeNumber(Word.Row row)
        {
            return GetPhoneNumber(row, ColumnIndexes.Home);
        }

        private string GetPhoneNumber(Word.Row row, ColumnIndexes index)
        {
            string phoneNumber = row.Cells[(int)index].Range.Text;
            phoneNumber = RemoveEmptyRowCharacters(phoneNumber);
            return Utility.RemoveSpaces(phoneNumber);
        }

        private string RemoveEmptyRowCharacters(string text)
        {
            text = text.Replace('\r', ' ').Replace('\a', ' ').Trim();
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }
            return text;
        }

        private string GetFirstName(string[] fields)
        {
            switch (fields.Count())
            {
                case 1:
                case 2:
                case 3:
                    return fields[0];
                case 4:
                    return fields[0] + " " + fields[1];
                default:
                    throw new NotSupportedException("Cannot get first name for [" + PrintArray(fields) + "]");
            }
        }

        private string GetSurname(string[] fields)
        {
            switch (fields.Count())
            {
                case 1:
                case 2:
                    return string.Empty;
                case 3:
                    return fields[1];
                case 4:
                    return fields[2];
                default:
                    throw new NotSupportedException("Cannot get surname for [" + PrintArray(fields) + "]");
            }
        }

        private string GetInitials(string[] fields)
        {
            switch (fields.Count())
            {
                case 1:
                    return string.Empty;
                case 2:
                case 3:
                case 4:
                    int indexToUse = fields.Count() - 1;
                    try
                    {
                        return fields[indexToUse].Substring(1, fields[indexToUse].Length - 2);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        return string.Empty;
                    }
                default:
                    throw new NotSupportedException("Cannot get initials for [" + PrintArray(fields) + "]");
            }
        }

        private string PrintArray(string[] array)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                if (sb.Length > 0)
                {
                    sb.Append(" ");
                }
                sb.Append(array[i]);
            }
            return sb.ToString();
        }
    }
}
