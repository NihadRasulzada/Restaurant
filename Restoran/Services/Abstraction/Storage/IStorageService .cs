namespace Softy_Pinko.Services.Abstraction.Storage
{
    public interface IStorageService : IStorage
    {
        public string StorageName { get; }
    }
}
