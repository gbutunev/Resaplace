﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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
            res.ReservationStatus = BasicStatus.Pending;

            await dbContext.Reservations.AddAsync(res);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<List<Reservation>> GetReservationsByUser(IdentityUser idUser)
        {
            return await dbContext
                .Reservations
                .Where(x => x.User == idUser)
                .Include(x => x.ReservationDishes)
                .Include(x => x.Restaurant)
                .ToListAsync();
        }

        public async Task<List<Reservation>> GetReservationsByRestaurantAndDate(Restaurant restaurant, DateTime date)
        {
            return await dbContext
                .Reservations
                .Where(x => x.Restaurant == restaurant)
                .Where(x => x.DateTime.Date == date.Date)
                .Include(x => x.ReservationDishes)
                .ToListAsync();
        }
        public async Task<bool> DeleteReservation(Reservation res)
        {
            dbContext.Reservations.Remove(res);
            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SetReservationStatus(Reservation res, BasicStatus status)
        {
            res.ReservationStatus = status;
            await dbContext.SaveChangesAsync();
            return true;
        }
    }
}
