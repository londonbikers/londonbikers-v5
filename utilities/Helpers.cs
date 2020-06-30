using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace Apollo.Utilities
{
	public class Helpers
	{
        public static double RoundUp(double valueToRound)
        {
            return (Math.Floor(valueToRound + 0.5));
        }

		/// <summary>
		/// Converts a filesystem-friendly filename into a human-friendly name, i.e. valentino-rossi-wins into "Valentino Rossi Wins".
		/// </summary>
		/// <param name="filename">The filename minus the extension (and period).</param>
		public static string FilenameToFriendlyName(string filename)
		{
			// replace encoded characters.
			filename = filename.Replace("%20", " ");

			// remove unwanted characters
            filename = Regex.Replace(filename, "[^a-z0-9-_]", String.Empty, RegexOptions.IgnoreCase);

			// remove duplicate delimeters.
			filename = filename.Replace("--", "-");
			filename = filename.Replace("__", "_");

			// convert spaces.
			filename = filename.Replace("-", " ");
			filename = filename.Replace("_", " ");

			filename = ToCapitalised(filename);
			return filename;
		}
        
		/// <summary>
		/// Takes a DateTime object and returns a human-friendly date string such as 'yesterday, 12.14pm'.
		/// </summary>
		public static string ToRelativeDateString(DateTime timeToTranslate) 
		{
			return ToRelativeDateString(timeToTranslate, true);
		}

	    /// <summary>
	    /// Takes a DateTime object and returns a human-friendly date string such as 'yesterday, 12.14pm'.
	    /// </summary>
	    /// <param name="timeToTranslate">The date to convert.</param>
	    /// <param name="showTime">Determines whether or not a specific datestamp is added to the end of the relative date wording.</param>
	    public static string ToRelativeDateString(DateTime timeToTranslate, bool showTime) 
		{
            var now = DateTime.Now;
			string date;
            var timeStamp = string.Format("{0}, {1}", timeToTranslate.ToShortDateString(), timeToTranslate.ToShortTimeString());
            var shortTimeStamp = timeToTranslate.ToShortTimeString();
			var difference = now - timeToTranslate;

            if (difference.TotalSeconds <= 60)
            {
                // less than a minte.
                date = "Less than a minute ago";
                showTime = false;
            }
            else if (difference.TotalMinutes <= 60)
            {
                // less than an hour.
                date = now.Date != timeToTranslate.Date ? "Yesterday" : string.Format("{0} Minutes ago", Convert.ToInt32(difference.TotalMinutes));
                timeStamp = shortTimeStamp;
            }
            else if (difference.TotalHours <= 48)
            {
                // less than a day.
                timeStamp = shortTimeStamp;
                date = now.Date != timeToTranslate.Date ? "Yesterday" : string.Format("{0} Hours ago", Convert.ToInt32(difference.TotalHours));
            }
            else if (difference.TotalDays <= 7)
            {
                // less than a week.
                date = string.Format("{0} Days ago", Convert.ToInt32(difference.TotalDays));
                timeStamp = shortTimeStamp;
            }
            else
            {
                // old, just display the date.
                date = timeToTranslate.ToLongDateString();
                if (showTime)
                    date += ", " + timeToTranslate.ToShortTimeString();

                showTime = false;
            }

            // add the time-stamp to the end?
            if (showTime)
            {
                // if this is an old date, we need to just write the time out.
                if (date == string.Empty)
                    date = timeStamp;
                else
                    date += ", " + timeStamp;
            }

			return date;
		}

		/// <summary>
		/// Capitalises each word in a sentence, i.e. converts 'sports bikes' to 'Sports Bikes'.
		/// </summary>
		public static string ToCapitalised(string textToCapitalise) 
		{
			if (string.IsNullOrEmpty(textToCapitalise))
				return string.Empty;

			textToCapitalise = textToCapitalise.Trim();
			var words = textToCapitalise.Split(char.Parse(" "));

			for (var i = 0; i < words.Length; i++)
			{
				if (words[i].Length > 1)
					words[i] = words[i].Substring(0, 1).ToUpper() + words[i].Substring(1);
			}

			textToCapitalise = words.Aggregate(string.Empty, (current, t) => current + (t + " "));
		    textToCapitalise = textToCapitalise.Trim();
			return textToCapitalise;
		}

		/// <summary>
		/// Email & RSS feeds make use of the RFC822 specification date-time, this converts a DateTime to that format in string form.
		/// </summary>
		public static string DateTimeToRfc822String(DateTime date) 
		{
			return date.ToString("ddd, dd MMM yyyy HH:mm:ss ") + "GMT";
		}

        /// <summary>
        /// Converts a DateTime to an ISO 8601 string, aka W3C Datetime format.
        /// </summary>
        public static string DateTimeToIso8601String(DateTime date)
        {
            // zulu time if you can believe it.
            return date.ToString("u");
            //return date.ToString("s");
        }

		/// <summary>
		/// As Image files in the contentstore retain their original filenames, they must be unique or an existing image
		/// may be overwritten by a newer Image. This function checks to see if there's a file already in the contentstore
		/// with the filename, if one's found, it'll adapt it to make sure it's unique.
		/// </summary>
		public static string GetUniqueFilename(string path, string filename) 
		{
            filename = filename.Trim();
			var isUnique = false;
			var filenamePart = Path.GetFileNameWithoutExtension(filename);
			var extension = Path.GetExtension(filename);
		    var count = 0;

			if (!path.EndsWith("\\"))
				path += "\\";
		
			while (!isUnique)
			{
				if (File.Exists(path + filename))
				{
                    filenamePart += string.Format("-{0}", count);
					filename = filenamePart + extension;
				}
				else
				{
					isUnique = true;
				}

			    count++;
			}

			return filename;
		}

		/// <summary>
		/// Transforms a filename so that it doesn't contain any illegal characters.
		/// </summary>
		/// <param name="filename">The filename to transform.</param>
		public static string MakeSafeFilename(string filename)
		{
            if (string.IsNullOrEmpty(filename))
                return string.Empty;

			var name = Path.GetFileNameWithoutExtension(filename);
			var extension = Path.GetExtension(filename);

            if (string.IsNullOrEmpty(name))
                return string.Empty;

            if (string.IsNullOrEmpty(extension))
                return string.Empty;

			// remove any naming faux-pas.
			name = name.Replace(extension, string.Empty);
			name = name.Replace("&", string.Empty);
			name = name.Replace("%", string.Empty);
			name = name.Replace(" ", "-");
			name = name.Replace("{", string.Empty);
			name = name.Replace("}", string.Empty);
			name = name.Replace("(", string.Empty);
			name = name.Replace(")", string.Empty);
            name = name.Replace("'", string.Empty);
            name = name.Replace("\"", string.Empty);
            name = name.Replace("{", string.Empty);
            name = name.Replace("}", string.Empty);
            name = name.Replace("!", string.Empty);
			name = name.Replace(".", "-");
			name = name.Replace(",", string.Empty);
			
			return name + extension;
		}

		/// <summary>
		/// Copies a string a set number of times.
		/// </summary>
		public static string MultiplyString(string stringToMultiply, int count) 
		{
			var builder = new StringBuilder();
			for (var i = 0; i < count; i++)
				builder.Append(stringToMultiply);

			return builder.ToString();
		}

		/// <summary>
		/// Validates a UK postal-code.
		/// </summary>
		public static bool IsPostCode(string postcode)
		{
            if (string.IsNullOrEmpty(postcode))
                return false;

		    postcode = postcode.Trim().Replace(" ", string.Empty);
		    return postcode.Length <= 8;
		}

	    /// <summary>
		/// Tests to see if a string numeral is a valid integer.
		/// </summary>
		public static bool IsNumeric(string numeral) 
		{
            if (string.IsNullOrEmpty(numeral))
                return false;

	        long result;
	        return long.TryParse(numeral, out result);
		}

		/// <summary>
		/// Performs a check to see if an input string matches the format for an Email address.
		/// </summary>
		public static bool IsEmail(string email) 
		{
            if (string.IsNullOrEmpty(email))
                return false;

            try
            {
                #pragma warning disable 168
                var addy = new MailAddress(email);
                #pragma warning restore 168
                return true;
            }
            catch
            {
                return false;
            }
		} 

		/// <summary>
		/// Performs a check to see if an input string matches the format for a valid date.
		/// </summary>
		public static bool IsDate(string date) 
		{
            if (string.IsNullOrEmpty(date))
                return false;

		    DateTime result;
		    return DateTime.TryParse(date, out result);
		}

		/// <summary>
		/// Performs a check to see if an input string matches the format for a valid Guid.
		/// </summary>
		public static bool IsGuid(string guid) 
		{
            if (string.IsNullOrEmpty(guid))
                return false;

            try
            {
                var g = new Guid(guid);
                return true;
            }
            catch
            {
                return false;
            }
		} 

        /// <summary>
        /// Returns the first paragraph from a block of text.
        /// </summary>
        /// <remarks>
        /// This method is written mainly to support the Apollo CMS and TinyMCE's odd html variances.
        /// </remarks>
        public static string GetFirstParagraph(string text, out string remainder)
        {
            //originalParagraphLength = 0;
            remainder = string.Empty;
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            // perform some norminalisation
            //text = text.Replace("align=\"justify\"", string.Empty);
            text = text.Replace("<div align=\"justify\">", "<div>");
            text = text.Replace("<div align=\"left\">", "<div>");
            text = text.Replace("<div align=\"right\">", "<div>");
            text = text.Replace("<div style=\"text-align: justify\">", "<div>");
            text = text.Replace("<div>&nbsp;</div>", string.Empty);
            text = text.Replace("<div>", "<p>");
            text = text.Replace("</div>", "</p>");
            text = text.Replace("<br>", "<br />");

            string delim;
            if (text.IndexOf("\r\n\r\n") == -1)
            {
                var pPosition = text.IndexOf("</p>");
                var bPosition = text.IndexOf("<br />");

                // looks like we've got just one paragraph here, so just return.
                if (pPosition == -1 && bPosition == -1)
                    return text;

                if (pPosition > -1 && bPosition > -1)
                {
                    delim = pPosition < bPosition ? "</p>" : "<br />";
                }
                else if (pPosition > -1)
                    delim = "</p>";
                else
                    delim = "<br />";
            }
            else
            {
                delim = "\r\n\r\n";
            }
            
            // if there's no delimiter, then just return.
            if (string.IsNullOrEmpty(delim))
                return text;

            remainder = text.Substring(text.IndexOf(delim) + delim.Length);
            text = text.Substring(0, text.IndexOf(delim) + delim.Length);

            // perform some more norminalisation
            text = text.Replace("<br />  <br />", "<br /><br />");
            text = text.Replace("<b>", string.Empty).Replace("</b>", string.Empty).Replace("<p>", string.Empty).Replace(
                "</p>", string.Empty).Replace("<strong>", string.Empty).Replace("</strong>", string.Empty).Replace(
                    "<span style=\"font-weight: bold\">", string.Empty).Replace("</span>", string.Empty).Replace(
                        "<span>", string.Empty);

            text = text.Replace("<br />", string.Empty);
            return text.Trim();
        }

		/// <summary>
		/// Restricts the length of a string by a certain number of characters and adds an elipses to the end if too long.
		/// </summary>
		public static string ToShortString(string text, int length) 
		{			
			if (text.Length > length)
				text = string.Concat(text.Substring(0, length), "...");

			return text;
		}

	    /// <summary>
		/// Processes an ArrayList collection by turning it's textual values into a delimited string, ready
		/// for persistence.
		/// </summary>
		/// <param name="collection">Must be an ArrayList of textual elements.</param>
		/// <returns>A delimited string, using ',' as the delimiter.</returns>
		public static string CollectionToDelimitedString(List<string> collection) 
		{
			var csv = collection.Where(element => element.Trim() != string.Empty).Aggregate(string.Empty, (current, element) => current + (element.Trim() + ","));
	        csv = csv.TrimEnd(new[]{char.Parse(",")});
			return csv;
		}

	    /// <summary>
	    /// Processes a string and populates a collection by turning it's textual values into an item collection.
	    /// </summary>
	    /// <param name="collection">The list to populate.</param>
	    /// <param name="delimitedString">The CSV to convert.</param>
	    public static void DelimitedStringToCollection(List<string> collection, string delimitedString) 
		{
	        if (string.IsNullOrEmpty(delimitedString))
                return;

	        delimitedString = delimitedString.Trim();
	        delimitedString = delimitedString.Replace(", ", ",");
	        delimitedString = delimitedString.Replace(" ,", ",");

	        var items = delimitedString.Split(char.Parse(","));
	        collection.AddRange(items.Where(t => t.Trim() != string.Empty).Select(t => t.Trim()));
		}
	}
}