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
            return await dbContext
                .RestaurantApplications
                .Include(x => x.Owner)
                .Include(x => x.Images)
                .ToListAsync();
        }

        public async Task<List<RestaurantApplication>> GetArchivedRestaurantApplicationAsync()
        {
            return await dbContext
                .RestaurantApplications
                .Where(x => x.ApplicationStatus == BasicStatus.Accepted || x.ApplicationStatus == BasicStatus.Declined)
                .Include(x => x.Owner)
                .Include(x => x.Images)
                .ToListAsync();
        }

        public async Task<List<RestaurantApplication>> GetPendingRestaurantApplicationAsync()
        {
            return await dbContext
                .RestaurantApplications
                .Where(x => x.ApplicationStatus == BasicStatus.Pending)
                .Include(x => x.Owner)
                .Include(x => x.Images)
                .ToListAsync();
        }

        public async Task<RestaurantApplication> GetRestaurantApplicationByIdAsync(int id)
        {
            return await dbContext
                .RestaurantApplications
                .Where(a => a.Id == id)
                .Include(x => x.Owner)
                .Include(x => x.Images)
                .FirstOrDefaultAsync();
        }

        public async Task<List<RestaurantApplication>> GetRestaurantApplicationsByOwnerIdAsync(string id)
        {
            return await dbContext
                .RestaurantApplications
                .Where(x => x.Owner.Id == id)
                .Include(x => x.Owner)
                .Include(x => x.Images)
                .ToListAsync();
        }

        public async Task<List<RestaurantApplication>> GetAllRestaurantApplicationsByOwnerAsync(IdentityUser owner)
        {
            return await dbContext
                .RestaurantApplications
                .Where(x => x.Owner == owner)
                .Include(x => x.Owner)
                .Include(x => x.Images)
                .ToListAsync();
        }

        public async Task<List<RestaurantApplication>> GetPendingRestaurantApplicationsByOwnerAsync(IdentityUser owner)
        {
            return await dbContext
                .RestaurantApplications
                .Where(x => x.Owner == owner)
                .Where(x => x.ApplicationStatus == BasicStatus.Pending)
                .Include(x => x.Owner)
                .Include(x => x.Images)
                .ToListAsync();
        }

        public async Task<List<RestaurantApplication>> GetArchivedRestaurantApplicationsByOwnerAsync(IdentityUser owner)
        {
            return await dbContext
                .RestaurantApplications
                .Where(x => x.Owner == owner)
                .Where(x => x.ApplicationStatus == BasicStatus.Accepted || x.ApplicationStatus == BasicStatus.Declined)
                .Include(x => x.Owner)
                .Include(x => x.Images)
                .ToListAsync();
        }

        public async Task<bool> InsertRestaurantApplicationAsync(RestaurantApplication application)
        {
            await dbContext.RestaurantApplications.AddAsync(application);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ChangeRestaurantApplicationStatusAsync(int id, BasicStatus status)
        {
            var application = await dbContext.RestaurantApplications.FirstOrDefaultAsync(x => x.Id == id);
            application.ApplicationStatus = status;
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetFeedbackMessageAsync(int id, string message)
        {
            var application = await dbContext.RestaurantApplications.FirstOrDefaultAsync(x => x.Id == id);
            application.FeedbackMessage = message;
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
