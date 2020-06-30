using System;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;

namespace Apollo.Utilities.Web
{
    public class WebUtils
    {
        /// <summary>
        /// Turns any literal URL references in a block of text into ANCHOR html elements.
        /// </summary>
        public static string ActivateLinksInText(string source)
        {
            source = " " + source + " ";
	        // easier to convert BR's to something more neutral for now.
            source = Regex.Replace(source, "<br>|<br />|<br/>", "\n");
            source = Regex.Replace(source, @"([\s])(www\..*?|http://.*?)([\s])", "$1<a href=\"$2\" target=\"_blank\">$2</a>$3");
            source = Regex.Replace(source, @"href=""www\.", "href=\"http://www.");
            //source = Regex.Replace(source, "\n", "<br />");
            return source.Trim();
        }

		/// <summary>
		/// Remves any profanity or other censored words from the source text.
		/// </summary>
		public static string FilterOutCensoredWords(string source)
		{
			if (ConfigurationManager.AppSettings["Apollo.Utilities.CensoredPhrases"] == null)
				return source;

			var phrases = ConfigurationManager.AppSettings["Apollo.Utilities.CensoredPhrases"].Split(char.Parse(","));
            return phrases.Aggregate(source, (current, phrase) => Regex.Replace(current, "\\b" + phrase + "\\b", "***", RegexOptions.IgnoreCase));
		}

        /// <summary>
        /// Strips out any HTML elements from a text fragment.
        /// </summary>
        public static string FilterOutHtml(string source)
        {
            source = Regex.Replace(source, "<br>", "\n", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "<br/>", "\n", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "<br />", "\n", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "<[^>].*>", " ");
            source = Regex.Replace(source, "&nbsp;", " ", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "&amp;", "&", RegexOptions.IgnoreCase);
            source = source.Replace("<", "&lt;");
            source = source.Replace(">", "&gt;");
            return source.Trim();
        }

        /// <summary>
        /// Strips out any potentially malicious SQL injection scripts.
        /// </summary>
        public static string FilterOutSqlInjection(string source)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;

            if (!string.IsNullOrEmpty(source))
            {
                var badCommands = "';,;,--,\bsp_,xp_".Split(char.Parse(","));
                source = badCommands.Aggregate(source, (current, t) => Regex.Replace(current, t, String.Empty, RegexOptions.IgnoreCase));
            }

            return source.Trim();
        }

        /// <summary>
        /// Strips out any Javascript statements from a text fragment.
        /// </summary>
        public static string FilterOutScripting(string source)
        {
            source = Regex.Replace(source, "<script[^>].*>.*?</script[^><].*>", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "<script>.*?</script>", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "<script[^>].*>.*?</script>", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "<script>.*?</script[^><].*>", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "javascript:", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "<input[^>].*>.*?</input[^><].*>", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "<object[^>].*>.*?</object[^><].*>", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "<applet[^>].*>.*?</applet[^><].*>", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "<form[^>]*>.*?</form[^><].*>", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "<option[^>].*>.*?</option[^><].*>", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "<select[^>].*>.*?</select[^><].*>", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "<iframe[^>].*>.*?</iframe[^><].*>", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "<form[^>].*>", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "</form[^><].*>", "", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "onabort=", "&#111;nabort=", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "onblur=", "&#111;nblur=", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "onchange=", "&#111;nchange=", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "onclick=", "&#111;nclick=", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "ondblclick=", "&#111;ndblclick=", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "ondragdrop=", "&#111;ndragdrop=", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "onerror=", "&#111;nerror=", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "onfocus=", "&#111;nfocus=", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "onerror=", "&#111;nerror=", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "onkeydown=", "&#111;nkeydown=", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "onkeypress=", "&#111;nkeypress=", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "onkeyup=", "&#111;nkeyup=", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "onload=", "&#111;nload=", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "onmousedown=", "&#111;nmousedown=", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "onmouseout=", "&#111;nmouseout=", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "onmouseover=", "&#111;nmouseover=", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "onmousemove=", "&#111;nmousemove=", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "onmouseup=", "&#111;nmouseup=", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "onserverchange=", "&#111;nserverchange=", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "onmove=", "&#111;nmove=", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "onreset=", "&#111;nreset=", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "onresize=", "&#111;nresize=", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "onselect=", "&#111;nselect=", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "onsubmit=", "&#111;nsubmit=", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, "onunload=", "&#111;nunload=", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, @"window\.", "&#119;indow.", RegexOptions.IgnoreCase);
            source = Regex.Replace(source, @"document\.", "&#100;ocument.", RegexOptions.IgnoreCase);

            return source;
        }

        /// <summary>
        /// Converts common characters and statements into the appropriate HTML representation.
        /// </summary>
        public static string PlainTextToHtml(string sPlainText)
        {
            sPlainText = Regex.Replace(sPlainText, "& ", "&amp; ", RegexOptions.Multiline);
            sPlainText = Regex.Replace(sPlainText, " < ", " &lt; ", RegexOptions.Multiline);
            sPlainText = Regex.Replace(sPlainText, " > ", " &gt; ", RegexOptions.Multiline);

            var sb = new StringBuilder((int)(sPlainText.Length * 1.2));
            foreach (var t in sPlainText)
            {
                switch (t)
                {
                    case '\n':
                        sb.Append("<br />");
                        break;
                    case '\r':
                        break;
                    case '\t':
                        break;
                    case '£':
                        sb.Append("&pound;");
                        break;
                    case '$':
                        sb.Append("&#36;");
                        break;
                    case '®':
                        sb.Append("&reg;");
                        break;
                    case '©':
                        sb.Append("&copy;");
                        break;
                    case '»':
                        sb.Append("&#187;");
                        break;
                    case '¼':
                        sb.Append("&#188;");
                        break;
                    case '½':
                        sb.Append("&#189;");
                        break;
                    case '¾':
                        sb.Append("&#190;");
                        break;
                    default:
                        sb.Append(t);
                        break;
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Ensures that a string will not break a html element if placed into a parameter. Useful for ensuring tooltips are safe.
        /// </summary>
        public static string ToSafeHtmlParameter(string textToMakeSafe)
        {
            return textToMakeSafe.Replace("\"", "'");
        }

        /// <summary>
        /// Encodes url strings into a human-friendly format.
        /// </summary>
        public static string SimpleUrlEncode(string textToEncode)
        {
            textToEncode = HttpUtility.HtmlEncode(textToEncode);
            textToEncode = textToEncode.Replace("-", "--");
            textToEncode = textToEncode.Replace(" ", "-");
            return textToEncode;
        }

        /// <summary>
        /// Decodes url strings from a human-friendly format.
        /// </summary>
        public static string SimpleUrlDecode(string textToDecode)
        {
            textToDecode = HttpUtility.HtmlDecode(textToDecode);
            textToDecode = textToDecode.Replace("--", "-");
            textToDecode = textToDecode.Replace("-", " ");
            return textToDecode;
        }

        /// <summary>
        /// Performs a Screen-Scrape of a specified URL.
        /// </summary>
        /// <param name="url">The full URI of the page to scrape.</param>
        /// <returns>The complete content response from the URL, whether it be XML/HTML, etc.</returns>
        public static string GetResponseFromUrl(string url)
        {
            string result;
            using (var webClient = new WebClient())
            {
                var html = webClient.DownloadData(url);
                var utf8 = new UTF8Encoding();
                result = utf8.GetString(html);
            }

            return result;
        }

        /// <summary>
        /// Returns the page segment of a URL.
        /// </summary>
        public static string PageNameFromUrl(string path)
        {
            return path.Substring((path.LastIndexOf("/") + 1));
        }

         /// <summary>
        /// Fills the contents of a DropDownList with the names and values from an Enum.
        /// </summary>
        public static void PopulateDropDownFromEnum(DropDownList list, object enumToUse, bool useEnumValueAsItemValue)
        {
            if (!enumToUse.GetType().IsEnum)
                return;

            ListItem item;
            var values = Enum.GetValues(enumToUse.GetType());

            for (var i = 0; i < values.Length; i++)
            {
                item = new ListItem { Text = Enum.GetName(enumToUse.GetType(), values.GetValue(i)) };
                item.Value = (useEnumValueAsItemValue) ? ((int)Enum.Parse(enumToUse.GetType(), values.GetValue(i).ToString())).ToString() : item.Text;
                list.Items.Add(item);
            }
        }

        /// <summary>
        /// Removes unwanted characters from a string, to be used in a HTML attribute, i.e. a title attribute.
        /// </summary>
        public static string ToAttributeString(string text)
        {
            return text.Replace("\"", string.Empty).Replace("'", string.Empty).Replace("<", string.Empty).Replace(">", string.Empty);
        }

        /// <summary>
        /// Converts strings into ones that can be safely appended to url's for SEO/Url Re-writing.
        /// </summary>
        public static string ToUrlString(string text)
        {
            text = text.Replace(" ", "-");
            var urlRegex = new Regex(@"[^\w-_]");
            text = urlRegex.Replace(text, string.Empty);
            text = Regex.Replace(text, "-{2,}", "-");
            text = Regex.Replace(text, "^-|-$", String.Empty);
            return text.ToLower();
        }

        public static string FromUrlString(string urlText)
        {
            urlText = urlText.Replace("--", "##");
            urlText = urlText.Replace("-", " ");
            urlText = urlText.Replace("##", "-");
            return urlText;
        }
    }
}