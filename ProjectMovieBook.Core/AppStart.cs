using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using ProjectMovieBook.Core.StartupLogic;

namespace ProjectMovieBook.Core
{
    public class AppStart
    {
        public static async Task InitializeAsync(WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                await SeedRolesAndOwner.SeedRolesAndOwnerAsync(scope);
            }
        }
    }
}
