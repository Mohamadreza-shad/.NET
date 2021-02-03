using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

    public DbSet<AppUser> Users { get; set; }
    public DbSet<LikeUser> Likes { get; set; }
    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<LikeUser>()
            .HasKey(lu => new {lu.SourceUserId,lu.LikedUserId});

        builder.Entity<LikeUser>()
            .HasOne(xx => xx.SourceUser)
            .WithMany(xx => xx.LikedUsers)
            .HasForeignKey(xx => xx.SourceUserId)
            .OnDelete(DeleteBehavior.Cascade);

        
        builder.Entity<LikeUser>()
            .HasOne(xx => xx.LikedUser)
            .WithMany(xx => xx.LikedByUsers)
            .HasForeignKey(xx => xx.LikedUserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Message>()
            .HasOne(xx => xx.Recipient)
            .WithMany(xx => xx.MessagesRecieved)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Message>()
            .HasOne(xx => xx.Sender)
            .WithMany(xx => xx.MessagesSent)
            .OnDelete(DeleteBehavior.Restrict);
    }


    }
}