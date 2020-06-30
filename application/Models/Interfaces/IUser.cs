using System;
using System.Collections;

namespace Apollo.Models.Interfaces
{
    public interface IUser
    {
        /// <summary>
        /// The email address for this User.
        /// </summary>
        string Email { get; set; }

        /// <summary>
        /// The Username for this User. The value must be distinct in the system.
        /// </summary>
        string Username { get; set; }

        /// <summary>
        /// The users christian name.
        /// </summary>
        string Firstname { get; set; }

        /// <summary>
        /// The users surname.
        /// </summary>
        string Lastname { get; set; }

        /// <summary>
        /// The users chosen login password. Represented as clear-text.
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// The exact date the User was created in the system.
        /// </summary>
        DateTime Created { get; set; }

        /// <summary>
        /// A collection of any Roles that the user may have associated with them.
        /// </summary>
        ArrayList Roles { get; }

        /// <summary>
        /// The unique identifier for this User.
        /// </summary>
        Guid Uid { get; set; }

        /// <summary>
        /// A reference to the users account within the forums.
        /// </summary>
        int ForumUserId { get; set; }

        /// <summary>
        /// The status of the User within the system.
        /// </summary>
        UserStatus Status { get; set; }

        /// <summary>
        /// If the users avatar image file is stored off-site or not network-accessible then this will contain the uri reference. Otherwise use the loca path property.
        /// </summary>
        Uri AvatarUri { get; set; }

        /// <summary>
        /// References the local or network-accessible user avatar image file.
        /// </summary>
        string AvatarPath { get; set; }

        /// <summary>
        /// Immediately associate a Role with the User object.
        /// </summary>
        void AddRole(Role role);

        /// <summary>
        /// Determines whether or not the User has a Role already associated with them.
        /// </summary>
        bool HasRole(Role role);

        bool HasRole(string roleName);

        /// <summary>
        /// Immediately disassociates a Role with the User object.
        /// </summary>
        void RemoveRole(Role role);
    }
}