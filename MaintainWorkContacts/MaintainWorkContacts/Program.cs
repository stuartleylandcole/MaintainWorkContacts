using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.GData.Client;
using Google.Contacts;
using System.Drawing;
using System.Windows.Forms;
using MaintainWorkContacts.UI;

namespace MaintainWorkContacts
{
    class Program
    {
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.Run(new ContactsManagerForm());
        }
    }
}
