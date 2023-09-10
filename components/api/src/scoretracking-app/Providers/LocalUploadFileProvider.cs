using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using ScoreTracking.App.Interfaces.Providers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.Providers
{
    public class LocalUploadFileProvider : IUploadFileProvider
    {
        private readonly IHostEnvironment _hostEnvironment;

        public LocalUploadFileProvider(IHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        public async Task<string?> UploadFile(IFormFile file, string destination)
        {
            if (file != null && file.Length > 0)
            {
                var uploadsFolder = Path.Combine(_hostEnvironment.ContentRootPath, "Uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                return filePath;
            }
            return null;
        }
    }
}
