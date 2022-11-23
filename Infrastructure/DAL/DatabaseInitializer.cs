﻿using System.CodeDom;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MySpot.Core.Entities;
using MySpot.Core.ValueObjects;
using MySpot.Infrastructure.Time;

namespace MySpot.Infrastructure.DAL;

internal sealed  class DatabaseInitializer : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public DatabaseInitializer(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            //Aplikacja migracji do bazy danych
            var dbContext = scope.ServiceProvider.GetRequiredService<MySpotsDbContext>();
            dbContext.Database.Migrate();

            var clock = new Clock();
            var test = new Date(clock.Current());
            var test2 = new Date(clock.Current().Date);

            //Seedowanie danych 
            var weeklyParkingSpots = dbContext.WeeklyParkingSpots.ToList();
            if (!weeklyParkingSpots.Any())
            {
                weeklyParkingSpots = new List<WeeklyParkingSpot>()
                {
                    WeeklyParkingSpot.Create(Guid.Parse("00000000-0000-0000-0000-000000000001"), new Week(clock.Current()), "P1"),
                    WeeklyParkingSpot.Create(Guid.Parse("00000000-0000-0000-0000-000000000002"), new Week(clock.Current()), "P2"),
                    WeeklyParkingSpot.Create(Guid.Parse("00000000-0000-0000-0000-000000000003"), new Week(clock.Current()), "P3"),
                    WeeklyParkingSpot.Create(Guid.Parse("00000000-0000-0000-0000-000000000004"), new Week(clock.Current()), "P4"),
                    WeeklyParkingSpot.Create(Guid.Parse("00000000-0000-0000-0000-000000000005"), new Week(clock.Current()), "P5")
                };

                dbContext.WeeklyParkingSpots.AddRange(weeklyParkingSpots);
                dbContext.SaveChanges();
            }
        };

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}