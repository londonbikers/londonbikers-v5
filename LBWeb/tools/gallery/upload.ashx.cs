using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using Apollo.Utilities;

namespace Tetron.Tools.Gallery
{
    [WebService(Namespace = "http://londonbikers.com/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class UploadHandler : IHttpHandler, IReadOnlySessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.Files == null || context.Request.Files.Count == 0 || string.IsNullOrEmpty(context.Request.Headers["UserUid"]))
            {
                Logger.LogWarning("Gallery uploader failed basic validation!");
                return;
            }

            ProcessUploadedFiles(context);
        }

        public bool IsReusable { get { return false; } }

        #region private methods
        private static void ProcessUploadedFiles(HttpContext context)
        {
            var files = context.Request.Files;
            var workingPath = ConfigurationManager.AppSettings["Global.MediaLibraryPath"] + "galleries\\";
            var uploadedFiles = new List<string>();

            // save the files to the working path.
            if (files == null || files.Count <= 0) return;
            for (var index = 0; index < files.Count; index++)
            {
                // ensure we're only handling JPEG's here.
                var extension = Path.GetExtension(files[index].FileName).ToLower();
                if ((extension != ".jpg" && extension != ".jpeg") && extension != ".jpe") continue;

                if (files[index].ContentLength <= 0) continue;
                var file = workingPath + files[index].FileName;
                files[index].SaveAs(file);
                uploadedFiles.Add(file);
            }

            var key = string.Format("UploadFilenames_User_{0}", context.Request.Headers["UserUid"]);
            context.Application[key] = uploadedFiles;
        }
        #endregion
    }
}
