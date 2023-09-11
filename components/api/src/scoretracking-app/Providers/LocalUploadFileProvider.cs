using BenchmarkDotNet.Loggers;
using Flurl;
using ImageMagick;
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
                    string? originalExtension = Path.GetExtension(file.FileName);
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    using (var stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        if (!originalExtension.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase))
                        {
                            fileName = ConvertToJPG(stream, uploadsFolder);
                        }
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
        private string ConvertToJPG(MemoryStream stream, string uploadedFolderPath)
        {
            stream.Seek(0, SeekOrigin.Begin); // Rewind the stream to the beginning
            string fileName;
            using (var image = new MagickImage(stream))
            {
                // Set the format to JPEG
                image.Format = MagickFormat.Jpg;
                image.Quality = 90;

                // Specify the path where you want to save the converted image
                fileName = Guid.NewGuid().ToString() + "." + image.Format;

                var absoluteFilePath = Path.Combine(uploadedFolderPath, fileName);
                // Save the converted image to the specified path

                image.Write(absoluteFilePath);
            }
            return fileName;
        }
    }
}
