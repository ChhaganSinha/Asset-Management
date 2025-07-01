// UserPermissionRepository.cs
using Microsoft.EntityFrameworkCore;
using AssetManagement.DataContext.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagement.DataContext.Repositories
{
    public class UserPermissionRepository
    {
        private readonly AuthDbContext _dbContext;

        public UserPermissionRepository(AuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<string>> GetUserPermissionsAsync(string userId)
        {
            var permissions = await _dbContext.UserPermissions
                .Where(up => up.UserId == userId)
                .Select(up => up.Permission)
                .ToListAsync();

            return permissions;
        }
    }
}
