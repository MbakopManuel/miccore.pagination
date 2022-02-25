using Miccore.Pagination.Model;
using Microsoft.AspNetCore.Http.Extensions;

namespace Miccore.Pagination.Service
{
    internal static class RouterLink{

        //
        // Summary:
        //     Generates Previous and next URL of the pagination object
        //
        // Parameters:
        //      paginationModel:
        //          object where next and previous url zill be added
        //      nameOfFunction:
        //          the name of the controller api
        //          example: nameof(GetAllItems)
        //
        // Returns:
        //     the pagination Model with next and previous urls if exist.
        
        public static PaginationModel<TModel> AddRouteLink<TModel>(
            this PaginationModel<TModel> paginationModel,
            string nameOfFunction,
            PaginationQuery query
        ) where TModel : class{
            // add previous route if exist
            if(paginationModel.CurrentPage > 1){
                paginationModel.Prev = $"{nameOfFunction}?paginate={query.paginate}&limit={query.limit}&page={query.page - 1}";
            }

            // add next route if exist
            if(paginationModel.CurrentPage < paginationModel.TotalPages){
                paginationModel.Next = $"{nameOfFunction}?paginate={query.paginate}&limit={query.limit}&page={query.page + 1}";
            }

            return paginationModel;
        }
    }
}