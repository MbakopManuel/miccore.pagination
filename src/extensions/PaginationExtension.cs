using Miccore.Pagination.Model;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class PaginationExtension{
         //
        // Summary:
        //    Pagination extension
        //
        // Parameters:
        //      query:
        //          context element to query
        //      paginationQuery:
        //          pagination query
        //
        // Returns:
        //     the pagination Model 
        
        public static async Task<PaginationModel<TModel>> PaginateAsync<TModel>(
            this IQueryable<TModel> query,
            PaginationQuery paginationQuery
        ) where TModel : class{
            

            // initialize pagination entity
            var paged = new PaginationModel<TModel>();

            // update page
            paginationQuery.page = (paginationQuery.page < 0) ? 1 : paginationQuery.page;

            //initialize current page
            paged.CurrentPage = paginationQuery.page;
            // initialize page size, number of element
            paged.PageSize = paginationQuery.limit;
            // update total items of elements
            paged.TotalItems = await query.CountAsync();
            // skip items and take another ones
            var startRow = (paginationQuery.page - 1) * paginationQuery.limit;
            
            // check if pagination is true or false
            if( paginationQuery.paginate ){
                paged.Items = await query.Skip(startRow).Take(paginationQuery.limit).ToListAsync();
            }else{
                 paged.Items = await query.ToListAsync();
            }

            // update total number of pages
            paged.TotalPages = (int)Math.Ceiling(paged.TotalItems / (double)paginationQuery.limit);
            // return the paginated
            return paged;

        }

    }
}