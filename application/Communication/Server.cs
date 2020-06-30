using System;
using System.IO;
using System.Configuration;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Apollo.Caching;
using Apollo.Models;
using Apollo.Utilities;
using Google.GData.Client;
using Google.GData.Calendar;

namespace Apollo.Communication
{
	/// <summary>
	/// The controller for the Communication module. All major Communication object operations are performed via here.
	/// </summary>
	public class CommunicationServer
	{
		#region constructors
		/// <summary>
		/// Creates a new CommunicationServer object.
		/// </summary>
		internal CommunicationServer() 
		{
		}
		#endregion

		#region public methods
		/// <summary>
		/// Constructs a branded email, based upon the arguments supplied, and then delivers it.
		/// </summary>
		/// <param name="emailType">The Apollo mail to send out, i.e. RegistrationConfirmation</param>
		/// <param name="isHtml">Is the mail HTML or TEXT format?</param>
		/// <param name="recipient">The email address for the person to send this mail to</param>
		/// <param name="arguments">The values to fill the mail with. Must be ordered according to the fill-order.</param>
		/// <returns>An indicator, saying whether or not the delivery was successful.</returns>
		public bool SendMail(EmailType emailType, bool isHtml, string recipient, string[] arguments) 
		{
            try
            {
                if (arguments == null || arguments.Length == 0)
                    return false;

                var format = (isHtml) ? "html" : "text";

                // determine which template to use.
                var templatePath = ConfigurationManager.AppSettings["Tetron.SystemFilePath"] + emailType + "_" + format + ".tpl";
                var template = File.ReadAllText(templatePath);

                // template not readable.
                if (template == String.Empty)
                    return false;

                // extract the mail subject.
                var subjectMatch = Regex.Match(template, "(##subject=(.*?)##)", RegexOptions.IgnoreCase);
                var subject = subjectMatch.Groups[2].Value;
                template = template.Replace(subjectMatch.Groups[1].Value, String.Empty);

                // merge the arguments with the template.
                for (var i = 0; i < arguments.Length; i++)
                    template = template.Replace("##" + i + "##", arguments[i]);

                // construct and deliver the email.
                var mail = new MailMessage();

                mail.To.Add(recipient);
                mail.Subject = subject;
                mail.IsBodyHtml = isHtml;
                mail.Body = template;
                mail.From = new MailAddress(ConfigurationManager.AppSettings["Tetron.SystemMailAddress"]);

                var smtp = new SmtpClient(ConfigurationManager.AppSettings["Tetron.SmtpServer"]);
                smtp.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                var message = string.Format("type: {0}, isHtml: {1}, recipient: {2}\nArguments: {3}", emailType, isHtml, recipient, arguments);
                Logger.LogException(ex, message);
                return false;
            }
		}
        
		/// <summary>
		/// Retrieves the upcoming events from the Google calendar.
		/// </summary>
		/// <param name="ceiling">The maximum number of entries to return.</param>
		/// <returns>A collection of Google Entry objects.</returns>
		public AtomEntryCollection GetUpcomingEvents(int ceiling) 
		{
			AtomEntryCollection entries = null;
			const string cacheKey = "UpcomingEventsGoogleCalendarFeed";
			var cachedTime = CacheManager.RetrieveItemCreationDate(cacheKey);

			if (cachedTime == DateTime.MinValue || DateTime.Now.Date > cachedTime.Date)
			{
				try
				{
					const string calendarUri = "http://www.google.com/calendar/feeds/mc50u40fgm9nvhhli686nmv1i4%40group.calendar.google.com/public/basic";
					var query = new EventQuery();
					var service = new CalendarService("MediaPanther.Apollo");

					query.Uri = new Uri(calendarUri);
					query.SortOrder = CalendarSortOrder.ascending;
					query.ExtraParameters = "orderby=starttime";
					query.StartTime = DateTime.Now.Date;
					query.EndTime = DateTime.Now.Date.AddDays(30);
					query.SingleEvents = true;
					query.NumberToRetrieve = ceiling;

					var feed = service.Query(query);
					entries = feed.Entries;

					// this might be an update, so just remove the old item first.
					CacheManager.RemoveItem(cacheKey);
					CacheManager.AddItem(entries, cacheKey);
				}
				catch (Exception ex)
				{
					Logger.LogException(ex, "Google calendar feed retrieval.");
				}
			}
			else
			{
				// retrieve from cache.
				entries = CacheManager.RetrieveItem(cacheKey) as AtomEntryCollection;
			}

			return entries;
		}
		#endregion
	}
}