namespace Core.Specifications
{
    public class ProductSpecParams
    {
        // Limit max size of page user can set up using API
        private const int MaxPageSize = 50;
        // Return first page by the default
        public int PageIndex { get; set; } = 1;
        // Default page size
        private int _pageSize = 6;
        // Page size is set by user chalk up MaxPage Size
        public int PageSize { 
            get => _pageSize; 
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value; 
        }

        // Filters
        public int? BrandId { get; set; }
        public int? TypeId {get; set;}

        // Field sort by
        public string Sort { get; set; }

        // Search
        private string _search;
        public string Search { 
            get => _search; 
            // find items no matter which case in used in input
            set => _search = value.ToLower(); 
        }
    }
}