
namespace api.Helpers
{
    public sealed class QueryObject
    {
        public string? Symbol { get; set; } = null;
        public string? CompanyName { get; set; } = null;
        public string? SortBy { get; set; } = null;

        public bool isDescending { get; set; } = false;
        public int PaneNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;


    }
}