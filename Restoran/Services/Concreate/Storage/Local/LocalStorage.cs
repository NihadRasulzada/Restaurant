using Softy_Pinko.Services.Abstraction.Storage.Local;

namespace Softy_Pinko.Services.Concreate.Storage.Local
{
    public class LocalStorage : Storage, ILocalStorage
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public LocalStorage(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task DeleteAsync(string path, string fileName)
            => File.Delete($"{path}\\{fileName}");

        public List<string> GetFiles(string path)
        {
            DirectoryInfo directory = new(path);
            return directory.GetFiles().Select(f => f.Name).ToList();
        }

        public bool HasFile(string path, string fileName)
            => File.Exists(Path.Combine(_webHostEnvironment.WebRootPath, path, fileName));
        async Task<bool> CopyFileAsync(string path, IFormFile file)
        {
            try
            {
                await using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: false);

                await file.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
                return true;
            }
            catch (Exception ex)
            {
                //todo log!
                throw ex;
            }
        }
        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string path, List<IFormFile> files)
        {
            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, path);
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);

            List<(string fileName, string path)> datas = new();
            foreach (IFormFile file in files)
            {
                string fileNewName = FileRename(file.FileName);

                await CopyFileAsync($"{uploadPath}\\{fileNewName}", file);
                datas.Add((fileNewName, $"{path}\\{fileNewName}"));
            }

            return datas;
        }

        public async Task<(string fileName, string pathOrContainerName)> UploadAsync(string path, IFormFile file)
        {
            bool b = false;
            string uploadPath;
            string fileNewName;

            try
            {
                uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, path);
                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);

                fileNewName = FileRename(file.FileName);

                await CopyFileAsync($"{uploadPath}\\{fileNewName}", file);
                return (fileNewName, $"{path}\\{fileNewName}");
            }
            catch
            {
                return (null, null);
            }
        }
    }
}
