using FluentValidation;
using Microsoft.AspNetCore.Http;
using ScoreTracking.App.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.Validations.CustomValidators
{
    public static class FileValidator
    {
        public static IRuleBuilder<T, IFormFile> IsFileValid<T>( this IRuleBuilder<T, IFormFile> ruleBuilder)
        {
            return ruleBuilder.NotEmpty().WithMessage("File Should Not Be Empty")
                              .Must(file => file.Length < 2 * 1024 * 1024).WithMessage("File Size Should Not Overpass 3MB")
                              .Must(HasValidExtension).WithMessage("File Extension is Not Supported");
        }
        private static bool HasValidExtension(IFormFile file)
        {
            string? fileExtension = Path.GetExtension(file.FileName);
            return Constants.ImageExtensions.ValidExtensions.Contains(fileExtension);
        }
    }
}
