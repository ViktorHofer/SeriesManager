using SQLite;

namespace SeriesManager.UILogic.Models
{
    [Table("Actors")]
    public class ActorDto
    {
        [PrimaryKey]
        public uint Id { get; set;}

        public uint SeriesId { get; set; }

        public string ImageRemotePath { get; set; }

        public string Name { get; set; }

        public string Role { get; set; }

        public int SortOrder { get; set; }
    }
}
