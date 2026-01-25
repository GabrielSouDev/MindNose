

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MindNose.Domain.Consts;
using MindNose.Domain.Request.User;

namespace MindNose.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public async Task SeedDataAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, UserRequest adminRequest)
    {
        await Database.MigrateAsync();

        foreach (var role in Role.All)
        {
            if(!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
        
        var adminUser = await userManager.FindByEmailAsync(adminRequest.Email); 
        
        if (adminUser == null)
        {
            adminUser = new ApplicationUser { UserName = adminRequest.UserName, Email = adminRequest.Email, EmailConfirmed = true }; 

            await userManager.CreateAsync(adminUser, adminRequest.Password);
            await userManager.AddToRoleAsync(adminUser, Role.Admin); 
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

    }
}

public class ApplicationUser : IdentityUser { }