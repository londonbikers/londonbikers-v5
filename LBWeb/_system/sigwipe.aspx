<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="Apollo.Utilities" %>
<script language="C#" runat="server">
	public void Page_Load(Object sender, EventArgs ea)
	{
		if (Helpers.IsNumeric(Request.Form["member"]))
		{
			var connection = new SqlConnection(ConfigurationManager.AppSettings["Global.ConnectionString"]);
			var command = new SqlCommand("UPDATE InstantForum_Members SET Signature = NULL WHERE MemberID = " + Request.Form["member"], connection);
		
			try
			{
				connection.Open();
				command.ExecuteNonQuery();
				Response.Write("* sig wiped!");
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
<form action="sigwipe.aspx" method="post">
	Member ID: <input type="text" name="member" /> 
	<input type="submit" value="wipe sig" />
</form>