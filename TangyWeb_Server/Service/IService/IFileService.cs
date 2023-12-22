using Microsoft.AspNetCore.Components.Forms;

namespace TangyWeb_Server.Service.IService
{
    public interface IFileService
    {
        Task<string> FileUpload(IBrowserFile browserFile);
        bool DeleteFile(string filePath);
    }
}
