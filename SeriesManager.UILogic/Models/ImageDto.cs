using SQLite;

namespace SeriesManager.UILogic.Models
{
    [Table("Images")]
    public class ImageDto
    {
        [Unique]
        public string RemotePath { get; set; }

        [NotNull]
        public string Image { get; set; }
    }
}
