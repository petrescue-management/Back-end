using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PetRescue.Data.Models
{
    public partial class PetRescueContext : DbContext
    {
        public PetRescueContext()
        {
        }

        public PetRescueContext(DbContextOptions<PetRescueContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Center> Center { get; set; }
        public virtual DbSet<CenterRegistrationForm> CenterRegistrationForm { get; set; }
        public virtual DbSet<Pet> Pet { get; set; }
        public virtual DbSet<Post> Post { get; set; }
        public virtual DbSet<RescueReport> RescueReport { get; set; }
        public virtual DbSet<RescueReportDetail> RescueReportDetail { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserProfile> UserProfile { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-SEQC2RA\\PIIMTRAN,1433;Database=PetRescue;Trusted_Connection=True;User Id=sa;Password=tranphimai");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Center>(entity =>
            {
                entity.Property(e => e.CenterId)
                    .HasColumnName("center_id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnName("address")
                    .HasMaxLength(80);

                entity.Property(e => e.CenterName)
                    .IsRequired()
                    .HasColumnName("center_name")
                    .HasMaxLength(100);

                entity.Property(e => e.CenterStatus).HasColumnName("center_status");

                entity.Property(e => e.InsertAt)
                    .HasColumnName("insert_at")
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InsertBy).HasColumnName("insert_by");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasColumnName("phone")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateAt)
                    .HasColumnName("update_at")
                    .HasColumnType("date");

                entity.Property(e => e.UpdateBy).HasColumnName("update_by");
            });

            modelBuilder.Entity<CenterRegistrationForm>(entity =>
            {
                entity.HasKey(e => e.FormId);

                entity.Property(e => e.FormId)
                    .HasColumnName("form_id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CenterAddress)
                    .IsRequired()
                    .HasColumnName("center_address")
                    .HasMaxLength(80);

                entity.Property(e => e.CenterName)
                    .IsRequired()
                    .HasColumnName("center_name")
                    .HasMaxLength(100);

                entity.Property(e => e.CenterRegisterStatus).HasColumnName("center_register_status");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasColumnName("phone")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("date");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            });

            modelBuilder.Entity<Pet>(entity =>
            {
                entity.Property(e => e.PetId)
                    .HasColumnName("pet_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Age).HasColumnName("age");

                entity.Property(e => e.CenterId).HasColumnName("center_id");

                entity.Property(e => e.Description).HasColumnName("description");

                entity.Property(e => e.InsertedAt)
                    .HasColumnName("inserted_at")
                    .HasColumnType("date");

                entity.Property(e => e.InsertedBy).HasColumnName("inserted_by");

                entity.Property(e => e.PetName)
                    .IsRequired()
                    .HasColumnName("pet_name")
                    .HasMaxLength(50);

                entity.Property(e => e.PetStatus)
                    .IsRequired()
                    .HasColumnName("pet_status")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("date");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

                entity.HasOne(d => d.PetNavigation)
                    .WithOne(p => p.Pet)
                    .HasForeignKey<Pet>(d => d.PetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Pet_Post");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.HasKey(e => e.PetId);

                entity.Property(e => e.PetId)
                    .HasColumnName("pet_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.InsertedAt)
                    .HasColumnName("inserted_at")
                    .HasColumnType("date");

                entity.Property(e => e.InsertedBy).HasColumnName("inserted_by");

                entity.Property(e => e.PostContent)
                    .IsRequired()
                    .HasColumnName("post_content");

                entity.Property(e => e.PostStatus)
                    .IsRequired()
                    .HasColumnName("post_status")
                    .HasMaxLength(4)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("date");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            });

            modelBuilder.Entity<RescueReport>(entity =>
            {
                entity.Property(e => e.RescueReportId)
                    .HasColumnName("rescue_report_id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.InsertedAt)
                    .HasColumnName("inserted_at")
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InsertedBy).HasColumnName("inserted_by");

                entity.Property(e => e.PetAttribute).HasColumnName("pet_attribute");

                entity.Property(e => e.ReportStatus).HasColumnName("report_status");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("date");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            });

            modelBuilder.Entity<RescueReportDetail>(entity =>
            {
                entity.HasKey(e => e.RescueReportId);

                entity.Property(e => e.RescueReportId)
                    .HasColumnName("rescue_report_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.ImgReportUrl)
                    .IsRequired()
                    .HasColumnName("img_report_url")
                    .HasMaxLength(50);

                entity.Property(e => e.ReportDescription).HasColumnName("report_description");

                entity.Property(e => e.ReportLocation)
                    .IsRequired()
                    .HasColumnName("report_location")
                    .HasMaxLength(80);

                entity.HasOne(d => d.RescueReport)
                    .WithOne(p => p.RescueReportDetail)
                    .HasForeignKey<RescueReportDetail>(d => d.RescueReportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RescueReportDetail_RescueReport");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleId)
                    .HasColumnName("role_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasColumnName("role_name")
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CenterId).HasColumnName("center_id");

                entity.Property(e => e.IsActived).HasColumnName("isActived");

                entity.Property(e => e.UserEmail)
                    .IsRequired()
                    .HasColumnName("user_email")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnName("address")
                    .HasMaxLength(80);

                entity.Property(e => e.Dob)
                    .HasColumnName("dob")
                    .HasColumnType("date");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasMaxLength(50);

                entity.Property(e => e.Gender).HasColumnName("gender");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasColumnName("phone")
                    .HasMaxLength(15)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => new { e.RoleId, e.UserId });

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.InsertedAt)
                    .HasColumnName("inserted_at")
                    .HasColumnType("date");

                entity.Property(e => e.InsertedBy).HasColumnName("inserted_by");

                entity.Property(e => e.IsActived).HasColumnName("is_actived");

                entity.Property(e => e.UpdateAt)
                    .HasColumnName("update_at")
                    .HasColumnType("date");

                entity.Property(e => e.UpdateBy).HasColumnName("update_by");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRole)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRole_Role");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRole)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserRole_User");
            });
        }
    }
}
