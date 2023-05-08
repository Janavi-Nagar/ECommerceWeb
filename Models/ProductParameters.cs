namespace ECommerceWeb.Models
{
    public class ProductParameters
    {
        public List<Products> products { get; set; }
        const int maxPageSize = 50;
        public int PageNumber { get; set; }
        public int PageCount { get; set; }
        private int _pageSize = 6;
        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}
