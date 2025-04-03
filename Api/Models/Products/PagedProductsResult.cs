namespace Api.Models.Products
{
    public class PagedProductsResult
    {
        public List<Product> Products { get; set; } = [];
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
