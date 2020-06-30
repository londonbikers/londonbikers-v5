using System;
using System.Linq;
using System.Threading;
using System.Configuration;
using Apollo.Models;

namespace Apollo.BackgroundTasks
{
    /// <summary>
    /// Handles the updating of cached tag objects and their latest-commented-documents by periodically going through all Tag objects cached and expiring their internal caches.
    /// </summary>
    public class PopularContentProcessor
    {
        #region members
        private delegate void ProcessorDelegate();
        #endregion

        #region constructors
        public PopularContentProcessor()
        {
            var myAction = new ProcessorDelegate(StartWork);
            myAction.BeginInvoke(null, null);
        }
        #endregion

        #region private methods
        private static void StartWork()
        {
            while (true)
            {
                // get all tags in the cache and zap their popular object caches so they're rebuilt the next time they're requested.
                foreach (var tag in from item in Server.Instance.CacheServer.Items
                                    where item.Key.StartsWith(typeof(Tag).FullName)
                                    select item.Value.Item as Tag)
                {
                    tag.PopularDocuments = null;
                    tag.LatestCommentedDocuments = null;
                }

                // this popular content list needs zapping so it's rebuilt on the next request.
                Server.Instance.ContentServer.PopularDocuments = null;

                Thread.Sleep(TimeSpan.FromMinutes(int.Parse(ConfigurationManager.AppSettings["Apollo.PopularContentProcessIntervalMins"])));
            }
        }
        #endregion
    }
}