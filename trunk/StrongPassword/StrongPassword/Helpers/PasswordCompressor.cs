using System.Linq;
using System.Collections.Generic;

namespace StrongPassword
{
    public static class PasswordCompressor
    {
        public static string GeneratePassword(string indata, int size)
        {
            List<char> charpos = indata.OrderBy(x => x.GetHashCode()).ToList();
            char[] outdata = indata.Take(size).ToArray();

            for(int i = 0; i < 31-size ; i++)
            {
                char tmp = (outdata[i % (outdata.Length -1)] & 1) == 0 ? charpos.First() : charpos.Last();

                outdata[i % (outdata.Length - 1)] = tmp;
                charpos.Remove(tmp);
            }

            return string.Concat(outdata);
        }
    }
}   