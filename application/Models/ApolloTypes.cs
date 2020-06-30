namespace Apollo.Models
{
	public class ApolloTypes
	{
	    public string[] DocumentStatus { get; private set; }
	    internal ApolloTypes() 
		{
			DocumentStatus = new [] {"Incoming", "New", "Pending", "Ready", "Published", "Inactive"};
		}
	}
}