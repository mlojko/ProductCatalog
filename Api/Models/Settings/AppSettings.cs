namespace Api.Models.Settings
{
    public class AppSettings
    {
        public bool Caching { get; set; }
        public int ProductsPerPage { get; set; } = 6;
    }
}
