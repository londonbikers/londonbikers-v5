using System;
using System.Web;
using Apollo;
using Apollo.Models;
using Apollo.Utilities;
using Apollo.Utilities.Web;

namespace Tetron.Code
{
    public class UrlConverter
    {
        public void Convert(string entityType, string identifier)
        {
            var type = DomainObjectType.Document;
            var objectType = entityType;
            if (string.IsNullOrEmpty(objectType))
                HttpContext.Current.Response.Redirect("~/");

            switch (objectType)
            {
                case "document":
                    type = DomainObjectType.Document;
                    break;
                case "documentimage":
                    type = DomainObjectType.DocumentImage;
                    break;
                case "gallery":
                    type = DomainObjectType.Gallery;
                    break;
                case "galleryslideshow":
                    type = DomainObjectType.GallerySlideshow;
                    break;
                case "gallerycategory":
                    type = DomainObjectType.GalleryCategory;
                    break;
                case "galleryimage":
                    type = DomainObjectType.GalleryImage;
                    break;
                case "galleryimagehandler":
                    type = DomainObjectType.GalleryImageHandler;
                    break;
                case "directorycategory":
                    type = DomainObjectType.DirectoryCategory;
                    break;
                case "directoryitem":
                    type = DomainObjectType.DirectoryItem;
                    break;
            }

            if (Helpers.IsNumeric(identifier))
            {
                ConvertNumericUrl(HttpContext.Current, type, System.Convert.ToInt64(identifier));
            }
            else if (!string.IsNullOrEmpty(identifier))
            {
                if (!Helpers.IsGuid(identifier)) HttpContext.Current.Response.Redirect("~/");
                ConvertGuidUrl(HttpContext.Current, type, new Guid(identifier));
            }
            //else if (!string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["i"]))
            //{
            //    // gallery image (page or handler). just pass the image-id in, that's enough.
            //    if (!Helpers.IsGuid(HttpContext.Current.Request.QueryString["i"])) HttpContext.Current.Response.Redirect("~/");
            //    if (Helpers.IsGuid(HttpContext.Current.Request.QueryString["i"]))
            //        ConvertGuidUrl(HttpContext.Current, type, new Guid(HttpContext.Current.Request.QueryString["i"]));
            //    else
            //        ConvertNumericUrl(HttpContext.Current, type, long.Parse(HttpContext.Current.Request.QueryString["i"]));
            //}
            else
            {
                if (HttpContext.Current.Request.UrlReferrer != null)
                    Logger.LogWarning(string.Format("Url Conversion Failed, no identifier supplied. Type: {0}, Referer: {1}", type, HttpContext.Current.Request.UrlReferrer.AbsoluteUri));
                HttpContext.Current.Response.Redirect("~/");
            }
        }

        private static void ConvertNumericUrl(HttpContext context, DomainObjectType type, long id)
        {
            var url = String.Empty;
            switch (type)
            {
                case DomainObjectType.Document:
                    {
                        var doc = Server.Instance.ContentServer.GetDocument(id);
                        url = string.Format("/{0}/{1}/{2}", doc.Sections[0].UrlIdentifier, id, WebUtils.ToUrlString(doc.Title));
                    }
                    break;
                case DomainObjectType.Gallery:
                    {
                        var g = Server.Instance.GalleryServer.GetGallery(id);
                        url = string.Format("/galleries/{0}/{1}", id, WebUtils.ToUrlString(g.Title));
                    }
                    break;
                case DomainObjectType.GalleryCategory:
                    {
                        var cat = Server.Instance.GalleryServer.GetCategory(id);
                        url = string.Format("/galleries/category/{0}/{1}", id, WebUtils.ToUrlString(cat.Name));
                    }
                    break;
                case DomainObjectType.GalleryImage:
                    {
                        var img = Server.Instance.GalleryServer.GetImage(id);
                        url = string.Format("/galleries/image/{0}/{1}/{2}", img.ParentGalleryId, id, WebUtils.ToUrlString(img.Name));
                    }
                    break;
                case DomainObjectType.GalleryImageHandler:
                    {
                        var img = Server.Instance.GalleryServer.GetImage(id);
                        url = string.Format("/galleries/img.aspx?g={0}&i={1}", img.ParentGalleryId, id);
                    }
                    break;
                case DomainObjectType.GallerySlideshow:
                    {
                        var g = Server.Instance.GalleryServer.GetGallery(id);
                        url = string.Format("/galleries/slideshow/{0}/{1}", id, WebUtils.ToUrlString(g.Title));
                    }
                    break;
                case DomainObjectType.DirectoryCategory:
                    {
                        var cat = Server.Instance.DirectoryServer.GetCategory(id);
                        url = string.Format("/directory/category/{0}/{1}", id, WebUtils.ToUrlString(cat.Name));
                    }
                    break;
                case DomainObjectType.DirectoryItem:
                    {
                        var item = Server.Instance.DirectoryServer.GetItem(id);
                        url = string.Format("/directory/{0}/{1}", id, WebUtils.ToUrlString(item.Title));
                    }
                    break;
            }

            Redirect(context, url);
        }

        private static void ConvertGuidUrl(HttpContext context, DomainObjectType type, Guid uid)
        {
            var url = String.Empty;
            var newIDs = Server.Instance.LegacyServer.GetNewIdForLegacyUid(type, uid);

            switch (type)
            {
                case DomainObjectType.Document:
                    {
                        var doc = Server.Instance.ContentServer.GetDocument(newIDs[0]);
                        url = string.Format("/{0}/{1}/{2}", doc.Sections[0].UrlIdentifier, doc.Id, WebUtils.ToUrlString(doc.Title));
                    }
                    break;
                case DomainObjectType.DocumentImage:
                    {
                        var doc = Server.Instance.ContentServer.GetDocument(newIDs[1]);
                        var img = doc.EditorialImages.GetImage(newIDs[0]);
                        url = string.Format("/{0}/image/{1}/{2}", doc.Sections[0].UrlIdentifier, doc.Id, img.Id);
                    }
                    break;
                case DomainObjectType.Gallery:
                    {
                        var g = Server.Instance.GalleryServer.GetGallery(newIDs[0]);
                        url = string.Format("/galleries/{0}/{1}", g.Id, WebUtils.ToUrlString(g.Title));
                    }
                    break;
                case DomainObjectType.GalleryCategory:
                    {
                        var cat = Server.Instance.GalleryServer.GetCategory(newIDs[0]);
                        url = string.Format("/galleries/category/{0}/{1}", cat.Id, WebUtils.ToUrlString(cat.Name));
                    }
                    break;
                case DomainObjectType.GalleryImage:
                    {
                        var img = Server.Instance.GalleryServer.GetImage(newIDs[0]);
                        url = string.Format("/galleries/image/{0}/{1}/{2}", img.ParentGalleryId, img.Id, WebUtils.ToUrlString(img.Name));
                    }
                    break;
                case DomainObjectType.GalleryImageHandler:
                    {
                        var img = Server.Instance.GalleryServer.GetImage(newIDs[0]);
                        url = string.Format("/galleries/img.aspx?g={0}&i={1}", img.ParentGalleryId, img.Id);
                    }
                    break;
                case DomainObjectType.GallerySlideshow:
                    {
                        var g = Server.Instance.GalleryServer.GetGallery(newIDs[0]);
                        url = string.Format("/galleries/slideshow/{0}/{1}", g.Id, WebUtils.ToUrlString(g.Title));
                    }
                    break;
                case DomainObjectType.DirectoryCategory:
                    {
                        var cat = Server.Instance.DirectoryServer.GetCategory(newIDs[0]);
                        url = string.Format("/directory/category/{0}/{1}", cat.Id, WebUtils.ToUrlString(cat.Name));
                    }
                    break;
                case DomainObjectType.DirectoryItem:
                    {
                        var item = Server.Instance.DirectoryServer.GetItem(newIDs[0]);
                        url = string.Format("/directory/{0}/{1}", item.Id, WebUtils.ToUrlString(item.Title));
                    }
                    break;
            }

            Redirect(context, url);
        }

        /// <summary>
        /// Performs a permenant redirect to new a new url.
        /// </summary>
        private static void Redirect(HttpContext context, string url)
        {
            context.Response.StatusCode = 301;
            context.Response.RedirectLocation = url;
            context.Response.End();
        }
    }
}