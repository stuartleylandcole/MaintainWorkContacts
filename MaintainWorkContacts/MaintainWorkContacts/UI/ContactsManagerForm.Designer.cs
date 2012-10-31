using System;
namespace MaintainWorkContacts.UI
{
    partial class ContactsManagerForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonSaveSettings = new System.Windows.Forms.Button();
            this.buttonPreviewChanges = new System.Windows.Forms.Button();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.textBoxEmailAddress = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.textboxOohSheetLocation = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBoxChanges = new System.Windows.Forms.TextBox();
            this.buttonApplyChanges = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.labelCurrentStatus = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxInitialsToIgnore = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxGoogleWorkGroupName = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonSaveSettings
            // 
            this.buttonSaveSettings.Location = new System.Drawing.Point(123, 226);
            this.buttonSaveSettings.Name = "buttonSaveSettings";
            this.buttonSaveSettings.Size = new System.Drawing.Size(98, 23);
            this.buttonSaveSettings.TabIndex = 20;
            this.buttonSaveSettings.Text = "Save settings";
            this.buttonSaveSettings.UseVisualStyleBackColor = true;
            this.buttonSaveSettings.Click += new System.EventHandler(this.buttonSaveSettings_Click);
            // 
            // buttonPreviewChanges
            // 
            this.buttonPreviewChanges.Location = new System.Drawing.Point(280, 226);
            this.buttonPreviewChanges.Name = "buttonPreviewChanges";
            this.buttonPreviewChanges.Size = new System.Drawing.Size(121, 23);
            this.buttonPreviewChanges.TabIndex = 19;
            this.buttonPreviewChanges.Text = "Preview Changes";
            this.buttonPreviewChanges.UseVisualStyleBackColor = true;
            this.buttonPreviewChanges.Click += new System.EventHandler(this.buttonUpdateContacts_Click);
            // 
            // textBoxPassword
            // 
            this.textBoxPassword.Location = new System.Drawing.Point(123, 64);
            this.textBoxPassword.Name = "textBoxPassword";
            this.textBoxPassword.PasswordChar = '*';
            this.textBoxPassword.Size = new System.Drawing.Size(278, 20);
            this.textBoxPassword.TabIndex = 18;
            // 
            // textBoxEmailAddress
            // 
            this.textBoxEmailAddress.Location = new System.Drawing.Point(123, 38);
            this.textBoxEmailAddress.Name = "textBoxEmailAddress";
            this.textBoxEmailAddress.Size = new System.Drawing.Size(278, 20);
            this.textBoxEmailAddress.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 16;
            this.label3.Text = "Google Password";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Google email address";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(407, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(24, 23);
            this.button1.TabIndex = 14;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // textboxOohSheetLocation
            // 
            this.textboxOohSheetLocation.Location = new System.Drawing.Point(123, 12);
            this.textboxOohSheetLocation.Name = "textboxOohSheetLocation";
            this.textboxOohSheetLocation.Size = new System.Drawing.Size(278, 20);
            this.textboxOohSheetLocation.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "OOH sheet location";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxChanges);
            this.groupBox1.Location = new System.Drawing.Point(12, 278);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(419, 252);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Changes";
            // 
            // textBoxChanges
            // 
            this.textBoxChanges.Location = new System.Drawing.Point(6, 19);
            this.textBoxChanges.Multiline = true;
            this.textBoxChanges.Name = "textBoxChanges";
            this.textBoxChanges.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxChanges.Size = new System.Drawing.Size(407, 227);
            this.textBoxChanges.TabIndex = 0;
            // 
            // buttonApplyChanges
            // 
            this.buttonApplyChanges.Location = new System.Drawing.Point(12, 536);
            this.buttonApplyChanges.Name = "buttonApplyChanges";
            this.buttonApplyChanges.Size = new System.Drawing.Size(105, 23);
            this.buttonApplyChanges.TabIndex = 22;
            this.buttonApplyChanges.Text = "Apply Changes";
            this.buttonApplyChanges.UseVisualStyleBackColor = true;
            this.buttonApplyChanges.Click += new System.EventHandler(this.button2_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 262);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "Status:";
            // 
            // labelCurrentStatus
            // 
            this.labelCurrentStatus.Location = new System.Drawing.Point(55, 262);
            this.labelCurrentStatus.Name = "labelCurrentStatus";
            this.labelCurrentStatus.Size = new System.Drawing.Size(300, 13);
            this.labelCurrentStatus.TabIndex = 24;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(9, 98);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(392, 30);
            this.label5.TabIndex = 25;
            this.label5.Text = "Add the initials of any work staff that you don\'t want to create contacts for her" +
    "e.\rSeparate the initials with a comma, eg: SCL,PC";
            // 
            // textBoxInitialsToIgnore
            // 
            this.textBoxInitialsToIgnore.Location = new System.Drawing.Point(12, 131);
            this.textBoxInitialsToIgnore.Name = "textBoxInitialsToIgnore";
            this.textBoxInitialsToIgnore.Size = new System.Drawing.Size(389, 20);
            this.textBoxInitialsToIgnore.TabIndex = 26;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(9, 166);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(392, 31);
            this.label6.TabIndex = 27;
            this.label6.Text = "Ensure you have a contacts group in Google Contacts just for work staff.\r\nThen pu" +
    "t the name of the group in the box below.";
            // 
            // textBoxGoogleWorkGroupName
            // 
            this.textBoxGoogleWorkGroupName.Location = new System.Drawing.Point(12, 200);
            this.textBoxGoogleWorkGroupName.Name = "textBoxGoogleWorkGroupName";
            this.textBoxGoogleWorkGroupName.Size = new System.Drawing.Size(389, 20);
            this.textBoxGoogleWorkGroupName.TabIndex = 28;
            // 
            // ContactsManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(439, 592);
            this.Controls.Add(this.textBoxGoogleWorkGroupName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxInitialsToIgnore);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.labelCurrentStatus);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.buttonApplyChanges);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.buttonSaveSettings);
            this.Controls.Add(this.buttonPreviewChanges);
            this.Controls.Add(this.textBoxPassword);
            this.Controls.Add(this.textBoxEmailAddress);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textboxOohSheetLocation);
            this.Controls.Add(this.label1);
            this.Name = "ContactsManagerForm";
            this.Text = "Contacts Manager";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSaveSettings;
        private System.Windows.Forms.Button buttonPreviewChanges;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.TextBox textBoxEmailAddress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textboxOohSheetLocation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonApplyChanges;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label labelCurrentStatus;
        private System.Windows.Forms.TextBox textBoxChanges;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxInitialsToIgnore;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxGoogleWorkGroupName;
    }
}