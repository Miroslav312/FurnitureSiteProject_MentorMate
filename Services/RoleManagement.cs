using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FurnitureSiteProject_MentorMate.Services
{
    /// <summary>
    /// Class defines methods to manage users roles
    /// </summary>
    public class RoleManagement : IRoleManagement
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleManagement(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        /// <summary>
        /// Method creates new role in database
        /// </summary>
        /// <param name="role">Role name</param>
        /// <returns>Identity operation result</returns>
        public async Task<IdentityResult> CreateAsync(IdentityRole role)
        {
            return await _roleManager.CreateAsync(role);
        }
    }
}