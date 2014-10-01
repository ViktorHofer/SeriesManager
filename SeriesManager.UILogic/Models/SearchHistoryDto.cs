using SQLite;

namespace SeriesManager.UILogic.Models
{
    [Table("SearchHistory")]
    public class SearchHistoryDto
    {
        public string SearchQuery { get; set; }
    }
}
