using ScoreTracking.App.DTOs.Requests;
using ScoreTracking.App.Helpers;
using ScoreTracking.App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.Extensions.Query
{
    public static class UserQueryExtension
    {
        public static IQueryable<User> SearchByTerm(this IQueryable<User> query, string? searchTerm)
        {
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                return query.Where(u => u.FirstName.Contains(searchTerm));
            }
            return query;
        }
        private static Expression<Func<User, object>> GetSortingColumn(string? sortedColumn)
        {
            return sortedColumn?.ToLower() switch
            {
                "firstname" => (User => User.FirstName),
                "lastname" => (User => User.LastName),
                "email" => (User => User.Email),
                "phone" => (User => User.Phone),
                _ => (User => User.Id),
            };
        }
        public static IQueryable<User> ApplySorting(this IQueryable<User> query, string? sortedColumn, string? sortingOrder)
        {
            Expression<Func<User, object>> column = GetSortingColumn(sortedColumn);
            return sortingOrder?.ToLower() switch
            {
                "desc" => query.OrderByDescending(column),
                _ => query.OrderBy(column),
            };
        }
        public static async Task<PagedList<User>?> ApplyPagination(this IQueryable<User> query, int page, int pageSize)
        {
            var pagedData = await PagedList<User>.CreateAsync(query, page, pageSize);
            return pagedData;
        }
    }
}
