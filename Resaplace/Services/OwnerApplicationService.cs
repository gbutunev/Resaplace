using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Resaplace.Data;
using Resaplace.Data.Models;

namespace Resaplace.Services
{
    public class OwnerApplicationService
    {
        private readonly ApplicationDbContext dbContext;

        public OwnerApplicationService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<OwnerApplication> GetLastApplicationByUserAsync(IdentityUser user)
        {
            return await dbContext
                .OwnerApplications
                .OrderByDescending(x => x.CreatedOn)
                .FirstOrDefaultAsync();
        }

        public async Task CreateApplicationAsync(OwnerApplication application)
        {
            await dbContext
                .OwnerApplications
                .AddAsync(application);
            await dbContext.SaveChangesAsync();
        }
    }
}
