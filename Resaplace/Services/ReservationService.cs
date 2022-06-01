using Resaplace.Data;
using Resaplace.Data.Models;

namespace Resaplace.Services
{
    public class ReservationService
    {
        private readonly ApplicationDbContext dbContext;

        public ReservationService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> InsertReservationAsync(Reservation res)
        {
            await dbContext.Reservations.AddAsync(res);
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
