using ChairsBackend.Entities;
using ChairsBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace ChairsBackend.DAL
{
    public class Seed
    {
        public static async void EnsurePopulated(IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetService<GameContext>();
            context.Database.Migrate();

            if (context.Games.Count() == 0)
            {
                context.Games.Add(new Game());
            }

            await context.SaveChangesAsync();
        }

    }

}
