namespace StrongPassword
{
    public interface IFile
    {
        void Save(IFile ifile);
        IFile Load();
    }
}