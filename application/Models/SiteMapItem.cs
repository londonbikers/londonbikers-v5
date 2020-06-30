using System;
using Apollo.Models.Interfaces;

namespace Apollo.Models
{
    public class SiteMapItem : ISiteMapItem
    {
        #region accessors
        public long ItemId { get; set; }
        public string Title { get; set; }
        public DateTime LastModified { get; set; }
        public string Keywords { get; set; }
        public SiteMapContentType ContentType { get; set; }
        #endregion

        #region constructors
        internal SiteMapItem()
        {
        }
        #endregion
    }
}