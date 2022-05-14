using Microsoft.EntityFrameworkCore;
using Resaplace.Data;
using Resaplace.Data.Models;

namespace Resaplace.Services
{
    public class RestaurantService
    {
        private readonly ApplicationDbContext dbContext;

        public RestaurantService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> InsertRestaurantWithApplicationAsync(Restaurant restaurant, RestaurantApplication application)
        {
            application.ApplicationStatus = BasicStatus.Accepted;
            await dbContext.Restaurants.AddAsync(restaurant);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Restaurant>> GetAllRestaurantsAsync()
        {
            return await dbContext
                .Restaurants
                .Include(x => x.Owner)
                .Include(x => x.Dishes)
                .Include(x => x.Images)
                .ToListAsync();
        }
    }
}
