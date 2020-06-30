using System;
using System.Web.UI;

namespace Tetron.Controls
{
    public partial class DirectorySearchBox : UserControl
    {
        #region members
        protected string Criteria;
        protected string TitleChecked;
        protected string ConceptChecked;
        protected bool StackedFormat = true;
        private bool _showModeControls = true;
        #endregion

        #region accessors
        /// <summary>
        /// Determines whether or not the mode controls are shown.
        /// </summary>
        public bool ShowModeControls { get { return _showModeControls; } set { _showModeControls = value; } }
        /// <summary>
        /// Determines whether or not the search box should go top down, or left to right.
        /// </summary>
        public bool IsStackedFormat { get { return StackedFormat; } set { StackedFormat = value; } }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            Criteria = Request.QueryString["s"];

            if (Request.QueryString["m"] == "t")
                TitleChecked = " checked=\"checked\"";
            else
                ConceptChecked = " checked=\"checked\"";
        }
    }
}