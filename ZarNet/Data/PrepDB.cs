using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using ZarNet.Models;

namespace ZarNet.Data
{
    public static class PrepDB
    {
        public static void PrepPopulation(IApplicationBuilder app, bool isDevelopment)
        {
            using (var servicScope = app.ApplicationServices.CreateScope())
            {
                SeedData(servicScope.ServiceProvider.GetService<ApplicationDbContext>(), isDevelopment);
            }
        }
        public static void SeedData(ApplicationDbContext context, bool isDevelopment)
        {
            if(!isDevelopment)
            {
                System.Console.WriteLine("Applying migrations");
                context.Database.EnsureDeleted();
                context.Database.Migrate();
            }
         /*   context.Database.EnsureDeleted();
            context.Database.Migrate();*/
            if (!context.Post.Any())
            {
                System.Console.WriteLine("Adding data");
                Models.Category cat = new Models.Category()
                {
                    ParentId = 1,
                    Name = "Barilga"
                };
                context.Category.AddRange(
                    new Models.Category()
                    {
                        ParentId = 0,
                        Name = "Ul hodloh"
                    },
                    new Models.Category()
                    {
                        ParentId = 0,
                        Name = "Cars"
                    },
                    cat
                );
                Company comp = new Models.Company() { Name = "telmen construction" };
                context.Company.AddRange(comp);
                context.Post.AddRange(new Models.Post()
                {
                    Company = comp,
                    Category = cat,
                    Title = "ul hodloh",
                    Price = "sdlkfj"
                });
                context.SaveChanges();
            }
            else
            {
                System.Console.WriteLine("Already have data");
            }
        }
    }
}