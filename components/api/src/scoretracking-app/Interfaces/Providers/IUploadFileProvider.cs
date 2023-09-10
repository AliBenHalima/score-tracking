using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace ScoreTracking.App.Interfaces.Providers
{
    public interface IUploadFileProvider
    {
        Task<string?> UploadFile(IFormFile file, string destination);
    }
}
