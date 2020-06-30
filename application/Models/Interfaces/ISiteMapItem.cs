using System;

namespace Apollo.Models.Interfaces
{
    public interface ISiteMapItem
    {
        long ItemId { get; set; }
        string Title { get; set; }
        DateTime LastModified { get; set; }
        string Keywords { get; set; }
        SiteMapContentType ContentType { get; set; }
    }
}