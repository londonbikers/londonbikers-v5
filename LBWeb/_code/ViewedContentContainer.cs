using Apollo.Models;

namespace Tetron.Logic
{
	public class ViewedContentContainer
	{
		public long ContentId { get; set; }
		public DomainObjectType DomainObjectType { get; set; }
		public int TimesViewed { get; set; }
	}
}