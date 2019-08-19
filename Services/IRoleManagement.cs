using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FurnitureSiteProject_MentorMate.Services
{
    /// <summary>
    /// Interface defines methods to manage users roles
    /// </summary>
    public interface IRoleManagement
    {
        /// <summary>
        /// Method creates new role in database
        /// </summary>
        /// <param name="role">Role name</param>
        /// <returns>Identity operation result</returns>
         Task<IdentityResult> CreateAsync(IdentityRole role);
    }
}