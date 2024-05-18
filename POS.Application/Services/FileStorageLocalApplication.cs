using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using POS.Application.Interfaces;
using POS.Infraestructure.FileStorage;

namespace POS.Application.Services
{
    public class FileStorageLocalApplication : IFileStorageLocalApplication
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFileStorageLocal _fileStorageLocal;

        public FileStorageLocalApplication(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor contextAccessor, IFileStorageLocal fileStorageLocal)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = contextAccessor;
            _fileStorageLocal = fileStorageLocal;
        }

        public async Task<string> SaveFile(string container, IFormFile file)
        {
           //obtener ruta raiz del sitio web
           var webRootPath = _webHostEnvironment.WebRootPath;
            //obtener esquema Http
            var scheme = _httpContextAccessor.HttpContext!.Request.Scheme;
            //obtener dominio de la request http
            var host = _httpContextAccessor.HttpContext.Request.Host;

            return await _fileStorageLocal.SaveFile(container, file, webRootPath, scheme, host.Value);
        }
        public async Task<string> EditFile(string container, IFormFile file, string route)
        {
            var webRootPath = _webHostEnvironment.WebRootPath;
            var scheme = _httpContextAccessor.HttpContext!.Request.Scheme;
            var host = _httpContextAccessor.HttpContext.Request.Host;

            return await _fileStorageLocal.EditFile(container, file, route, webRootPath, scheme, host.Value);
        }

        public async Task RemoveFile(string route, string container)
        {
            var webRootPath = _webHostEnvironment.WebRootPath;

            await _fileStorageLocal.RemoveFile(route, container, webRootPath);
        }

        
    }
}
