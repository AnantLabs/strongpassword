using System;
using System.Windows.Forms;

namespace StrongPassword
{
    public partial class FormMasterKeyWord : Form
    {
        private readonly ForceCloseHandler forceCloseHandler;

        public FormMasterKeyWord(ForceCloseHandler forceCloseHandler)
        {
            InitializeComponent();

            forceCloseHandler.ForceCloseEvent += ForceMasterKeyWordsClose;
            this.forceCloseHandler = forceCloseHandler;

            if (SettingsHelper.CheckSettingsFile())
                MessageBox.Show("Warning! Changing the master keywords will effect every password!");
        }


        private void ButtonSaveClick(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBoxMasterPassword.Text)) //Don´t save a empty master keywords
            {
                MessageBox.Show("You need to enter a master keyword");
                return;
            }

            try
            {
                SettingsHelper.Save(new Settings
                {
                    MySecret = CryptWrapper.Generate.EncryptSettings(textBoxMasterPassword.Text)
                });

                ForceMasterKeyWordsClose();
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message);
                forceCloseHandler.Run();
            }
        }

        private void ForceMasterKeyWordsClose()
        {
            FormCloseClick(new ForceClose(), new FormClosingEventArgs(CloseReason.ApplicationExitCall, true));
        }

        private void FormCloseClick(object sender, FormClosingEventArgs e)
        {
            if (sender is ForceClose)
            {
                FormClosing -= FormCloseClick;
                forceCloseHandler.ForceCloseEvent -= ForceMasterKeyWordsClose;

                Close();
            }
            else
            {
                if (!SettingsHelper.CheckSettingsFile()) //Don´t close if we don´t have a master password file
                    e.Cancel = true;
            }
        }
    }
}