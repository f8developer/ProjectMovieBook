using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProjectMovieBook.Data;

namespace ProjectMovieBook.Core.StartupLogic
{
    internal class SeedRolesAndOwner
    {
        internal static async Task SeedRolesAndOwnerAsync(IServiceScope scope)
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<ApplicationDbContext>();
                var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var configuration = services.GetRequiredService<IConfiguration>();

                // Apply pending migrations
                await context.Database.MigrateAsync();

                // Create roles if they don't exist
                string[] roles = { "User", "Administrator", "Owner" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                // Setup owner user
                var ownerUser = new IdentityUser
                {
                    UserName = "owner@example.com",
                    Email = "owner@example.com"
                };

                // Create admin user if no users exist
                if (await userManager.FindByEmailAsync(ownerUser.Email.ToString()) == null)
                {
                    var ownerPassword = configuration["OwnerPassword"] ??
                                      Environment.GetEnvironmentVariable("OwnerPassword");
                    if (string.IsNullOrEmpty(ownerPassword))
                    {
                        throw new InvalidOperationException("Owner password not configured in secrets.");
                    }
                    // Create the user with a secure password
                    var result = await userManager.CreateAsync(ownerUser, ownerPassword);
                    if (!result.Succeeded)
                    {
                        var logger = services.GetRequiredService<ILogger<SeedRolesAndOwner>>();
                        logger.LogError("Failed to create Owner user. Errors: {Errors}", string.Join(", ", result.Errors));
                        throw new Exception("Failed to create Owner user.");
                    }

                    if (result.Succeeded)
                    {
                        // Assign both roles to the admin user
                        await userManager.AddToRolesAsync(ownerUser, roles);
                    }
                    else
                    {
                        Console.WriteLine("Failed to create admin user: {Errors}", result.Errors);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while seeding the database. -> {ex}");
            }

        }
    }
}
