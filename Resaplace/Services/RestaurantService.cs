using Microsoft.AspNetCore.Identity;
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

        public async Task<List<Restaurant>> GetRestaurantsByOwner(IdentityUser owner)
        {
            return await dbContext
                .Restaurants
                .Where(x => x.Owner == owner)
                .Include(x => x.Owner)
                .Include(x => x.Dishes)
                .Include(x => x.Images)
                .ToListAsync();
        }

        public async Task<Restaurant> GetRestaurantByIdAsync(int id)
        {
            return await dbContext
                .Restaurants
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateRestaurantAsync(Restaurant restaurant)
        {
            dbContext.Restaurants.Update(restaurant);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public bool DeleteRestaurant(Restaurant restaurant)
        {
            dbContext.Restaurants.Remove(restaurant);
            dbContext.SaveChanges();
            return true;
        }
    }
}
