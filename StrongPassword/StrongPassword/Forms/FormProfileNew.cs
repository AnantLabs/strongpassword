using System;
using System.Linq;
using System.Windows.Forms;

namespace StrongPassword
{
    public partial class FormProfileNew : Form
    {
        private readonly ForceCloseHandler forceCloseHandler;
        public Profile selected { get; private set; }

        public FormProfileNew(ForceCloseHandler forceCloseHandler)
        {
            InitializeComponent();
            this.forceCloseHandler = forceCloseHandler;

            numericSize.Maximum = 31;
            numericSize.Minimum = 1;
        }

        private void ButtonSaveClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text)) 
                return;

            Profile newData = new Profile { Name = textBoxName.Text, Size = (int)numericSize.Value };
            AProfiles loaded = new AProfiles();

            try
            {
                if (SettingsHelper.CheckProfileFile())                  //Only load current profiles if file exist
                {
                    loaded = SettingsHelper.Load(new AProfiles()) as AProfiles;       //Load Current profiles

                    if (loaded == null)
                        throw new ApplicationException("Error loading profiles");

                    if (loaded.Count(x => x.Name == newData.Name) > 0)             //Avoid adding dupes.
                    {
                        MessageBox.Show("Dupe..");
                        return;
                    }

                    loaded.Add(newData);                    //Add new profile
                    loaded.Sort((profile, aProfile) => String.CompareOrdinal(profile.Name, aProfile.Name));
                }
                else
                    loaded.Add(newData);                    //Add new profile

                SettingsHelper.Save(loaded);            //Save profiles
                selected = newData;

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                Close();
                forceCloseHandler.Run();
            }
        }
    }
}