namespace Api.Models.Settings
{
    public class AppSettings
    {
        public bool Caching { get; set; }
        public int CacheDuration { get; set; } = 5; // Default cache duration in minutes
        public int ProductsPerPage { get; set; } = 6;
    }
}
