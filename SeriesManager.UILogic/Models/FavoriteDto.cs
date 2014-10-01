using SQLite;

namespace SeriesManager.UILogic.Models
{
    [Table("Favorites")]
    public class FavoriteDto
    {
        [PrimaryKey]
        public uint SeriesId { get; set; }
    }
}
