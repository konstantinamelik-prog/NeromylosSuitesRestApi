using Microsoft.EntityFrameworkCore;
using NeromylosSuites.Models;

namespace NeromylosSuites.Data
{
    public partial class NeromylosSuitesMvcContext : DbContext
    {

        public NeromylosSuitesMvcContext(DbContextOptions<NeromylosSuitesMvcContext> options)
            : base(options)
        {
        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Visitor> Visitors { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<SeasonalPrice> SeasonalPrices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(255);

                entity.HasIndex(e => e.Name, "UQ_Roles_Name").IsUnique();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Username).HasMaxLength(50);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.Password).HasMaxLength(60);
                entity.Property(e => e.Firstname).HasMaxLength(50);
                entity.Property(e => e.Lastname).HasMaxLength(50);

                entity.HasOne(d => d.Role).WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.Restrict)
                    .HasConstraintName("FK_Users_RoleId");

                entity.HasIndex(e => e.Username, "UQ_Users_Username").IsUnique();
                entity.HasIndex(e => e.Email, "UQ_Users_Email").IsUnique();
                entity.HasIndex(e => e.Lastname, "IX_Users_Lastname");
                entity.HasIndex(e => e.RoleId, "IX_Users_RoleId");
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.Property(e => e.CountryCode).HasMaxLength(2);
                entity.Property(e => e.PhoneNumber).HasMaxLength(15);

                entity.HasOne(d => d.User).WithOne(p => p.Member)
                    .HasForeignKey<Member>(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Members_UserId");

                entity.HasIndex(e => e.CountryCode, "IX_Members_CountryCode");
                entity.HasIndex(e => e.PhoneNumber, "IX_Members_PhoneNumber");
                entity.HasIndex(e => e.UserId, "IX_Members_UserId").IsUnique();
            });

            modelBuilder.Entity<Visitor>(entity =>
            {
                entity.Property(e => e.Firstname).HasMaxLength(50);
                entity.Property(e => e.Lastname).HasMaxLength(50);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.PhoneNumber).HasMaxLength(50);
                entity.Property(e => e.CountryCode).HasMaxLength(2);

                entity.HasIndex(e => e.Email, "IX_Visitors_Email");
                entity.HasIndex(e => e.Lastname, "IX_Visitors_Lastname");
                entity.HasIndex(e => e.CountryCode, "IX_Visitors_CountryCode");
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.Property(e => e.CheckIn).IsRequired();
                entity.Property(e => e.CheckOut).IsRequired();
                entity.Property(e => e.TotalPrice).HasColumnType("decimal(10,2)");
                entity.Property(e => e.Status).HasMaxLength(20);
                entity.Property(e => e.SpecialRequests).HasMaxLength(255);

                entity.HasOne(d => d.User).WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_Bookings_UserId")
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(d => d.Visitor).WithMany(p => p.Bookings)
                    .HasForeignKey(d => d.VisitorId)
                    .HasConstraintName("FK_Bookings_VisitorId")
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(d => d.Rooms).WithMany(p => p.Bookings)
                    .UsingEntity("BookingsRooms");

                entity.HasIndex(e => e.UserId, "IX_Bookings_UserId");
                entity.HasIndex(e => e.VisitorId, "IX_Bookings_VisitorId");
                entity.HasIndex(e => e.CheckIn, "IX_Bookings_CheckIn");
                entity.HasIndex(e => e.CheckOut, "IX_Bookings_CheckOut");
                entity.HasIndex(e => e.Status, "IX_Bookings_Status");
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(255);
                entity.Property(e => e.Status).HasMaxLength(20);
                entity.Property(e => e.ImageUrl).HasMaxLength(255);
                entity.Property(e => e.Price).HasColumnType("decimal(10,2)");

                entity.HasIndex(e => e.RoomNumber, "IX_Room_RoomNumber").IsUnique();
                entity.HasIndex(e => e.Name, "IX_Room_Name");
                entity.HasIndex(e => e.Status, "IX_Room_Status");
            });

            modelBuilder.Entity<SeasonalPrice>(entity =>
            {
                entity.Property(e => e.SeasonName).HasMaxLength(50);
                entity.Property(e => e.DateFrom).IsRequired();
                entity.Property(e => e.DateTo).IsRequired();
                entity.Property(e => e.Price).HasColumnType("decimal(10,2)");

                entity.HasOne(d => d.Room).WithMany(p => p.SeasonalPrices)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK_SeasonalPrices_RoomId")
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(e => e.SeasonName, "IX_SeasonalPrices_SeasonName");
                entity.HasIndex(e => e.DateFrom, "IX_SeasonalPrices_DateFrom");
                entity.HasIndex(e => e.DateTo, "IX_SeasonalPrices_DateTo");
            });
        }
    }
}
