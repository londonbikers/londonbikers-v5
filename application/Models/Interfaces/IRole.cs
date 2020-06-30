namespace Apollo.Models.Interfaces
{
    public interface IRole
    {
        /// <summary>
        /// The identifier for this Role.
        /// </summary>
        int Id { get; set; }

        /// <summary>
        /// The name for this Role.
        /// </summary>
        string Name { get; set; }
    }
}