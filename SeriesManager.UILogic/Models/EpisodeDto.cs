﻿using System;
using SQLite;
using TheTVDBSharp.Models;

namespace SeriesManager.UILogic.Models
{
    /// <summary>
    ///     Entity describing an episode of a <see cref="Series" />show.
    /// </summary>
    [Table("Episodes")]
    public class EpisodeDto
    {
        /// <summary>
        ///     Unique identifier for an episode.
        /// </summary>
        [PrimaryKey]
        public uint Id { get; set; }

        public uint SeriesId { get; set; }

        /// <summary>
        ///     This episode's season id.
        /// </summary>
        public uint? SeasonId { get; set; }

        /// <summary>
        ///     This episode's season number.
        /// </summary>
        public uint? SeasonNumber { get; set; }

        /// <summary>
        ///     This episode's number in the appropriate season.
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        ///     Main language spoken in the episode.
        /// </summary>
        public uint? Language { get; set; }

        /// <summary>
        ///     This episode's title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     A short description of the episode.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Path of the episode thumbnail
        /// </summary>
        public string ThumbRemotePath { get; set; }

        /// <summary>
        ///     Director of the episode.
        /// </summary>
        public string Directors { get; set; }

        /// <summary>
        ///     Writers(s) of the episode.
        /// </summary>
        public string Writers { get; set; }

        /// <summary>
        ///     A list of guest stars.
        /// </summary>
        public string GuestStars { get; set; }

        /// <summary>
        ///     The date of the first time this episode has aired.
        /// </summary>
        public DateTime? FirstAired { get; set; }

        /// <summary>
        ///     Average rating as shown on IMDb.
        /// </summary>
        public double? Rating { get; set; }

        /// <summary>
        ///     Amount of votes cast.
        /// </summary>
        public int? RatingCount { get; set; }

        /// <summary>
        ///     Timestamp of the last update to this episode.
        /// </summary>
        public DateTime? LastUpdated { get; set; }

        /// <summary>
        ///     Width dimension of the thumbnail in pixels;
        /// </summary>
        public int? ThumbWidth { get; set; }

        /// <summary>
        ///     Height dimension of the thumbnail in pixels.
        /// </summary>
        public int? ThumbHeight { get; set; }
    }
}