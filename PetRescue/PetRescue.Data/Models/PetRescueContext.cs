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

        public virtual DbSet<Adoption> Adoption { get; set; }
        public virtual DbSet<AdoptionRegisterForm> AdoptionRegisterForm { get; set; }
        public virtual DbSet<Center> Center { get; set; }
        public virtual DbSet<CenterRegistrationForm> CenterRegistrationForm { get; set; }
        public virtual DbSet<Pet> Pet { get; set; }
        public virtual DbSet<PetBreed> PetBreed { get; set; }
        public virtual DbSet<PetFurColor> PetFurColor { get; set; }
        public virtual DbSet<PetProfile> PetProfile { get; set; }
        public virtual DbSet<PetType> PetType { get; set; }
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
                optionsBuilder.UseSqlServer("Server=petrescueserver.database.windows.net;Database=PetRescue;Trusted_Connection=False;Encrypt=True;User Id=petrescue;Password=Admin123");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Adoption>(entity =>
            {
                entity.HasKey(e => e.AdoptionRegisterId)
                    .HasName("PK_Adotion");

                entity.Property(e => e.AdoptionRegisterId)
                    .HasColumnName("adoption_register_id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.AdoptedAt)
                    .HasColumnName("adopted_at")
                    .HasColumnType("date");

                entity.Property(e => e.AdoptionStatus).HasColumnName("adoption_status");

                entity.Property(e => e.InsertedAt)
                    .HasColumnName("inserted_at")
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InsertedBy).HasColumnName("inserted_by");

                entity.Property(e => e.ReturnedAt)
                    .HasColumnName("returned_at")
                    .HasColumnType("date");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("date");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

                entity.HasOne(d => d.AdoptionRegister)
                    .WithOne(p => p.Adoption)
                    .HasForeignKey<Adoption>(d => d.AdoptionRegisterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Adotion_AdoptionRegisterForm");
            });

            modelBuilder.Entity<AdoptionRegisterForm>(entity =>
            {
                entity.HasKey(e => e.AdoptionRegisterId);

                entity.Property(e => e.AdoptionRegisterId)
                    .HasColumnName("adoption_register_id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnName("address");

                entity.Property(e => e.AdoptionRegisterStatus).HasColumnName("adoption_register_status");

                entity.Property(e => e.BeViolentTendencies).HasColumnName("be_violent_tendencies");

                entity.Property(e => e.ChildAge).HasColumnName("child_age");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .IsUnicode(false);

                entity.Property(e => e.FrequencyAtHome).HasColumnName("frequency_at_home");

                entity.Property(e => e.HaveAgreement).HasColumnName("have_agreement");

                entity.Property(e => e.HaveChildren).HasColumnName("have_children");

                entity.Property(e => e.HavePet).HasColumnName("have_pet");

                entity.Property(e => e.HouseType).HasColumnName("house_type");

                entity.Property(e => e.InsertedAt)
                    .HasColumnName("inserted_at")
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InsertedBy).HasColumnName("inserted_by");

                entity.Property(e => e.Job)
                    .IsRequired()
                    .HasColumnName("job")
                    .HasMaxLength(50);

                entity.Property(e => e.PetId).HasColumnName("pet_id");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasColumnName("phone")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateAt)
                    .HasColumnName("update_at")
                    .HasColumnType("date");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("user_name")
                    .HasMaxLength(50);

                entity.HasOne(d => d.Pet)
                    .WithMany(p => p.AdoptionRegisterForm)
                    .HasForeignKey(d => d.PetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AdoptionRegisterForm_Pet");
            });

            modelBuilder.Entity<Center>(entity =>
            {
                entity.Property(e => e.CenterId)
                    .HasColumnName("center_id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnName("address")
                    .HasMaxLength(150);

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
                entity.HasKey(e => e.CenterRegistrationId);

                entity.Property(e => e.CenterRegistrationId)
                    .HasColumnName("center_registration_id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CenterAddress)
                    .IsRequired()
                    .HasColumnName("center_address")
                    .HasMaxLength(150);

                entity.Property(e => e.CenterName)
                    .IsRequired()
                    .HasColumnName("center_name")
                    .HasMaxLength(100);

                entity.Property(e => e.CenterRegistrationStatus).HasColumnName("center_registration_status");

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
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CenterId).HasColumnName("center_id");

                entity.Property(e => e.InsertedAt)
                    .HasColumnName("inserted_at")
                    .HasColumnType("date")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InsertedBy).HasColumnName("inserted_by");

                entity.Property(e => e.PetStatus).HasColumnName("pet_status");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("date");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

                entity.HasOne(d => d.Center)
                    .WithMany(p => p.Pet)
                    .HasForeignKey(d => d.CenterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Pet_Center");

                entity.HasOne(d => d.PetNavigation)
                    .WithOne(p => p.Pet)
                    .HasForeignKey<Pet>(d => d.PetId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Pet_PetProfile");
            });

            modelBuilder.Entity<PetBreed>(entity =>
            {
                entity.Property(e => e.PetBreedId)
                    .HasColumnName("pet_breed_id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.PetBreedName)
                    .IsRequired()
                    .HasColumnName("pet_breed_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PetTypeId).HasColumnName("pet_type_id");

                entity.HasOne(d => d.PetType)
                    .WithMany(p => p.PetBreed)
                    .HasForeignKey(d => d.PetTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PetBreed_PetType");
            });

            modelBuilder.Entity<PetFurColor>(entity =>
            {
                entity.Property(e => e.PetFurColorId)
                    .HasColumnName("pet_fur_color_id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.PetFurColorName)
                    .IsRequired()
                    .HasColumnName("pet_fur_color_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PetProfile>(entity =>
            {
                entity.HasKey(e => e.PetId);

                entity.Property(e => e.PetId)
                    .HasColumnName("pet_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .IsUnicode(false);

                entity.Property(e => e.ImageUrl)
                    .HasColumnName("image_url")
                    .IsUnicode(false);

                entity.Property(e => e.IsSterilized).HasColumnName("is_sterilized");

                entity.Property(e => e.IsVaccinated).HasColumnName("is_vaccinated");

                entity.Property(e => e.PetAge).HasColumnName("pet_age");

                entity.Property(e => e.PetBreedId).HasColumnName("pet_breed_id");

                entity.Property(e => e.PetFurColorId).HasColumnName("pet_fur_color_id");

                entity.Property(e => e.PetGender).HasColumnName("pet_gender");

                entity.Property(e => e.PetName)
                    .IsRequired()
                    .HasColumnName("pet_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Weight).HasColumnName("weight");

                entity.HasOne(d => d.PetBreed)
                    .WithMany(p => p.PetProfile)
                    .HasForeignKey(d => d.PetBreedId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PetProfile_PetBreed1");

                entity.HasOne(d => d.PetFurColor)
                    .WithMany(p => p.PetProfile)
                    .HasForeignKey(d => d.PetFurColorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PetProfile_PetFurColor1");
            });

            modelBuilder.Entity<PetType>(entity =>
            {
                entity.Property(e => e.PetTypeId)
                    .HasColumnName("pet_type_id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.PetTypeName)
                    .IsRequired()
                    .HasColumnName("pet_type_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);
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
                    .IsUnicode(false);

                entity.Property(e => e.ReportDescription).HasColumnName("report_description");

                entity.Property(e => e.ReportLocation)
                    .IsRequired()
                    .HasColumnName("report_location")
                    .HasMaxLength(150);

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

                entity.Property(e => e.IsBelongToCenter)
                    .HasColumnName("is_belong_to_center")
                    .HasDefaultValueSql("((1))");

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
                    .HasMaxLength(150);

                entity.Property(e => e.Dob)
                    .HasColumnName("dob")
                    .HasColumnType("date");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasMaxLength(50);

                entity.Property(e => e.Gender).HasColumnName("gender");

                entity.Property(e => e.ImageUrl)
                    .HasColumnName("image_url")
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasColumnName("phone")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.HasOne(d => d.User)
                    .WithOne(p => p.UserProfile)
                    .HasForeignKey<UserProfile>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserProfile_User");
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
