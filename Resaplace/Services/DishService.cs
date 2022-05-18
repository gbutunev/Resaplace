using Microsoft.EntityFrameworkCore;
using Resaplace.Data;
using Resaplace.Data.Models;

namespace Resaplace.Services
{
    public class DishService
    {
        private readonly ApplicationDbContext dbContext;

        public DishService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> InsertDishAsync(Dish dish)
        {
            await dbContext.Dishes.AddAsync(dish);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Dish>> GetDishesByRestaurantIdAsync(int restaurantId)
        {
            return await dbContext
                .Dishes
                .Where(x => x.Restaurant.Id == restaurantId)
                .Include(x =>x.Image)
                .ToListAsync();
        }
    }
}
