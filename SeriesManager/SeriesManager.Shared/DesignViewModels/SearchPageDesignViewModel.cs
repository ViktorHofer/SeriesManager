using SeriesManager.UILogic.ViewModels;
using System;
using System.Linq;
using TheTVDBSharp.Models;

namespace SeriesManager.DesignViewModels
{
    class SearchPageDesignViewModel : SearchPageViewModel
    {
        public SearchPageDesignViewModel()
            : base()
        {
            SearchQuery = "Scrubs";

            var seriesCollection = new Series[] 
                {
                    new Series(76156) 
                    {
                        Language = Language.English,
                        Title = "Scrubs",
                        BannerRemotePath = "graphical/76156-g9.jpg",
                        Description = "Scrubs focuses on the lives of several people working at Sacred Heart, a teaching hospital. It features fast-paced dialogue, slapstick, and surreal vignettes presented mostly as the daydreams of the central character, Dr. John Michael \"J.D.\" Dorian.",
                        FirstAired = new DateTime(2001, 2, 10),
                        Network = "ABC",
                        ImdbId = "tt0285403",
                        Zap2ItId = "EP00446160"
                    },
                    new Series(167151) 
                    {
                        Language = Language.English,
                        Title = "Wormwood Scrubs",
                        BannerRemotePath = "graphical/167151-g.jpg",
                        Description = "Wormwood Scrubs is a documentary series which takes a look at life for the inmates and staff at one of Europe's biggest prisons. ",
                        FirstAired = new DateTime(2010, 10, 5),
                        Network = "ITV1"
                    }
                };

            SearchResult = seriesCollection
                .Select(series => new SearchItemDesignViewModel(series))
                .ToArray();
        }
    }
}
