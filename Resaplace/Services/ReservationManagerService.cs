using Resaplace.Data;
using Resaplace.Data.Models;

namespace Resaplace.Services
{
    public class ReservationManagerService
    {
        private readonly ApplicationDbContext dbContext;

        public ReservationManagerService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<TimeOnly> GetAvailableHours(int restaurantId, DateOnly selectedDate, int numberOfPeople)
        {
            TimeOnly timeStart = new(9, 0);
            TimeOnly timeEnd = new(21, 0);

            Restaurant restaurantInfo = dbContext.Restaurants.Where(x => x.Id == restaurantId).FirstOrDefault();

            List<TimeOnly> result = new();

            for (; timeStart <= timeEnd; timeStart = timeStart.Add(TimeSpan.FromMinutes(30)))
            {
                DateTime resBegin = new(selectedDate.Year, selectedDate.Month, selectedDate.Day, timeStart.Hour, timeStart.Minute, 0);

                #region Check reservations by number of people before required time
                var timeframePeople1 = resBegin.Subtract(TimeSpan.FromMinutes(90));
                int amountPeople1 = dbContext
                    .Reservations
                    .Where(x => x.Restaurant.Id == restaurantId)
                    .Where(x => x.PeopleNumber <= 4)
                    .Where(x => x.DateTime == timeframePeople1)
                    .Sum(x => x.PeopleNumber);

                var timeframePeople2 = resBegin.Subtract(TimeSpan.FromMinutes(150));
                int amountPeople2 = dbContext
                    .Reservations
                    .Where(x => x.Restaurant.Id == restaurantId)
                    .Where(x => x.PeopleNumber > 4)
                    .Where(x => x.DateTime >= timeframePeople2 && x.DateTime <= timeframePeople1)
                    .Sum(x => x.PeopleNumber);
                #endregion

                #region Check reservations by number of people after required time
                DateTime timeframePeople3;
                if (numberOfPeople <= 4)
                    timeframePeople3 = resBegin.Add(TimeSpan.FromMinutes(90));
                else
                    timeframePeople3 = resBegin.Add(TimeSpan.FromMinutes(150));
                int amountPeople3 = dbContext
                    .Reservations
                    .Where(x => x.Restaurant.Id == restaurantId)
                    .Where(x => x.DateTime >= resBegin && x.DateTime <= timeframePeople3)
                    .Sum(x => x.PeopleNumber);
                #endregion

                #region Check reservations by table numbers before required time
                var timeframeTables1 = resBegin.Subtract(TimeSpan.FromMinutes(90));
                int amountTables1 = dbContext
                    .Reservations
                    .Where(x => x.Restaurant.Id == restaurantId)
                    .Where(x => x.PeopleNumber <= 4)
                    .Where(x => x.DateTime == timeframeTables1)
                    .Count();

                var timeframeTables2 = resBegin.Subtract(TimeSpan.FromMinutes(150));
                int amountTables2 = dbContext
                    .Reservations
                    .Where(x => x.Restaurant.Id == restaurantId)
                    .Where(x => x.PeopleNumber > 4)
                    .Where(x => x.DateTime >= timeframeTables2 && x.DateTime <= timeframeTables1)
                    .Count();
                #endregion

                #region Check reservations by table numbers after required time
                DateTime timeframeTables3;
                if (numberOfPeople <= 4)
                    timeframeTables3 = resBegin.Add(TimeSpan.FromMinutes(90));
                else
                    timeframeTables3 = resBegin.Add(TimeSpan.FromMinutes(150));
                int amountTables3 = dbContext
                    .Reservations
                    .Where(x => x.Restaurant.Id == restaurantId)
                    .Where(x => x.DateTime >= resBegin && x.DateTime <= timeframeTables3)
                    .Count();
                #endregion

                if (amountPeople1 + amountPeople2 + amountPeople3 + numberOfPeople <= restaurantInfo.TotalSeats &&
                    amountTables1 + amountTables2 + amountTables3 + 1 <= restaurantInfo.TotalTables)
                {
                    result.Add(timeStart);
                }
            }

            return result;
        }
    }
}
