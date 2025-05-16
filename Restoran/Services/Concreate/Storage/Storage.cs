namespace Softy_Pinko.Services.Concreate.Storage
{
    public class Storage
    {
        protected  string FileRename(string fileName)
        {
            return Guid.NewGuid().ToString() + fileName;
        }
    }
}
