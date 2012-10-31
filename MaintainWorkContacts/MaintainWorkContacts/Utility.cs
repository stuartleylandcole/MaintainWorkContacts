using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.GData.Client;
using System.Security;
using System.Security.Cryptography;

namespace MaintainWorkContacts
{
    public static class Utility
    {
        private static byte[] salt = Encoding.Unicode.GetBytes("SomeRandomBitOfText");

        public static WorkGroup GetGroup(string group)
        {
            switch (group)
            {
                case "Directors":
                    return WorkGroup.Director;
                case "Coders":
                    return WorkGroup.Coder;
                case "Tech":
                    return WorkGroup.Tech;
                case "CSAs":
                    return WorkGroup.CSA;
                case "Imports":
                    return WorkGroup.Import;
                case "Customer Relations":
                    return WorkGroup.CRM;
                case "Others":
                    return WorkGroup.Other;
                case "Trainers":
                    return WorkGroup.Trainer;
                case "Misc":
                    return WorkGroup.Misc;
                case "Office":
                    return WorkGroup.Office;
                default:
                    return WorkGroup.Unknown;
            }
        }

        public static string RemoveSpaces(string text)
        {
            return text.Replace(" ", "");
        }

        public static void LogGoogleException(GDataRequestException e)
        {
            Console.WriteLine("Had the following exception: " + e.Message + " and response string: " + e.ResponseString);
            Console.WriteLine(e);
        }

        public static string EncryptString(SecureString input)
        {
            byte[] encryptedData = ProtectedData.Protect(Encoding.Unicode.GetBytes(ToInsecureString(input)),
                                                         salt,
                                                         DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encryptedData);
        }

        public static SecureString DecryptString(string encryptedData)
        {
            try
            {
                byte[] decryptedData = ProtectedData.Unprotect(Convert.FromBase64String(encryptedData),
                                                               salt,
                                                               DataProtectionScope.CurrentUser);
                return ToSecureString(Encoding.Unicode.GetString(decryptedData));
            }
            catch
            {
                return new SecureString();
            }
        }

        public static SecureString ToSecureString(string input)
        {
            SecureString secure = new SecureString();
            foreach (char c in input)
            {
                secure.AppendChar(c);
            }
            secure.MakeReadOnly();
            return secure;
        }

        public static string ToInsecureString(SecureString input)
        {
            string retval = string.Empty;
            IntPtr ptr = System.Runtime.InteropServices.Marshal.SecureStringToBSTR(input);
            try
            {
                retval = System.Runtime.InteropServices.Marshal.PtrToStringBSTR(ptr);
            }
            finally
            {
                System.Runtime.InteropServices.Marshal.ZeroFreeBSTR(ptr);
            }
            return retval;
        }
    }
}
