using SQLite;

namespace SeriesManager.UILogic.Models
{
    [Table("Banners")]
    public class BannerDto
    {
        [PrimaryKey]
        public uint Id { get; set; }

        public uint SeriesId { get; set; }

        public string RemotePath { get; set; }

        public uint? Language { get; set; }

        public double? Rating { get; set; }

        public int? RatingCount { get; set; }
    }
}
