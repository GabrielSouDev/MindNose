

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MindNose.Domain.Consts;
using MindNose.Domain.Entities;
using MindNose.Domain.Request.User;

namespace MindNose.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<ConversationGuide> ConversationGuides { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Chunk> Chunks { get; set; }

    public async Task SeedDataAsync(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager, UserRequest adminRequest)
    {
        var retries = 1;
        while (true)
        {
            try
            {
                await Database.MigrateAsync();
                break;
            }
            catch (Npgsql.NpgsqlException)
            {
                Console.WriteLine("#");
                retries++;
                if (retries > 10) throw;
                await Task.Delay(2000); // espera 3s e tenta novamente
            }
        }

        foreach (var role in Role.All)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>(role));
            }
        }

        var adminUser = await userManager.FindByEmailAsync(adminRequest.Email);

        if (adminUser == null)
        {
            adminUser = new User { UserName = adminRequest.UserName, Email = adminRequest.Email, EmailConfirmed = true };

            await userManager.CreateAsync(adminUser, adminRequest.Password);
            await userManager.AddToRoleAsync(adminUser, Role.Admin);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Chave primária explícita para UserProfile
        modelBuilder.Entity<UserProfile>()
            .HasKey(u => u.Id);

        // 1:1 User -> UserProfile
        modelBuilder.Entity<User>()
            .HasOne(u => u.UserProfile)
            .WithOne()
            .HasForeignKey<UserProfile>(p => p.Id)
            .IsRequired();

        // 1:N ConversationGuide -> UserProfile
        modelBuilder.Entity<ConversationGuide>()
            .HasOne(c => c.UserProfile)
            .WithMany(p => p.ConversationGuides)
            .HasForeignKey(c => c.UserProfileId)
            .IsRequired();

        // 1:N Message -> ConversationGuide
        modelBuilder.Entity<Message>()
            .HasOne(m => m.ConversationGuide)
            .WithMany(c => c.Messages)
            .HasForeignKey(m => m.ConversationGuideId)
            .IsRequired();

        // 1:N Chunk -> UserProfile
        modelBuilder.Entity<Chunk>()
            .HasOne(c => c.ConversationGuide)
            .WithMany(c => c.Chunks)
            .HasForeignKey(m => m.ConversationGuideId)
            .IsRequired();
    }
}
public class User : IdentityUser<Guid>
{
    public UserProfile? UserProfile { get; set; }
}