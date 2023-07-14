using Common.WebApi.Database;
using Microsoft.EntityFrameworkCore;
using ScoreTracking.Models;
using System.ComponentModel.DataAnnotations;

namespace ScoreTracking.Validations
{
    public class ValidateNameAttribute : ValidationAttribute
    {
        private readonly string _entityName;

        public ValidateNameAttribute(string entityName)
        {
            this._entityName = entityName;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var dbContext = (DatabaseContext)validationContext.GetService(typeof(DatabaseContext));
            var firstName = (string)value;
            var entitySet = dbContext.GetType().GetProperty(_entityName)?.GetValue(dbContext);
            var entities = entitySet as IEnumerable<object>;

            //var httpContextAccessor = (IHttpContextAccessor)validationContext.GetService(typeof(IHttpContextAccessor));
            //var routeData = httpContextAccessor.HttpContext.Request.RouteValues;
            //routeData.TryGetValue("id", out var idValue);
            //    if (validationContext.ObjectInstance is User user)
            //{
               
            //    var currentUser = GetCurrentUser(int.TryParse(idValue?.ToString(), out var id), dbContext); // Replace with your logic to get the current user
            //    if (currentUser != null && user.Id == currentUser.Id)
            //    {
            //        return ValidationResult.Success;
            //    }
            //}

            if (entities.Any(u => (string)u.GetType().GetProperty("FirstName")?.GetValue(u) == firstName))
            {
                return new ValidationResult("First name already exists");
            }

            return ValidationResult.Success;
        }

        private async Task<User?> GetCurrentUser(int id, DatabaseContext dbContext)
        {
            return await dbContext.users
                  .AsNoTracking()
                  .Where(x => x.Id == id)
                  .FirstOrDefaultAsync().ConfigureAwait(true);
        }
    }
}
