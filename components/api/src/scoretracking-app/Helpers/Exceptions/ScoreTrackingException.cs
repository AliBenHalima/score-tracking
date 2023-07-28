using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.Helpers.Exceptions
{
    public class ScoreTrackingException : Exception
    {
        public int StatusCode;

        // Constructor used when status code is implicitly used

        public ScoreTrackingException(string message, params object[] args) :
        base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
            StatusCode = (int)HttpStatusCode.BadRequest;
        }
        // Constructor used when status code is explicitly used
        public ScoreTrackingException(string message, int statusCode, params object[] args)
      : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
            StatusCode = statusCode;
        }
    }
}
