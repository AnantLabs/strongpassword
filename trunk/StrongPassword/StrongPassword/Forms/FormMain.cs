using System;
using System.Linq;
using System.Drawing;
using System.Windows.Forms;

namespace StrongPassword
{
    public partial class FormMain : Form
    {
        private string settingsPassword;
        private readonly Timer clearTimer;
        private readonly bool silent;
        private readonly QueuedBackgroundWorker queuedBackgroundWorker;
        private ForceCloseHandler forceCloseHandler;

        //Hide the window on start (used on autostart)
        public FormMain(bool silent) : this()
        {
            this.silent = silent;
        }

        public FormMain()
        {
            InitializeComponent();
            forceCloseHandler = new ForceCloseHandler();
            forceCloseHandler.ForceCloseEvent += () => FormCloseClick(new ForceClose(), new FormClosingEventArgs(CloseReason.ApplicationExitCall, true));
            
            notifyIcon1.Icon = Icon;

            queuedBackgroundWorker = new QueuedBackgroundWorker(UpdateStrongPassword);

            SetupProfiles();

            if (!CheckSettingsFirstTime())
            {
                forceCloseHandler.Run();
                return;
            }

            clearTimer = new Timer();
            clearTimer.Tick += ButtonClearClick;

            Location = new Point(Screen.PrimaryScreen.Bounds.Width - Bounds.Width - 20, 
                                 Screen.PrimaryScreen.Bounds.Height - Bounds.Height - 60);
        }

        private void UpdateStrongPassword(string value)
        {
            textBoxStrong.Text = value;

            if (!string.IsNullOrEmpty(textBoxStrong.Text))
            {
                Clipboard.SetText(textBoxStrong.Text);
                clearTimer.Interval = 60000;
                clearTimer.Start();
            }
            else
            {
                Clipboard.Clear();
                clearTimer.Stop();
            }
        }

        //Check if settings exist or make user do settings and reload
        private bool CheckSettingsFirstTime()
        {
            if (SettingsHelper.CheckSettingsFile())
                LoadSettings();
            else
            {
                MessageBox.Show("You need to enter master keywords", "First time user", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (IsDisposed) //Check if users exit the program after the messagebox
                    return false;

                new FormMasterKeyWord(forceCloseHandler).ShowDialog(this);

                return !forceCloseHandler.IsClosing && CheckSettingsFirstTime();
            }
            return true;
        }

        //Initilize the settings
        private void LoadSettings()
        {
            try
            {
                Settings settings = SettingsHelper.Load(new Settings()) as Settings;

                if (settings == null)
                    throw  new ApplicationException();

                settingsPassword = settings.MySecret;
            }
            catch
            {
                forceCloseHandler.Run();
            }    
        }

        //Initilize the profiles
        private void SetupProfiles(Profile profile = null)
        {
            comboBoxProfile.SelectedIndexChanged -= ComboBoxProfileSelectedIndexChanged;

            AProfiles aprofiles = new AProfiles {new Profile {Name = "Strong", Size = 31}, new ProfileNew {Name = "New"}};

            if (SettingsHelper.CheckProfileFile())
            {
                try
                {
                    AProfiles loadedProfiles = SettingsHelper.Load(new AProfiles()) as AProfiles;

                    if (loadedProfiles == null)
                        throw  new ApplicationException();

                    aprofiles.AddRange(loadedProfiles);
                }
                catch
                {
                    forceCloseHandler.Run();
                }
            }

            comboBoxProfile.DataSource = aprofiles;
            comboBoxProfile.SelectedItem = profile ?? new Profile {  Name = "Strong" };
            comboBoxProfile.SelectedIndexChanged += ComboBoxProfileSelectedIndexChanged;
        }

        //Generate new strongpassword from a weakpassword
        private void TextBoxWeakTextChanged(object sender, EventArgs e)
        {
            Profile profile = comboBoxProfile.SelectedItem as Profile;

            if (profile != null)
                queuedBackgroundWorker.Add(textBoxWeak.Text, settingsPassword, profile.Size);
        }

        //Hide or Closes the main window depending on the sender
        private void FormCloseClick(object sender, FormClosingEventArgs e)
        {
            if (sender is ForceClose)
            {
                FormClosing -= FormCloseClick;
                Close();
            }
            else
            {
                Hide();
                e.Cancel = true;
            }
        }

        //Clear all fields and clipboard
        private void ButtonClearClick(object sender, EventArgs e)
        {
            if (Clipboard.GetText() == textBoxStrong.Text)
                Clipboard.Clear();

            textBoxWeak.Text = string.Empty;
        }

        //Click on the notification icon
        private void NotifyIcon1Click(object sender, EventArgs e)
        {
            foreach (Form form in OwnedForms.Where(form => (form is FormMasterKeyWord) && form.Visible))
            {
                form.Activate();
                return;
            }

            Show();
        }

        //Show the aboutform
        private void AboutToolStripMenuItemClick(object sender, EventArgs e)
        {
            new FormAbout().Show();
        }

        //Exit the program
        private void ExitToolStripMenuItemClick(object sender, EventArgs e)
        {
            ButtonClearClick(this, new EventArgs());
            forceCloseHandler.Run();  
        }

        //Change the master keywords, opening the FormMasterPassword
        private void ChangeMasterKeyWordToolStripMenuItemClick(object sender, EventArgs e)
        {
            foreach (Form form in OwnedForms.Where(form => (form is FormMasterKeyWord) && form.Visible))
            {
                form.Activate();
                return;
            }

            using (new FormMasterKeyWord(forceCloseHandler))
            {
                if (IsDisposed)
                    return;

                ShowDialog(this);
            }

            LoadSettings();
            ButtonClearClick(this, new EventArgs());
        }

        //Start hidden if silent start
        private void FormLoad(object sender, EventArgs e)
        {
            if (silent)
            {
                BeginInvoke(new MethodInvoker(Hide));
            }
        }

        //Prevent minimize/maximizing
        private void FormSize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
                Hide();
            }
            else
                WindowState = FormWindowState.Normal;
        }

        //Combobox profile changed
        private void ComboBoxProfileSelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxProfile.SelectedItem is ProfileNew)
            {
                FormProfileNew formProfileNew = new FormProfileNew(forceCloseHandler);
                formProfileNew.ShowDialog();
                SetupProfiles(formProfileNew.selected);
            }

            TextBoxWeakTextChanged(this, new EventArgs());

        }
    }
}