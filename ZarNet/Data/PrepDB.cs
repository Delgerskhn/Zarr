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

            if (!context.Post.Any())
            {
                System.Console.WriteLine("Adding data");
                Models.Category cat = new Models.Category()
                {
                    CategoryId = 2,
                    ParentId = 1,
                    Name = "Barilga"
                };
                context.Category.AddRange(
                    new Models.Category()
                    {
                        CategoryId = 1,
                        ParentId = 0,
                        Name = "Ul hodloh"
                    },
                    cat
                );
                Company comp = new Models.Company() { Name = "telmen construction" };
                context.Company.AddRange(comp);
                context.Post.AddRange(new Models.Post()
                {
                    Company = comp,
                    Category = cat,
                    Name = "ul hodloh",
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