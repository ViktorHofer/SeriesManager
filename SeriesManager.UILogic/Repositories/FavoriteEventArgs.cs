using System;
using System.Collections.Generic;
using TheTVDBSharp.Models;

namespace SeriesManager.UILogic.Repositories
{
    public class FavoriteEventArgs : EventArgs
    {
        public IEnumerable<Series> NewSeriesCollection { get; private set; }

        public IEnumerable<Series> RemovedSeriesCollection { get; private set; }

        public FavoriteEventArgs(IEnumerable<Series> newSeriesCollection, 
            IEnumerable<Series> removedSeriesCollection)
        {
            NewSeriesCollection = newSeriesCollection;
            RemovedSeriesCollection = removedSeriesCollection;
        }
    }
}
