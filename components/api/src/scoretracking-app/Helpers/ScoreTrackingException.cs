using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.Helpers
{
    public class ScoreTrackingException : Exception
    {
        public int statusCode;

        // Constructor used when status code is implicitly used

        public ScoreTrackingException(string message, params object[] args) :
        base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
        // Constructor used when status code is explicitly used
        public ScoreTrackingException(string message, int statusCode, params object[] args)
      : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
            this.statusCode = statusCode;
        }
    }
}
