using System.Net;

namespace WebApp.Models
{
    public class ProductHttpResponse
    {
        public List<ProductResponse>? Products { get; set; }
        public HttpStatusCode ResponseStatusCode { get; set; }
    }
}
