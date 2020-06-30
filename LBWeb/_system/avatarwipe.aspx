<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="Apollo.Utilities" %>
<script language="C#" runat="server">
	public void Page_Load(Object sender, EventArgs ea)
	{
		if (Helpers.IsNumeric(Request.Form["member"]))
		{
			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var command = new SqlCommand("UPDATE InstantForum_Members SET AvatarURL = NULL WHERE MemberID = " + Request.Form["member"], connection);
		
			try
			{
				connection.Open();
				command.ExecuteNonQuery();
				Response.Write("* avatar wiped!");
			}
			catch (Exception ex)
			{
				Response.Write("Whoops, there was an error: " + ex.Message);
			}
			finally
			{
				connection.Close();
			}
		}
	}
</script>
<form action="avatarwipe.aspx" method="post">
	Member ID: <input type="text" name="member" /> 
	<input type="submit" value="wipe avatar" />
</form>