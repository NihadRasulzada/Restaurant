namespace Softy_Pinko.Services.Abstraction.Storage
{
    public interface IStorage
    {
        Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string pathOrContainerName, List<IFormFile> files);
        Task<(string fileName, string pathOrContainerName)> UploadAsync(string path, IFormFile file);
        Task DeleteAsync(string pathOrContainerName, string fileName);
        List<string> GetFiles(string pathOrContainerName);
        bool HasFile(string pathOrContainerName, string fileName);
    }
}
