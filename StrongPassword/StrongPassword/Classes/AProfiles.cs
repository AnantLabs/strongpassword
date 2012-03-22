using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

namespace StrongPassword
{
    [XmlInclude(typeof(Profile))]
    public class AProfiles : List<AProfile>, IFile
    {
        public void Save(IFile ifile)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(AProfiles));

            using (StreamWriter sw = new StreamWriter(SettingsHelper.GetPath(ifile), false))
            {
                serializer.Serialize(sw, ifile);
            }
        }

        public IFile Load()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(AProfiles));

            using (StreamReader sr = new StreamReader(SettingsHelper.GetPath(new AProfiles())))
            {
                AProfiles aProfiles = serializer.Deserialize(sr) as AProfiles;

                if (aProfiles != null)
                    return aProfiles;
            }

            throw new ApplicationException("Error loading profile");
        }
    }
}