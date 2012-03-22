namespace StrongPassword
{
    public abstract class AProfile
    {
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}