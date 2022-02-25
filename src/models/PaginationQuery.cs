using System.ComponentModel;

namespace Miccore.Pagination.Model
{
    internal class PaginationQuery{
        [DefaultValue(false)]
        public bool paginate { get; set;}
        [DefaultValue(1)]
        public int page { get; set;}
        [DefaultValue(10)]
        public int limit { get; set;}
    }
}