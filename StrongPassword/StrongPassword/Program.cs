using System;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;
using System.Runtime.InteropServices;

namespace StrongPassword
{
    public static class Program
    {
        [STAThread]
        public static void Main(string[] arg)
        {
            bool instance;
            string name = String.Empty;

            //Get GUID or The Application name
            try
            {
                object[] attribute = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), true);

                GuidAttribute guidAttribute = attribute[0] as GuidAttribute;
                
                if (guidAttribute != null) 
                    name = guidAttribute.Value;
            }
            catch //Should never happend but you never known...
            {
                name = Application.ProductName;
            }

            //Using mutex to only start one instance (using GUID as unique namne)
            using (new Mutex(false, @"Local\" + name, out instance))
            {
                if (!instance)
                {
                    MessageBox.Show("You are trying to open a second instance of this program!");
                }
                else
                {

                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    //Instance the mainform with the silent argument
                    FormMain main = new FormMain((arg != null && arg.Contains("-silent", StringComparer.OrdinalIgnoreCase)));

                    if (!main.IsDisposed) //Fix ObjectDisposedException (Thanks google;))
                    {
                        Application.Run(main);
                    }
                }
            }
        }
    }
}