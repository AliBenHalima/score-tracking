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
    public class RessourceNotFoundException : ScoreTrackingException
    {
        public RessourceNotFoundException(string message, params object[] args)
      : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
            this.statusCode = (int)HttpStatusCode.NotFound;
        }
    }
}
