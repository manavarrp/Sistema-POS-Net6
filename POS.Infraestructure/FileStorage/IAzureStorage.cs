using Microsoft.AspNetCore.Http;

namespace POS.Infraestructure.FileStorage
{
    public interface IAzureStorage
    {
        Task<string> SaveFile(string container, IFormFile file);

        Task<string> EditFile(string container, IFormFile file, string route);

        Task DeleteFile(string container, string route);
    }
}
