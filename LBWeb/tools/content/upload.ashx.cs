using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Services;
using Apollo.Utilities;

namespace Tetron.Tools.Content
{
    [WebService(Namespace = "http://londonbikers.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class UploadHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.Files == null || context.Request.Files.Count == 0 || string.IsNullOrEmpty(context.Request.Headers["UserUid"]))
            {
                Logger.LogWarning("Content slideshow-image uploader failed basic validation!");
                return;
            }

            ProcessUploadedFiles(context);
        }

        public bool IsReusable { get { return false; } }

        #region private methods
        private void ProcessUploadedFiles(HttpContext context)
        {
            var files = context.Request.Files;
            var workingPath = ConfigurationManager.AppSettings["Global.MediaLibraryPath"] + "editorial\\";
            var uploadedFileNames = new List<string>();

            // save the files to the working path.
            if (files == null || files.Count <= 0) return;
            for (var index = 0; index < files.Count; index++)
            {
                // ensure the filename is safe.
                var fileName = Helpers.MakeSafeFilename(Path.GetFileName(files[index].FileName).ToLower());

                // ensure the filename is unique.
                fileName = Helpers.GetUniqueFilename(workingPath, fileName);

                if (files[index].ContentLength <= 0) continue;
                files[index].SaveAs(workingPath + fileName);
                uploadedFileNames.Add(fileName);
            }

            var key = string.Format("UploadFilenames_User_{0}", context.Request.Headers["UserUid"]);
            context.Application[key] = uploadedFileNames;
        }
        #endregion
    }
}
