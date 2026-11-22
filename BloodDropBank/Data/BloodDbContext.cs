
using BloodDropBank.Models;
using Microsoft.EntityFrameworkCore;

namespace BloodDropBank.Data
{
    public class BloodDbContext : DbContext
    {
        public BloodDbContext(DbContextOptions<BloodDbContext> options) : base(options)
        {
        }

        // Primary Keys, Required and MaxLength
        // are already configured in Models.

        public DbSet<Admin> Admins { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Hospital> Hospitals { get; set; }
        public DbSet<BloodInventory> BloodInventories { get; set; }
        public DbSet<BloodRequest> BloodRequests { get; set; }
        public DbSet<UserDonation> UserDonations { get; set; }
        public DbSet<InventoryDonation> InventoryDonations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ================= Admin =================

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.HasIndex(a => a.Email).IsUnique();
                entity.HasIndex(a => a.PhoneNumber).IsUnique();
            });

            // ================= User =================

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(u => u.NationalId).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();
                entity.HasIndex(u => u.PhoneNumber).IsUnique();

                entity.Property(u => u.WeightKg).HasPrecision(5, 2);

                entity.ToTable(table =>
                {
                    table.HasCheckConstraint("CK_Users_BloodGroup", "[BloodGroup] BETWEEN 1 AND 8");
                });
            });

            // ================= Hospital =================

            modelBuilder.Entity<Hospital>(entity =>
            {
                entity.HasIndex(h => h.Email).IsUnique();
                entity.HasIndex(h => h.PhoneNumber).IsUnique();
            });

            // ================= BloodInventory =================

            modelBuilder.Entity<BloodInventory>(entity =>
            {
                // One blood group per hospital.
                entity.HasIndex(i => new
                {
                    i.HospitalId,
                    i.BloodGroup
                })
                .IsUnique();

                // Detect concurrent stock updates.
                //entity.Property(i => i.UnitsAvailable).IsConcurrencyToken();

                entity.ToTable(table =>
                {
                    table.HasCheckConstraint("CK_BloodInventories_UnitsAvailable", "[UnitsAvailable] BETWEEN 0 AND 10000");

                    table.HasCheckConstraint("CK_BloodInventories_BloodGroup", "[BloodGroup] BETWEEN 1 AND 8");
                });

                entity.HasOne(i => i.Hospital)
                    .WithMany(h => h.BloodInventories)
                    .HasForeignKey(i => i.HospitalId)
                    .OnDelete(DeleteBehavior.ClientNoAction);
            });

            // ================= BloodRequest =================

            modelBuilder.Entity<BloodRequest>(entity =>
            {
                //entity.Property(r => r.RemainingUnits).IsConcurrencyToken();

                //entity.Property(r => r.Status).IsConcurrencyToken();

                entity.ToTable(table =>
                {
                    table.HasCheckConstraint("CK_BloodRequests_UnitsNeeded", "[UnitsNeeded] BETWEEN 1 AND 40");

                    table.HasCheckConstraint(
                        "CK_BloodRequests_RemainingUnits",
                        "[RemainingUnits] >= 0 AND " +
                        "[RemainingUnits] <= [UnitsNeeded]");

                    table.HasCheckConstraint("CK_BloodRequests_BloodGroup", "[BloodGroup] BETWEEN 1 AND 8");

                    table.HasCheckConstraint("CK_BloodRequests_Status", "[Status] BETWEEN 1 AND 4");
                });

                entity.HasOne(r => r.User)
                    .WithMany(u => u.BloodRequests)
                    .HasForeignKey(r => r.UserId)
                    .OnDelete(DeleteBehavior.ClientNoAction);

                entity.HasOne(r => r.Hospital)
                    .WithMany(h => h.BloodRequests)
                    .HasForeignKey(r => r.HospitalId)
                    .OnDelete(DeleteBehavior.ClientNoAction);
            });

            // ================= UserDonation =================

            modelBuilder.Entity<UserDonation>(entity =>
            {
                // Find the last approved donation.
                entity.HasIndex(d => new
                {
                    d.UserId,
                    d.DonationStatus,
                    d.DonatedAt
                });

                // Protect against concurrent status updates.
                //entity.Property(d => d.DonationStatus).IsConcurrencyToken();

                entity.ToTable(table =>
                {
                    table.HasCheckConstraint("CK_UserDonations_UnitDonated", "[UnitDonated] = 1");

                    table.HasCheckConstraint("CK_UserDonations_BloodGroup", "[BloodGroup] BETWEEN 1 AND 8");

                    table.HasCheckConstraint("CK_UserDonations_UserDonationType", "[UserDonationType] BETWEEN 1 AND 2");

                    table.HasCheckConstraint("CK_UserDonations_DonationStatus", "[DonationStatus] BETWEEN 1 AND 3");

                    // ToInventory: no BloodRequestId.
                    // ForBloodRequest: BloodRequestId required.
                    table.HasCheckConstraint(
                        "CK_UserDonations_TypeAndRequest",
                        "([UserDonationType] = 1 AND " +
                        "[BloodRequestId] IS NULL) OR " +
                        "([UserDonationType] = 2 AND " +
                        "[BloodRequestId] IS NOT NULL)");

                    // DonatedAt is set only on approval.
                    table.HasCheckConstraint(
                        "CK_UserDonations_StatusAndDate",
                        "([DonationStatus] = 2 AND " +
                        "[DonatedAt] IS NOT NULL) OR " +
                        "([DonationStatus] IN (1, 3) AND " +
                        "[DonatedAt] IS NULL)");
                });

                entity.HasOne(d => d.User)
                    .WithMany(u => u.UserDonations)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientNoAction);

                entity.HasOne(d => d.Hospital)
                    .WithMany(h => h.UserDonations)
                    .HasForeignKey(d => d.HospitalId)
                    .OnDelete(DeleteBehavior.ClientNoAction);

                entity.HasOne(d => d.BloodRequest)
                    .WithMany(r => r.UserDonations)
                    .HasForeignKey(d => d.BloodRequestId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.ClientNoAction);
            });

            // ================= InventoryDonation =================

            modelBuilder.Entity<InventoryDonation>(entity =>
            {
                entity.ToTable(table =>
                {
                    table.HasCheckConstraint("CK_InventoryDonations_UnitsDonated", "[UnitsDonated] BETWEEN 1 AND 40");
                });

                entity.HasOne(d => d.BloodRequest)
                    .WithMany(r => r.InventoryDonations)
                    .HasForeignKey(d => d.BloodRequestId)
                    .OnDelete(DeleteBehavior.ClientNoAction);

                entity.HasOne(d => d.BloodInventory)
                    .WithMany(i => i.InventoryDonations)
                    .HasForeignKey(d => d.BloodInventoryId)
                    .OnDelete(DeleteBehavior.ClientNoAction);
            });
        }
    }
}
