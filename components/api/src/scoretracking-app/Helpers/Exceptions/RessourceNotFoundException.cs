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
    public class RessourceNotFoundException : ScoreTrackingException
    {
        public RessourceNotFoundException(string message, params object[] args)
      : base(string.Format(CultureInfo.CurrentCulture, message, args))
        {
            StatusCode = (int)HttpStatusCode.NotFound;
        }
    }
}
