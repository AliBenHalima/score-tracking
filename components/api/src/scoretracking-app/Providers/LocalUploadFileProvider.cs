using BenchmarkDotNet.Loggers;
using Flurl;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ScoreTracking.App.Helpers.Exceptions;
using ScoreTracking.App.Interfaces.Providers;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ScoreTracking.App.Providers
{
    public class LocalUploadFileProvider : IUploadFileProvider
    {
        private readonly IHostEnvironment _hostEnvironment;
        private readonly ILogger<LocalUploadFileProvider> _logger;

        public LocalUploadFileProvider(IHostEnvironment hostEnvironment, ILogger<LocalUploadFileProvider> logger)
        {
            _hostEnvironment = hostEnvironment;
            _logger = logger;
        }

        public async Task<string?> UploadFile(IFormFile file, string filePath)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_hostEnvironment.ContentRootPath, filePath);
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    var absoluteFilePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(absoluteFilePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    return fileName;
                }
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new UploadingFileException("Error uploading Image");
            }
        }
    }
}
