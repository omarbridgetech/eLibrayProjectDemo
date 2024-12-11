using eLibrayProjectDemo.Models;
using Microsoft.AspNetCore.Identity;

namespace eLibrayProjectDemo.Services
{
    public class DataBaseInitializer
    {
        public static async Task SeedDataAsync(UserManager<ApplicationUser>? userManger,RoleManager<IdentityRole>? roleManager)
        {
            if(userManger == null || roleManager==null)
            {
                Console.WriteLine("userManager or roleManager is null=>exit");
                return;
            }
            //check if we have the admin role or not

            var exists = await roleManager.RoleExistsAsync("admin");
            if (!exists) {
                Console.WriteLine("Admin Role is not /defied and will be created");
                await roleManager.CreateAsync(new IdentityRole("admin"));
            
            }

            exists = await roleManager.RoleExistsAsync("seller");
            if (!exists)
            {
                Console.WriteLine("seller Role is not /defied and will be created");
                await roleManager.CreateAsync(new IdentityRole("seller"));

            }

            exists = await roleManager.RoleExistsAsync("client");
            if (!exists)
            {
                Console.WriteLine("client Role is not /defied and will be created");
                await roleManager.CreateAsync(new IdentityRole("client"));

            }

            //create the admin users

            var adminUsers=await userManger.GetUsersInRoleAsync("admin");
            if (adminUsers.Any()) {
                //admin user already exists
                Console.WriteLine("Admin user alreday exist =>exit");
                return;
            }

            //create the Admin user

            var user = new ApplicationUser()
            {
                FirstName = "Admin",
                LastName="Admin",
                UserName="admin@admin.com",
                Email= "admin@admin.com",
                CreateAt= DateTime.Now,
            };

            string initialPassword = "Admin123456@";

            var result =await userManger.CreateAsync(user,initialPassword);
            //if the account has been created 
            if (result.Succeeded) { 
                //set the user role
                await userManger.AddToRoleAsync(user,"admin");
                Console.WriteLine("Admin User created successfully! please update the initial Password");
                Console.WriteLine("Email"+user.Email);
                Console.WriteLine("initial Password: " + initialPassword);
            }
        }
    }
}
