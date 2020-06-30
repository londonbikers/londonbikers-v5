using System;
using System.IO;

namespace Apollo.Utilities.Files
{
    public static class Files
    {
        /// <summary>
        /// Translates a byte filesize into a human-friendly filesize, i.e. 24kb, 31mb or 43.01gb
        /// </summary>
        public static string GetFriendlyFilesize(long filesize)
        {
            if (filesize < 1)
                return "-";

            if (filesize < 1048576)
            {
                // under one mb.
                return (filesize / 1024L).ToString("N0") + "kb";
            }
            if (filesize < 1073741824)
            {
                // under one gb.
                return (filesize / 1048576L).ToString("N0") + "mb";
            }
            // over one gb. could fail but I can't get it to do a proper divide.
            try
            {
                var gb = filesize / (decimal)1073741824;
                return gb.ToString("N2") + "gb";
            }
            catch
            {
                return "??";
            }
        }

        /// <summary>
        /// Attempts to delete a file and keeps on attempting until a file lock is released or the timeout value is reached.
        /// </summary>
        /// <param name="msTimeout">The time in miliseconds to wait before giving up.</param>
        /// <param name="filePath">The local or unc path to the file to delete.</param>
        public static void DeleteFile(int msTimeout, string filePath)
		{
			var startTime = DateTime.Now;
			while (DateTime.Now.Subtract(startTime).Milliseconds < msTimeout)
			{
				try
				{
					File.Delete(filePath);
				    return;
				}
				catch (FileNotFoundException)
				{
					// bad file-path, or the file has already been deleted.
					Logger.LogWarning(string.Format("DeleteFile() - File not found: '{0}'.", filePath));
				    return;
				}
                // ReSharper disable EmptyGeneralCatchClause
				catch
                // ReSharper restore EmptyGeneralCatchClause
				{
					// file must be locked, continue.
				}
			}

            return;
		}
    }
}