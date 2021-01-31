using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
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
            if (!isDevelopment)
            {
                // System.Console.WriteLine("Applying migrations");
                // context.Database.EnsureDeleted();
                // context.Database.Migrate();
            }
            // context.Database.EnsureDeleted();
            // context.Database.Migrate();
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

        public static void CreateRoles(IServiceProvider serviceProvider)
        {
            //initializing custom roles 
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            string[] roleNames = { "Admin", "Client" };
            IdentityResult roleResult;

            foreach (var roleName in roleNames)
            {
                var roleExist = RoleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult();
                // ensure that the role does not exist
                if (!roleExist)
                {
                    //create the roles and seed them to the database: 
                    roleResult = RoleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
                }
            }

            // find the user with the admin email 
            var _user = UserManager.FindByEmailAsync("admin@email.com").GetAwaiter().GetResult();

            // check if the user exists
            if (_user == null)
            {
                //Here you could create the super admin who will maintain the web app
                var poweruser = new IdentityUser
                {
                    UserName = "Admin",
                    Email = "admin@email.com",
                    EmailConfirmed = true,
                };
                string adminPassword = "Pass1234!";

                var createPowerUser = UserManager.CreateAsync(poweruser, adminPassword).GetAwaiter().GetResult();
                if (createPowerUser.Succeeded)
                {
                    //here we tie the new user to the role
                    UserManager.AddToRoleAsync(poweruser, "Admin").GetAwaiter().GetResult();

                }
            }
            return;
        }


    }
}