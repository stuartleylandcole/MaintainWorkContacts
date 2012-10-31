using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MaintainWorkContacts.Properties;
using MaintainWorkContacts.Service;
using Google.GData.Client;
using Google.Contacts;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Specialized;

namespace MaintainWorkContacts.UI
{
    public partial class ContactsManagerForm : Form
    {
        private GoogleContactsManager _contactManager;
        private List<ContactAction> _actions;

        public ContactsManagerForm()
        {
            InitializeComponent();

            buttonPreviewChanges.Enabled = false;
            buttonApplyChanges.Enabled = false;

            PopulateSettings();
        }

        private void PopulateSettings()
        {
            textboxOohSheetLocation.Text = Settings.Default.OohSheetLocation;
            textBoxEmailAddress.Text = Settings.Default.EmailAddressUsername;
            textBoxPassword.Text = Utility.ToInsecureString(Utility.DecryptString(Settings.Default.EmailAddressPassword));
            textBoxGoogleWorkGroupName.Text = Settings.Default.GoogleWorkGroupName;

            StringCollection initialsToIgnoreSetting = Settings.Default.InitialsToIgnore;
            if (initialsToIgnoreSetting == null)
            {
                initialsToIgnoreSetting = new StringCollection();
            }

            string initialsToIgnore = string.Empty;
            foreach (string initials in initialsToIgnoreSetting)
            {
                if (!string.IsNullOrWhiteSpace(initialsToIgnore))
                {
                    initialsToIgnore += ",";
                }
                initialsToIgnore += initials;
            }

            textBoxInitialsToIgnore.Text = initialsToIgnore;
        }

        private void buttonSaveSettings_Click(object sender, EventArgs e)
        {
            Settings.Default.OohSheetLocation = textboxOohSheetLocation.Text;
            Settings.Default.EmailAddressUsername = textBoxEmailAddress.Text;
            Settings.Default.EmailAddressPassword = Utility.EncryptString(Utility.ToSecureString(textBoxPassword.Text));
            Settings.Default.GoogleWorkGroupName = textBoxGoogleWorkGroupName.Text;

            StringCollection initialsToIgnore = new StringCollection();
            initialsToIgnore.AddRange(textBoxInitialsToIgnore.Text.Split(','));
            Settings.Default.InitialsToIgnore = initialsToIgnore;

            Settings.Default.Save();

            buttonPreviewChanges.Enabled = true;
        }

        private void buttonUpdateContacts_Click(object sender, EventArgs e)
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.DoWork += new DoWorkEventHandler(worker_DoWork);
            worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
            worker.RunWorkerAsync();
        }

        void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            WorkerState state = new WorkerState();
            state.ProgressText = "Parsing word document...";

            BackgroundWorker worker = sender as BackgroundWorker;
            worker.ReportProgress(0, state);

            WordDocumentParser wordDocumentParser;
            try
            {
                wordDocumentParser = new WordDocumentParser(Settings.Default.OohSheetLocation);
            }
            catch (FileNotFoundException exception)
            {
                state.ThrownException = exception;
                state.ErrorMessage = "Could not find the word document. File path is: " + exception.FileName + ".";
                worker.ReportProgress(0, state);
                return;
            }

            List<WorkContact> oohSheetContacts = wordDocumentParser.GetContacts();

            try
            {
                _contactManager = GoogleContactsManager.CreateContactsManager();
            }
            catch (InvalidCredentialsException argumentException)
            {
                state.ThrownException = argumentException;
                state.ErrorMessage = "Invalid username and password were provided.";
                worker.ReportProgress(0, state);
                return;
            }
            catch (ArgumentNullException nullArg)
            {
                state.ThrownException = nullArg;

                string groupName = string.Empty;
                string paramName = nullArg.ParamName;
                if (paramName == "WorkGroup")
                {
                    groupName = Settings.Default.GoogleWorkGroupName;
                }
                else
                {
                    groupName = "My Contacts";
                }
                state.ErrorMessage = "There is no group called " + groupName + " in the Google account.";
                worker.ReportProgress(0, state);
                return;
            }

            state.ProgressText = "Comparing Google contacts to the word document...";
            worker.ReportProgress(0, state);

            var comparer = new ContactComparer(oohSheetContacts, _contactManager);
            _actions = comparer.Compare();

            state.ProgressText = "Finished comparing.";
            worker.ReportProgress(0, state);
        }

        void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            WorkerState state = e.UserState as WorkerState;
            if (!string.IsNullOrWhiteSpace(state.ErrorMessage))
            {
                MessageBox.Show(state.ErrorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            else
            {
                labelCurrentStatus.Text = state.ProgressText;
            }
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //if the actions aren't set, we didn't complete properly and the ReportProgress() method will deal with that.
            if (_actions == null)
            {
                return;
            }

            if (_actions.Count == 0)
            {
                textBoxChanges.Text = "There are no changes to be made.";
                buttonApplyChanges.Enabled = false;
            }
            else
            {
                textBoxChanges.Text = string.Empty;
                _actions.ForEach(action => textBoxChanges.Text += action.ChangeDescription + Environment.NewLine);
                buttonApplyChanges.Enabled = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            labelCurrentStatus.Text = "Updating Google...";

            //TODO: do this in a background worker thread.
            _contactManager.UpdateContacts(_actions);

            labelCurrentStatus.Text = "Done.";
        }

        internal class WorkerState
        {
            public Exception ThrownException { get; set; }
            public string ErrorMessage { get; set; }
            public string ProgressText { get; set; }
        }
    }
}
