using Microsoft.EntityFrameworkCore;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Infrastructure.DAL.Repositories;

internal class DatabaseWeeklyParkingSpotRepository : IWeeklyParkingSpotRepository
{
    private readonly MySpotsDbContext _dbContext;

    public DatabaseWeeklyParkingSpotRepository(MySpotsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<WeeklyParkingSpot> GetAsync (ParkingSpotId id) 
        =>  _dbContext.WeeklyParkingSpots
            .Include(x => x.Reservations)
            .SingleOrDefaultAsync(x => x.Id == id);

    public async Task<IEnumerable<WeeklyParkingSpot>> GetAllAsync()
    {
        var weeklyParkingSpots = await _dbContext.WeeklyParkingSpots.Include(x => x.Reservations).ToListAsync();
        return weeklyParkingSpots;
    }

    public async Task<IEnumerable<WeeklyParkingSpot>> GetByWeekAsync(Week week)
    {
        return await _dbContext.WeeklyParkingSpots
            .Include(x => x.Reservations)
            .Where(x => x.Week == week)
            .ToListAsync();
    }

    public async Task AddAsync (WeeklyParkingSpot weeklyParkingSpot)
    {
        await _dbContext.AddAsync(weeklyParkingSpot);
    }

    public Task UpdateAsync (WeeklyParkingSpot weeklyParkingSpot)
    {
        _dbContext.Update(weeklyParkingSpot);
        return Task.CompletedTask;
    }

    public async Task DeleteAsync (WeeklyParkingSpot weeklyParkingSpot)
    {
        _dbContext.Remove(weeklyParkingSpot);
        await _dbContext.SaveChangesAsync();
    }
}