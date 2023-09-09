using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ScoreTracking.App.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoreTracking.App.Helpers
{
    public class PagedList<T> where T:class
    {
        public PagedList(List<T> items, int totalCount, int page, int pageSize, Uri nextPageUrl, Uri previousPageUrl)
        {
            Items = items;
            TotalCount = totalCount;
            Page = page;
            PageSize = pageSize;
            NextPageUrl = nextPageUrl;
            PreviousPageUrl = previousPageUrl;
        }

        public List<T> Items { get; }
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public bool HasNextPage => Page * PageSize < TotalCount;
        public bool HasPreviousPage => Page > 1;
        public Uri? NextPageUrl { get; set; }
        public Uri? PreviousPageUrl { get; set; }

        //public static async Task<PagedList<T>> CreateAsync(IQueryable<T> query, IUriService uriService, int page, int pageSize, string route)
        //{
        //    // Get all rows count.
        //    var totalCount = await query.CountAsync();
        //    var nextPageUrl = PagedList<T>.HasNextPageCheck(page, pageSize, totalCount) ? uriService.GetPageUri(page + 1, pageSize, route) : default;
        //    var previousPageUrl = PagedList<T>.HasPreviousPageCheck(page) ? uriService.GetPageUri(page -1, pageSize, route) : default;
        //    //Apply Pagination
        //    var items = await query.Skip((page -1) * pageSize).Take(pageSize).ToListAsync();
        //    return new(items, totalCount, page, pageSize, nextPageUrl, previousPageUrl);
        //}
        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> query, int page, int pageSize)
        {
            // Get all rows count.
            var totalCount = await query.CountAsync();
            //Apply Pagination
            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return new(items, totalCount, page, pageSize, null, null);
        }

        public static PagedList<T>? AddLinks(PagedList<T>? pagedData, IUriService _uriService, string route)
        {
            var previousPageUrl = PagedList<T>.HasPreviousPageCheck(pagedData.Page) ? _uriService.GetPageUri(pagedData.Page - 1, pagedData.PageSize, route) : default;
            var nextPageUrl = PagedList<T>.HasNextPageCheck(pagedData.Page, pagedData.PageSize, pagedData.TotalCount) ? _uriService.GetPageUri(pagedData.Page + 1, pagedData.PageSize, route) : default;
            pagedData.PreviousPageUrl = previousPageUrl;
            pagedData.NextPageUrl = nextPageUrl;
            return pagedData;
        }

        private static bool HasNextPageCheck(int page, int pageSize, int totalCount) {
            return page * pageSize < totalCount;
        }

        private static bool HasPreviousPageCheck(int page)
        {
            return page > 1;
        }

    }
}
