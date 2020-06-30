using System.Collections.Generic;

namespace Apollo.Utilities.Web
{
    public class UserSession
    {
        #region accessors
        /// <summary>
        /// Provides access to a collection of PageAction's which can be used to outline actions to be performed on a web-page.
        /// </summary>
        public List<PageAction> PageActions { get; private set; }

        /// <summary>
        /// Represents a user object that may be required by the host web-application.
        /// </summary>
        public object User { get; set; }
        #endregion

        #region constructors
        /// <summary>
        /// Returns a new UserSession object which can be used to track and help with web-site sessions.
        /// </summary>
        public UserSession()
        {
            PageActions = new List<PageAction>();
        }
        #endregion
    }
}