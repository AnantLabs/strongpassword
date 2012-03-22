/*
Dump of file BCrypt.dll

File Type: DLL

  Section contains the following exports for BCrypt.dll

    00000000 characteristics
    4F576438 time date stamp Wed Mar 07 14:35:52 2012
        0.00 version
           1 ordinal base
           1 number of functions
           1 number of names

    ordinal hint RVA      name

          1    0 00001450 _bcrypt@8

  Summary

        2000 .data
        B000 .rdata
        1000 .reloc
        1000 .rsrc
        4000 .text
*/
using System;
using System.Runtime.InteropServices;

namespace CryptWrapper
{
    public static class Generate
    {
        /// <summary>
        /// Bcrypt hash function
        /// 
        /// C Function
        /// char* bcrypt(const char *key, const char *salt)
        /// </summary>
        /// <param name="key">Plainword End with \0</param>
        /// <param name="salt">Salt End with \0</param>
        /// <returns>Bcrypt Hash</returns>
        [DllImport("BCrypt.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern string bcrypt(Char[] key, Char[] salt);

        public static string Encrypt(string weakkey,string settingSalt)
        {
            if (string.IsNullOrEmpty(weakkey))
                return string.Empty;
            else if (string.IsNullOrEmpty(settingSalt))
                throw new ApplicationException("Setting is empty");

            char[] key = (weakkey + '\0').ToCharArray();
            char[] salt = ("$2a$10$" + settingSalt + '\0').ToCharArray();

            return bcrypt(key, salt).Substring(settingSalt.Length - 2);
        }

        public static string EncryptSettings(string settingSalt)
        {
            if (string.IsNullOrEmpty(settingSalt))
                throw new ApplicationException("Setting is empty");

            char[] key = (settingSalt + "\0").ToCharArray();
            char[] salt = "$2a$10$fVH8e28OQRj9tqiDXs1e1u.\0".ToCharArray();

            return bcrypt(key, salt).Substring(salt.Length - 2);
        }
    }
}
