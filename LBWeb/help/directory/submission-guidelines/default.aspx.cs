using System;
using System.Web.UI;

namespace Tetron.Help.Directory.SubmissionGuidelines
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Master != null) ((SiteMaster)Master).PageTitle = "Help: Directory Submission Guidelines";
        }
    }
}