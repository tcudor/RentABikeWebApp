using Microsoft.AspNetCore.Identity;
using RentABikeWebApp.Models;
using System;

namespace RentABikeWebApp.Data
{
    public class DbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                context.Database.EnsureCreated();
                if (context.Bikes.Any())
                {
                    var bikesToAdd = new List<Bike>
            {
                new Bike { Type = BikeType.Mountain, PricePerHour = 15, Status = "Available", Image = null },
                new Bike { Type = BikeType.Simple, PricePerHour = 12, Status = "Available", Image = null },
                new Bike { Type = BikeType.Double, PricePerHour = 10, Status = "Available", Image = null },
                new Bike { Type = BikeType.Hybrid, PricePerHour = 20, Status = "Available", Image = null },
                new Bike { Type = BikeType.Simple, PricePerHour = 8, Status = "Available", Image = null }
            };
                    context.Bikes.AddRange(bikesToAdd);
                    context.SaveChanges();
                }

                if (context.Reservations.Any())
                {
                    var reservationsToAdd = new List<Reservation>
            {
                new Reservation { StartDate = DateTime.Now.AddDays(1), EndDate = DateTime.Now.AddDays(3), TotalCost = 45, BikeId = 1, CustomerId = 1 },
                new Reservation { StartDate = DateTime.Now.AddDays(2), EndDate = DateTime.Now.AddDays(4), TotalCost = 36, BikeId = 2, CustomerId = 2 },
                new Reservation { StartDate = DateTime.Now.AddDays(3), EndDate = DateTime.Now.AddDays(5), TotalCost = 60, BikeId = 3, CustomerId = 3 }
            };
                    context.Reservations.AddRange(reservationsToAdd);
                    context.SaveChanges();
                }

                if (context.Customers.Any())
                {
                    var customersToAdd = new List<Customer>
            {
                new Customer { Name = "John Doe", Email = "john@example.com", Phone = "123456789", IdCode = "ABC123", IdSeries = "XY" },
                new Customer { Name = "Jane Smith", Email = "jane@example.com", Phone = "987654321", IdCode = "DEF456", IdSeries = "ZW" },
                new Customer { Name = "Bob Johnson", Email = "bob@example.com", Phone = "555123456", IdCode = "GHI789", IdSeries = "UV" }

            };
                    context.Customers.AddRange(customersToAdd);
                    context.SaveChanges();


                }
            }
        }

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {

                //Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync("Admin"))
                    await roleManager.CreateAsync(new IdentityRole("Admin"));

                //Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                string adminUserEmail = "admin@gmail.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new IdentityUser()
                    {
                        UserName = "admin",
                        Email = adminUserEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAdminUser, "Test12%");
                    await userManager.AddToRoleAsync(newAdminUser, "Admin");
                }
            }
        }
    }
}
