using System.Drawing;
using Apollo.Utilities;

namespace Tetron.Logic
{
    #region enums
    /// <summary>
    /// Defines the type of image to rezize.
    /// </summary>
    public enum DynamicImageType
    {
        GalleryThumbnail,
        GalleryImage,
        Editorial,
        ForumAttachment,
        ForumAvatar
    }

    public enum DynamicImageResizeType
    {
        ByWidth,
        ByLongestSide,
        Square,
        Letterbox,
        SetSize
    }
    #endregion

    public partial class Functions
    {
        public static string DynamicImage(string id,
            DynamicImageType imageType = DynamicImageType.Editorial,
            DynamicImageResizeType resizeType = DynamicImageResizeType.ByLongestSide,
            bool showWatermark = false,
            bool logView = false)
        {
            return DynamicImage(id, null, imageType, resizeType, showWatermark, logView);
        }

        public static string DynamicImage(string id, 
            int primaryAxisSize, 
            DynamicImageType imageType = DynamicImageType.Editorial,
            DynamicImageResizeType resizeType = DynamicImageResizeType.ByLongestSide,
            bool showWatermark = false,
            bool logView = false)
        {
            var size = new Size {Width = primaryAxisSize};
            return DynamicImage(id, size, imageType, resizeType, showWatermark, logView);
        }

        public static string DynamicImage(string id, 
            Size? specificSize,
            DynamicImageType imageType = DynamicImageType.Editorial,
            DynamicImageResizeType resizeType = DynamicImageResizeType.Letterbox,
            bool showWatermark = false,
            bool logView = false)
        {
            var encrypt = false;
            var url = "/img.ashx?";
            var query = string.Empty;
            query += string.Format("id={0}", id);

            // resolve image type (Editorial is the default for the handler).
            switch (imageType)
            {
                case DynamicImageType.ForumAttachment:
                    query += "&s=fla";
                    break;
                case DynamicImageType.ForumAvatar:
                    query += "&s=fa";
                    break;
                case DynamicImageType.GalleryImage:
                    query += "&s=gi";
                    break;
                case DynamicImageType.GalleryThumbnail:
                    query += "&s=gt";
                    break;
            }

            // resolve image aspect and sizings.
            switch (resizeType)
            {
                case DynamicImageResizeType.Square:
                    query += "&c=1";
                    if (specificSize.HasValue)
                        query += "&w=" + specificSize.Value.Width;
                    break;
                case DynamicImageResizeType.Letterbox:
                    query += "&l=1";
                    if (specificSize.HasValue)
                        query += string.Format("&w={0}&h={1}", specificSize.Value.Width, specificSize.Value.Height);
                    break;
                case DynamicImageResizeType.SetSize:
                    query += "&ss=1";
                    if (specificSize.HasValue)
                        query += string.Format("&w={0}&h={1}", specificSize.Value.Width, specificSize.Value.Height);
                    break;
                case DynamicImageResizeType.ByLongestSide:
                    if (specificSize.HasValue)
                        query += "&w=" + specificSize.Value.Width;
                    break;
                case DynamicImageResizeType.ByWidth:
                    if (specificSize.HasValue)
                        query += "&wr=1&w=" + specificSize.Value.Width;
                    break;
            }

            if (specificSize.HasValue && specificSize.Value.Width > 200 && !showWatermark)
            {
                // this requires an encrypted query.
                query += "&nw=1";
                encrypt = true;
            }

            if (encrypt)
                query = "e=" + SecurityHelpers.DesEncrypt(query);

            url += query;
            return url;
        }
    }
}