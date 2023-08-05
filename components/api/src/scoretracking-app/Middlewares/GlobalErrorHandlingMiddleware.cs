using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ScoreTracking.App.Helpers.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ScoreTracking.App.Middlewares
{
    public class GlobalErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public GlobalErrorHandlingMiddleware(RequestDelegate next, ILogger<GlobalErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception appError)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                 switch (appError)
                {
                    // Custom Application exception
                    case ScoreTrackingException exception:
                        response.StatusCode = (int)exception.StatusCode;
                        _logger.LogError(appError, appError.Message);
                        break;
                    default:
                        _logger.LogError(appError, appError.Message);
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(new { errorMessage = appError?.Message });
                await response.WriteAsync(result);
            }
        }

    }
}
