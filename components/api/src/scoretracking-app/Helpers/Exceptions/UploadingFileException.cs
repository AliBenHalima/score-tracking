using System.Globalization;
using System.Net;

namespace ScoreTracking.App.Helpers.Exceptions
{
    public class UploadingFileException : ScoreTrackingException
    {
        public UploadingFileException(string message, params object[] args)
      : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
            StatusCode = (int)HttpStatusCode.BadRequest;
        }
    }
}
