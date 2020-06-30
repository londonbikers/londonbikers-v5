using System;

namespace Apollo.Utilities.Web
{
    public class PageAction
    {
        #region members
        private string _screenMessage = string.Empty;
        #endregion

        #region accessors
        /// <summary>
        /// A message that needs to be presented on-screen to the user.
        /// </summary>
        public string ScreenMessage { get { return _screenMessage; } set { _screenMessage = value; } }

        /// <summary>
        /// Once processing is complete on a page, the user should be shown a redirection prompt and then redirected on to the following URI.
        /// </summary>
        public Uri OnCompleteRedirectUri { get; set; }
        #endregion

        #region constructors
        /// <summary>
        /// Returns a new PageAction object.
        /// </summary>
        public PageAction()
        {
        }

        /// <summary>
        /// Returns a new PageAction object with a screen-message specified.
        /// </summary>
        public PageAction(string screenMessage)
        {
            ScreenMessage = screenMessage;
        }

        /// <summary>
        /// Returns a new PageAction object with an OnCompleteRedirectUri specified.
        /// </summary>
        public PageAction(Uri onCompleteRedirectUri)
        {
			OnCompleteRedirectUri = onCompleteRedirectUri;
        }
        #endregion
    }
}