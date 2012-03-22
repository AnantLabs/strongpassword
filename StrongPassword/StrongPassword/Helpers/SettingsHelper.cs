using System;
using System.Windows.Forms;
using System.IO;

namespace StrongPassword
{
    public static class SettingsHelper
    {
        private static readonly string RootLocation = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        private static readonly string PathLocation = RootLocation + @"\StrongPassword";
        private static readonly string SettingsLocation = PathLocation + @"\Settings.xml";
        private static readonly string ProfileLocation = PathLocation + @"\Profile.xml";

        public static bool CheckProfileFile()
        {
            return File.Exists(ProfileLocation);
        }

        public static bool CheckSettingsFile()
        {
            return File.Exists(SettingsLocation);
        }

        public static void Save(IFile ifile)
        {
            if (!Directory.Exists(PathLocation))
                Directory.CreateDirectory(PathLocation);
            
            try
            {
                ifile.Save(ifile);
            }
            catch (Exception e)
            {
                Error(e);
                throw;
            }
        }

        public static IFile Load(IFile ifile)
        {
            try
            {
                return ifile.Load();
            }
            catch(Exception e)
            {
                Error(e);
                throw;
            }
        }

        public static string GetPath(IFile ifile)
        {
            if (ifile is Settings)
                return SettingsLocation;
            if (ifile is AProfiles)
                return ProfileLocation;

            throw new ApplicationException("Wrong type..");
        }

        private static void Error(Exception e)
        {
            MessageBox.Show(e.Message, "File error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (e.InnerException != null)
                MessageBox.Show(e.InnerException.Message, "InnerException", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
