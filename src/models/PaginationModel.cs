using System.Collections.Generic;

namespace Miccore.Net.Pagination.Model
{
    public class PaginationModel<TModel>{
        const int MaxPageSize = 100;
        private int _pageSize;
        public int PageSize {
            get => _pageSize;
            set => _pageSize = ( value > MaxPageSize) ? MaxPageSize : value;
        }
        public int CurrentPage {get; set;}
        public int TotalItems {get; set;}
        public int TotalPages { get; set;}
        public List<TModel> Items {get; set;}
        public string Prev {get; set;}
        public string Next {get; set;}
        public PaginationModel(){
            Items = new List<TModel>();
        }
    }
}