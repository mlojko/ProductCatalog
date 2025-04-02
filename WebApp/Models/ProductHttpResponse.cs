using System.Net;

namespace WebApp.Models
{
    public class ProductHttpResponse
    {
        public List<ProductResponse>? Products { get; internal set; }
        public HttpStatusCode StatusCode { get; internal set; }
        public bool IsSuccessStatusCode { get; internal set; }
    }
}
