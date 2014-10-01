using System;
using SQLite;

namespace SeriesManager.UILogic.Models
{
    /// <summary>
    ///     Entity describing a show.
    /// </summary>
    [Table("Series")]
    public class SeriesDto
    {
        /// <summary>
        ///     Unique identifier used by TVDB and TVDBSharp.
        /// </summary>
        [PrimaryKey]
        public uint Id { get; set;}

        /// <summary>
        ///     Main language of the show.
        /// </summary>
        public uint? Language { get; set; }

        /// <summary>
        ///     Name of the show.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     Link to the banner image.
        /// </summary>
        public string BannerRemotePath { get; set; }

        /// <summary>
        ///     A short overview of the show.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     The date the show aired for the first time.
        /// </summary>
        public DateTime? FirstAired { get; set; }

        /// <summary>
        ///     Network that broadcasts the show.
        /// </summary>
        public string Network { get; set; }

        /// <summary>
        ///     Unique identifier used by IMDb.
        /// </summary>
        public string ImdbId { get; set; }

        /// <summary>
        ///     List of all actors in the show.
        /// </summary>
        public string Actors { get; set; }

        /// <summary>
        ///     Day of the week when the show airs.
        /// </summary>
        public uint? AirDay { get; set; }

        /// <summary>
        ///     Time of the day when the show airs.
        /// </summary>
        public TimeSpan? AirTime { get; set; }

        /// <summary>
        ///     Rating of the content provided by an official organ.
        /// </summary>
        public uint? ContentRating { get; set; }

        /// <summary>
        ///     A list of genres the show is associated with.
        /// </summary>
        public string Genres { get; set; }

        /// <summary>
        ///     Average rating as shown on IMDb.
        /// </summary>
        public double? Rating { get; set; }

        /// <summary>
        ///     Amount of votes cast.
        /// </summary>
        public int? RatingCount { get; set; }

        /// <summary>
        ///     Let me know if you find out what this is.
        /// </summary>
        public int? Runtime { get; set; }

        /// <summary>
        ///     Current status of the show.
        /// </summary>
        public uint? Status { get; set; }

        /// <summary>
        ///     Link to a fanart image.
        /// </summary>
        public string FanartRemotePath { get; set; }

        /// <summary>
        ///     Timestamp of the latest update.
        /// </summary>
        public DateTime? LastUpdated { get; set; }

        /// <summary>
        ///     Let me know if you find out what this is.
        /// </summary>
        public string PosterRemotePath { get; set; }

        /// <summary>
        ///     No clue
        /// </summary>
        public string Zap2ItId { get; set; }
    }
}