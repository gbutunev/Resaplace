using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Resaplace.Data;
using Resaplace.Data.Models;

namespace Resaplace.Services
{
    public class RestaurantApplicationsService
    {
        private readonly ApplicationDbContext dbContext;

        public RestaurantApplicationsService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<RestaurantApplication>> GetAllRestaurantApplicationsAsync()
        {
            return await dbContext.RestaurantApplications.ToListAsync();
        }

        public async Task<RestaurantApplication> GetRestaurantApplicationByIdAsync(int id)
        {
            return await dbContext.RestaurantApplications.Where(a => a.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<RestaurantApplication>> GetRestaurantApplicationsByOwnerIdAsync(string id)
        {
            return await dbContext.RestaurantApplications.Where(x => x.Owner.Id == id).ToListAsync();
        }

        public async Task<List<RestaurantApplication>> GetRestaurantApplicationsByOwnerAsync(IdentityUser owner)
        {
            return await dbContext.RestaurantApplications.Where(x => x.Owner == owner).ToListAsync();
        }

        public async Task<bool> InsertRestaurantApplicationAsync(RestaurantApplication application)
        {
            await dbContext.RestaurantApplications.AddAsync(application);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
