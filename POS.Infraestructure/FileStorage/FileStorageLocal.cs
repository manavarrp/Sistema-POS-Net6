using Microsoft.AspNetCore.Http;

namespace POS.Infraestructure.FileStorage
{
    public class FileStorageLocal : IFileStorageLocal
    {
        public async Task<string> SaveFile(string container, IFormFile file, string webRootPath, string scheme, string host)
        {
           //obtener la extension del archivo
           var extension = Path.GetExtension(file.FileName);
            //asginar un nombre único
            var fileName = $"{Guid.NewGuid()}{extension}";
            //combinar la ruta de nuestra API con la carpeta de destino para armar la ruta de destino
            string folder = Path.Combine(webRootPath, container);
            //creamos la carpeta
            if(!Directory.Exists(folder)) 
                Directory.CreateDirectory(folder);
            //para obtener la ruta se combina carpeta + nombre del archivo
            var path = Path.Combine(folder, fileName);

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                var content = memoryStream.ToArray();
                await File.WriteAllBytesAsync(path, content);
            }

            var currentUrl = $"{scheme}//:{host}";
            var pathDb = Path.Combine(currentUrl, container, fileName).Replace("\\", "/");

            return pathDb;
            
        }
        public async Task<string> EditFile(string container, IFormFile file, string route, string webRootPath, string scheme, string host)
        {
            await RemoveFile(route, container, webRootPath );

            return await SaveFile(container, file, webRootPath, scheme, host);
        }

        public Task RemoveFile(string route, string container, string webRootPath)
        {
            if (string.IsNullOrEmpty(route))
                return Task.CompletedTask;

            var fileName = Path.GetFileName(route);

            var directoryFile = Path.Combine(webRootPath, container, fileName);

            if (File.Exists(directoryFile))
                File.Delete(directoryFile);

            return Task.CompletedTask;
        }
    }
}
