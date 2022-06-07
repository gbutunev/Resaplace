using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Resaplace.Data;
using Resaplace.Data.Models;

namespace Resaplace.Services
{
    public class RestaurantStaffService
    {
        private readonly ApplicationDbContext dbContext;
        private readonly UserManager<IdentityUser> userManager;

        public RestaurantStaffService(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public async Task<List<Restaurant>> GetRestaurantsByStaffMemberAsync(IdentityUser user)
        {
            return await dbContext
                .RestaurantStaff
                .Where(x => x.Person == user)
                .Select(x => x.Restaurant)
                .ToListAsync();
        }

        public async Task<bool> AddStaffMember(IdentityUser user, Restaurant restaurant)
        {
            int userInXRestaurants = await dbContext.RestaurantStaff.Where(x => x.Person == user).CountAsync();

            if (userInXRestaurants == 0) //user is not in a role
            {
                await userManager.AddToRoleAsync(user, "Staff");
            }
            else //user is already a staff member
            {
                //check if they are already added in the same restaurant
                int userInThisRestaurant = await dbContext.RestaurantStaff.Where(x => x.Person == user && x.Restaurant == restaurant).CountAsync();
                if (userInThisRestaurant != 0) return false;
            }

            await dbContext.RestaurantStaff.AddAsync(new RestaurantStaff
            {
                Person = user,
                Restaurant = restaurant
            });
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveStaffMemberAsync(IdentityUser user, Restaurant restaurant)
        {
            var row = await dbContext.RestaurantStaff.Where(x => x.Person == user && x.Restaurant == restaurant).FirstOrDefaultAsync();

            dbContext.RestaurantStaff.Remove(row);

            await dbContext.SaveChangesAsync();

            int check = await dbContext.RestaurantStaff.Where(x => x.Person == user).CountAsync();

            if (check == 0) await userManager.RemoveFromRoleAsync(user, "Staff");

            return true;
        }

        public async Task<List<IdentityUser>> GetStaffMembersByRestaurantAsync(Restaurant restaurant)
        {
            return await dbContext
                .RestaurantStaff
                .Where(x => x.Restaurant == restaurant)
                .Select(x => x.Person)
                .ToListAsync();
        }

        public async Task<bool> ContainsStaffMember(IdentityUser user, Restaurant restaurant)
        {
            var row = await dbContext.RestaurantStaff.Where(x => x.Person == user && x.Restaurant == restaurant).FirstOrDefaultAsync();
            
            if (row == null) return false;
            else return true;
        }
    }
}
