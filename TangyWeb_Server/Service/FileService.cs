using Microsoft.AspNetCore.Components.Forms;
using TangyWeb_Server.Service.IService;

namespace TangyWeb_Server.Service
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            this.webHostEnvironment = webHostEnvironment;
        }

        public bool DeleteFile(string filePath)
        {
            if (File.Exists(webHostEnvironment.WebRootPath + filePath))
            {
                File.Delete(webHostEnvironment.WebRootPath + filePath);
                return true;
            }
            return false;
        }

        public async Task<string> FileUpload(IBrowserFile browserFile)
        {
            FileInfo fileInfo = new(browserFile.Name);
            string fileName = Guid.NewGuid().ToString() + fileInfo.Extension;
            string folderDirectory = $"{webHostEnvironment.WebRootPath}\\images\\product";

            if (!Directory.Exists(folderDirectory))
            {
                Directory.CreateDirectory(folderDirectory);
            }

            string filePath = Path.Combine(folderDirectory, fileName);

            await using FileStream fileStream = new(filePath, FileMode.Create);
            await browserFile.OpenReadStream().CopyToAsync(fileStream);

            string fullPath = $"/images/product/{fileName}";

            return fullPath;
        }
    }
}
