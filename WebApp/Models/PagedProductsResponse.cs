using System.Net;

namespace WebApp.Models
{
    public class PagedProductsResponse
    {
        public List<ProductResponse> Products { get; set; } = [];
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public HttpStatusCode StatusCode { get; internal set; }
        public bool IsSuccessStatusCode { get; internal set; }
    }
}
