using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web.Services;
using Apollo.Models;
using Apollo.Utilities;
using Document = Tetron.Logic.Models.Document;

namespace Tetron
{
    /// <summary>
    /// Summary description for editorial_loader
    /// </summary>
    [WebService(Namespace = "http://el.londonbikers.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class EditorialLoader : WebService
    {
        #region methods
        private readonly User _defaultAuthor;
        #endregion

        #region constructors
        public EditorialLoader()
        {
            _defaultAuthor = Apollo.Server.Instance.UserServer.GetUser("londonbikers.com");
        }
        #endregion

        [WebMethod]
        public bool UpdateDocument(Document document)
        {
            // construct Apollo document
            var finalDoc = Apollo.Server.Instance.ContentServer.NewDocument();
            finalDoc.Title = document.Title;
            finalDoc.Abstract = document.Abstract;
            finalDoc.Body = document.Body;
            finalDoc.Created = document.Created;
            finalDoc.Status = "Incoming";
            finalDoc.Author = _defaultAuthor;
            finalDoc.Sections.Add(Logic.Functions.GetConfiguredSite().Channels[0].News);

            foreach (var t in document.Tags)
                finalDoc.Tags.Add(Apollo.Server.Instance.GetTag(t));

            // copy image files to file-store
            var filestorePath = ConfigurationManager.AppSettings["Global.MediaLibraryPath"] + "editorial\\";
            foreach (var i in document.Images)
            {
                // move the image from the email-loader file structure to the Tetron one.
                var filename = Helpers.GetUniqueFilename(filestorePath, i.Filename);
                File.Move(i.Path, filestorePath + filename);

                // create a new Content Image object.
                var editorialImage = new EditorialImage(ObjectCreationMode.New)
                {
                    Type = i.IsSlideshowImage ? ContentImageType.SlideShow : ContentImageType.Normal,
                    Name = i.Name,
                    Filename = filename,
                    Width = i.Width,
                    Height = i.Height
                };

                // persist the image.
                Apollo.Server.Instance.ContentServer.UpdateImage(editorialImage);

                // add image to document.
                finalDoc.EditorialImages.Add(editorialImage.Id);
            }

            // if there's a cover-image in the client object, match it here.
            if (document.Images.Any(q => q.IsCoverImage))
                finalDoc.EditorialImages.CoverImage = document.Images.IndexOf(document.Images.First(q => q.IsCoverImage));

            // save!
            Apollo.Server.Instance.ContentServer.UpdateDocument(finalDoc);
            return true;
        }

        /// <summary>
        /// Checks if a prospective document is duplicate by checking the title. Checks for the documents from today only.
        /// </summary>
        /// <param name="title">The title of the prospective document.</param>
        [WebMethod]
        public bool IsDocumentDuplicateToday(string title)
        {
            return Apollo.Server.Instance.ContentServer.IsDocumentDuplicateToday(title);
        }
    }
}