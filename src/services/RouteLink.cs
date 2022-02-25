using Miccore.Pagination.Model;

namespace Miccore.Pagination.Service
{
    public static class RouterLink{

        //
        // Summary:
        //     Generates Previous and next URL of the pagination object
        //
        // Parameters:
        //      paginationModel:
        //          object where next and previous url zill be added
        //      controllerUrl:
        //          the name of the controller api
        //          example: Url.RouteUrl(nameof(GetAllItems))
        //      query:
        //          the query element
        //
        // Returns:
        //     the pagination Model with next and previous urls if exist.
        
        public static PaginationModel<TModel> AddRouteLink<TModel>(
            this PaginationModel<TModel> paginationModel,
            string controllerUrl,
            PaginationQuery query
        ) where TModel : class{
            if(query.paginate){
                // add previous route if exist
                if(paginationModel.CurrentPage > 1){
                    paginationModel.Prev = $"{controllerUrl}?paginate={query.paginate}&limit={query.limit}&page={query.page - 1}";
                }

                // add next route if exist
                if(paginationModel.CurrentPage < paginationModel.TotalPages){
                    paginationModel.Next = $"{controllerUrl}?paginate={query.paginate}&limit={query.limit}&page={query.page + 1}";
                }
            }
            
            return paginationModel;
        }
    }
}