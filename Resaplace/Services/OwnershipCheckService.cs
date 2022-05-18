using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Resaplace.Data;

namespace Resaplace.Services
{
    public class OwnershipCheckService
    {
        private readonly ApplicationDbContext dbContext;

        public OwnershipCheckService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> IsRestaurantOwnedByUser(int restaurantId, IdentityUser owner)
        {
            var result = await dbContext
                .Restaurants
                .Where(x => x.Id == restaurantId)
                .Where(x => x.Owner == owner)
                .FirstOrDefaultAsync();

            if (result == null) return false;
            else return true;
        }
    }
}
