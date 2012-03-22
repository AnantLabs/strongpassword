using System;

namespace StrongPassword
{
    public class Profile : AProfile
    {
        public int Size { get; set; }


        public override bool Equals(object obj)
        {
            return (obj is AProfile && ((AProfile)obj).Name == Name);
        }

        public override int GetHashCode()
        {
            return Size.GetHashCode() ^ Name.GetHashCode();
        }

        public override string ToString()
        {
            return String.Format("{0,-10} {1,10} ", base.ToString(), "Length " +Size+" chars");
        }
    }
}