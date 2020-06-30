using System;
using System.Web.UI;

namespace Tetron.Controls
{
    public partial class OpenGraphMetaData : UserControl
    {
        #region accessors
        public string OpenGraphTitle { get; set; }
        public string OpenGraphImage { get; set; }
        public string OpenGraphUrl { get; set; }
        public string OpenGraphDescription { get; set; }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}