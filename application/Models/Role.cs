using Apollo.Models.Interfaces;

namespace Apollo.Models
{
	/// <summary>
	/// Describes a role within the security model. Roles afford privledges.
	/// </summary>
	public class Role : IRole
	{
		#region accessors
	    /// <summary>
	    /// The identifier for this Role.
	    /// </summary>
	    public int Id { get; set; }

	    /// <summary>
	    /// The name for this Role.
	    /// </summary>
	    public string Name { get; set; }
	    #endregion

		#region constructors
		/// <summary>
		/// Creates a new Role object.
		/// </summary>
		internal Role()
		{
			Id = 0;
			Name = string.Empty;
		}
		#endregion
	}
}
