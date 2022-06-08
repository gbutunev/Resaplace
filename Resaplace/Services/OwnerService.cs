using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Resaplace.Data;
using Resaplace.Data.Models;

namespace Resaplace.Services
{
    public class OwnerService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<IdentityUser> userManager;

        public OwnerService(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public async Task<bool> UserExists(IdentityUser user)
        {
            var number = await dbContext
                .Owners
                .Where(x => x.User == user)
                .CountAsync();

            return number > 0;
        }

        public async Task<bool> UserExists(Owner owner)
        {
            return await UserExists(owner.User);
        }

        public async Task AddOwner(Owner newOwner)
        {
            await dbContext
                .Owners
                .AddAsync(newOwner);
            await dbContext.SaveChangesAsync();

            await userManager.AddToRoleAsync(newOwner.User, "Owner");
        }
    }
}
