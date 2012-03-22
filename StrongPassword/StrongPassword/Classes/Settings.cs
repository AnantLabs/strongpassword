using System;
using System.Xml.Serialization;
using System.IO;

namespace StrongPassword
{
    public class Settings : IFile
    {
        public string MySecret { get; set; }

        public void Save(IFile ifile)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));

            using (StreamWriter sw = new StreamWriter(SettingsHelper.GetPath(ifile), false))
            {
                serializer.Serialize(sw, ifile);
            }
        }

        public IFile Load()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Settings));

            using (StreamReader sr = new StreamReader(SettingsHelper.GetPath(new Settings())))
            {
                Settings settings = serializer.Deserialize(sr) as Settings;

                if (settings != null)
                    return settings;
            }

            throw new ApplicationException("Error loading settings");
        }
    }
}