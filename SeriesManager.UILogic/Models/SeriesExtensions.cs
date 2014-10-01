using TheTVDBSharp.Models;

namespace SeriesManager.UILogic.Models
{
    public static class SeriesExtensions
    {
        public static void Populate(this Series series, Series updatedSeries)
        {
            series.Actors = updatedSeries.Actors;
            series.AirDay = updatedSeries.AirDay;
            series.AirTime = updatedSeries.AirTime;
            series.BannerRemotePath = updatedSeries.BannerRemotePath;
            series.Banners = updatedSeries.Banners;
            series.ContentRating = updatedSeries.ContentRating;
            series.Description = updatedSeries.Description;
            series.Episodes = updatedSeries.Episodes;
            series.FanartRemotePath = updatedSeries.FanartRemotePath;
            series.FirstAired = updatedSeries.FirstAired;
            series.Genres = updatedSeries.Genres;
            series.ImdbId = updatedSeries.ImdbId;
            series.Language = updatedSeries.Language;
            series.LastUpdated = updatedSeries.LastUpdated;
            series.Network = updatedSeries.Network;
            series.PosterRemotePath = updatedSeries.PosterRemotePath;
            series.Rating = updatedSeries.Rating;
            series.RatingCount = updatedSeries.RatingCount;
            series.Runtime = updatedSeries.Runtime;
            series.Status = updatedSeries.Status;
            series.Title = updatedSeries.Title;
            series.Zap2ItId = updatedSeries.Zap2ItId;
        }
    }
}
