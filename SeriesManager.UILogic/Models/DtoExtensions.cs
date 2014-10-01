using System;
using System.Collections.Generic;
using System.Linq;
using TheTVDBSharp.Models;

namespace SeriesManager.UILogic.Models
{
    public static class DtoExtensions
    {
        #region Actor

        public static ActorDto FromDomain(this Actor model, uint seriesId)
        {
            return new ActorDto()
            {
                Id = model.Id,
                SeriesId = seriesId,
                ImageRemotePath = model.ImageRemotePath,
                Name = model.Name,
                Role = model.Role,
                SortOrder = model.SortOrder
            };
        }

        public static Actor ToDomain(this ActorDto dto)
        {
            return new Actor(dto.Id)
            {
                ImageRemotePath = dto.ImageRemotePath,
                Name = dto.Name,
                Role = dto.Role,
                SortOrder = dto.SortOrder
            };
        }

        #endregion

        #region Episode

        public static EpisodeDto FromDomain(this Episode model, uint seriesId) 
        {
            return new EpisodeDto()
            {
                SeriesId = seriesId,
                Description = model.Description,
                Directors = model.Directors != null ? model.Directors.Aggregate((a,b) => string.Format("{0}|{1}", a, b)) : null,
                FirstAired = model.FirstAired,
                GuestStars = model.GuestStars != null ? model.GuestStars.Aggregate((a,b) => string.Format("{0}|{1}", a, b)) : null,
                Id = model.Id,
                Language = (uint?)model.Language,
                LastUpdated = model.LastUpdated,
                Number = model.Number,
                Rating = model.Rating,
                RatingCount = model.RatingCount,
                SeasonId = model.SeasonId,
                SeasonNumber = model.SeasonNumber,
                ThumbHeight = model.ThumbHeight,
                ThumbWidth = model.ThumbWidth,
                ThumbRemotePath = model.ThumbRemotePath,
                Title = model.Title,
                Writers = model.Writers != null ? model.Writers.Aggregate((a,b) => string.Format("{0}|{1}", a, b)) : null
            };
        }

        public static Episode ToDomain(this EpisodeDto dto)
        {
            return new Episode(dto.Id)
            {
                Description = dto.Description,
                Directors = dto.Directors != null ? dto.Directors.Split('|') : null,
                FirstAired = dto.FirstAired,
                GuestStars = dto.GuestStars != null ? dto.GuestStars.Split('|') : null,
                Language = (Language?)dto.Language,
                LastUpdated = dto.LastUpdated,
                Number = dto.Number,
                Rating = dto.Rating,
                RatingCount = dto.RatingCount,
                SeasonId = dto.SeasonId,
                SeasonNumber = dto.SeasonNumber,
                ThumbHeight = dto.ThumbHeight,
                ThumbRemotePath = dto.ThumbRemotePath,
                ThumbWidth = dto.ThumbWidth,
                Title = dto.Title,
                Writers = dto.Writers != null ? dto.Writers.Split('|') : null
            };
        }

        #endregion

        #region Series

        public static Tuple<SeriesDto, IReadOnlyCollection<EpisodeDto>, IReadOnlyCollection<ActorDto>, IReadOnlyCollection<BannerDto>> FromDomain(this Series model)
        {
            var seriesDto = new SeriesDto()
            {
                AirDay = (uint?)model.AirDay,
                AirTime = model.AirTime,
                BannerRemotePath = model.BannerRemotePath,
                ContentRating = (uint?)model.ContentRating,
                Description = model.Description,
                FanartRemotePath = model.FanartRemotePath,
                FirstAired = model.FirstAired,
                Genres = model.Genres != null ? model.Genres.Aggregate((a,b) => string.Format("{0}|{1}", a, b)) : null,
                Id = model.Id,
                ImdbId = model.ImdbId,
                Language = (uint?)model.Language,
                LastUpdated = model.LastUpdated,
                Network = model.Network,
                PosterRemotePath = model.PosterRemotePath,
                Rating = model.Rating,
                RatingCount = model.RatingCount,
                Runtime = model.Runtime,
                Status = (uint?)model.Status,
                Title = model.Title,
                Zap2ItId = model.Zap2ItId
            };

            return new Tuple<SeriesDto,IReadOnlyCollection<EpisodeDto>,IReadOnlyCollection<ActorDto>,IReadOnlyCollection<BannerDto>>(seriesDto,
                model.Episodes != null ? model.Episodes.Select(epi => epi.FromDomain(model.Id)).ToArray() : null,
                model.Actors != null ? model.Actors.Select(actor => actor.FromDomain(model.Id)).ToArray() : null,
                model.Banners != null ? model.Banners.Select(banner => banner.FromDomain(model.Id)).ToArray() : null);
        }

        public static Series ToDomain(this SeriesDto dto, 
            IReadOnlyCollection<EpisodeDto> episodeDtos, 
            IReadOnlyCollection<ActorDto> actorDtos,
            IReadOnlyCollection<BannerDto> bannerDtos)
        {
            return new Series(dto.Id)
            {
                Actors = actorDtos.Select(actorDto => actorDto.ToDomain()).ToArray(),
                AirDay = (Frequency?)dto.AirDay,
                AirTime = dto.AirTime,
                BannerRemotePath = dto.BannerRemotePath,
                Banners = bannerDtos.Select(bannerDto => bannerDto.ToDomain()).ToArray(),
                ContentRating = (ContentRating?)dto.ContentRating,
                Description = dto.Description,
                Episodes = episodeDtos.Select(episodeDto => episodeDto.ToDomain()).ToArray(),
                FanartRemotePath = dto.FanartRemotePath,
                FirstAired = dto.FirstAired,
                Genres = dto.Genres != null ? dto.Genres.Split('|') : null,
                ImdbId = dto.ImdbId,
                Language = (Language?)dto.Language,
                LastUpdated = dto.LastUpdated,
                Network = dto.Network,
                PosterRemotePath = dto.PosterRemotePath,
                Rating = dto.Rating,
                RatingCount = dto.RatingCount,
                Runtime = dto.Runtime,
                Status = (Status?)dto.Status,
                Title = dto.Title,
                Zap2ItId = dto.Zap2ItId
            };
        }

        #endregion

        #region Banners

        public static BannerDto FromDomain(this Banner model, uint seriesId) 
        {
            return new BannerDto()
            {
                Id = model.Id,
                Language = (uint?)model.Language,
                Rating = model.Rating,
                RatingCount = model.RatingCount,
                RemotePath = model.RemotePath,
                SeriesId = seriesId
            };
        }

        public static Banner ToDomain(this BannerDto dto)
        {
            return new Banner(dto.Id)
            {
                Language = (Language?)dto.Language,
                Rating = dto.Rating,
                RatingCount = dto.RatingCount,
                RemotePath = dto.RemotePath
            };
        }

        #endregion
    }
}
