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
        public virtual DbSet<AdoptionRegistrationForm> AdoptionRegistrationForm { get; set; }
        public virtual DbSet<AdoptionReportTracking> AdoptionReportTracking { get; set; }
        public virtual DbSet<Center> Center { get; set; }
        public virtual DbSet<CenterRegistrationForm> CenterRegistrationForm { get; set; }
        public virtual DbSet<FinderForm> FinderForm { get; set; }
        public virtual DbSet<NotificationToken> NotificationToken { get; set; }
        public virtual DbSet<PetBreed> PetBreed { get; set; }
        public virtual DbSet<PetFurColor> PetFurColor { get; set; }
        public virtual DbSet<PetProfile> PetProfile { get; set; }
        public virtual DbSet<PetTracking> PetTracking { get; set; }
        public virtual DbSet<PetType> PetType { get; set; }
        public virtual DbSet<PickerForm> PickerForm { get; set; }
        public virtual DbSet<RescueDocument> RescueDocument { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserProfile> UserProfile { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
        public virtual DbSet<VolunteerRegistrationForm> VolunteerRegistrationForm { get; set; }
        public virtual DbSet<WorkingHistory> WorkingHistory { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=petrescue.database.windows.net;Database=PetRescue;Trusted_Connection=False;Encrypt=True;User Id=petrescue;Password=Admin123");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Adoption>(entity =>
            {
                entity.HasKey(e => e.AdoptionRegistrationId)
                    .HasName("PK_Adotion");

                entity.Property(e => e.AdoptionRegistrationId)
                    .HasColumnName("adoption_registration_id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.AdoptionStatus).HasColumnName("adoption_status");

                entity.Property(e => e.InsertedAt)
                    .HasColumnName("inserted_at")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InsertedBy).HasColumnName("inserted_by");

                entity.Property(e => e.ReturnedReason)
                    .HasColumnName("returned_reason")
                    .HasMaxLength(200);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

                entity.HasOne(d => d.AdoptionRegistration)
                    .WithOne(p => p.Adoption)
                    .HasForeignKey<Adoption>(d => d.AdoptionRegistrationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Adotion_AdoptionRegisterForm");
            });

            modelBuilder.Entity<AdoptionRegistrationForm>(entity =>
            {
                entity.HasKey(e => e.AdoptionRegistrationId)
                    .HasName("PK_AdoptionRegisterForm");

                entity.Property(e => e.AdoptionRegistrationId)
                    .HasColumnName("adoption_registration_id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnName("address")
                    .HasMaxLength(200);

                entity.Property(e => e.AdoptionRegistrationStatus).HasColumnName("adoption_registration_status");

                entity.Property(e => e.BeViolentTendencies).HasColumnName("be_violent_tendencies");

                entity.Property(e => e.CanceledRejectedReason)
                    .HasColumnName("canceled_rejected_reason")
                    .HasMaxLength(200);

                entity.Property(e => e.ChildAge).HasColumnName("child_age");

                entity.Property(e => e.Dob)
                    .HasColumnName("dob")
                    .HasColumnType("date");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.FrequencyAtHome).HasColumnName("frequency_at_home");

                entity.Property(e => e.HaveAgreement).HasColumnName("have_agreement");

                entity.Property(e => e.HaveChildren).HasColumnName("have_children");

                entity.Property(e => e.HavePet).HasColumnName("have_pet");

                entity.Property(e => e.HouseType).HasColumnName("house_type");

                entity.Property(e => e.InsertedAt)
                    .HasColumnName("inserted_at")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InsertedBy).HasColumnName("inserted_by");

                entity.Property(e => e.Job)
                    .IsRequired()
                    .HasColumnName("job")
                    .HasMaxLength(50);

                entity.Property(e => e.PetProfileId).HasColumnName("pet_profile_id");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasColumnName("phone")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("user_name")
                    .HasMaxLength(70);

                entity.HasOne(d => d.PetProfile)
                    .WithMany(p => p.AdoptionRegistrationForm)
                    .HasForeignKey(d => d.PetProfileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AdoptionRegistrationForm_PetProfile");
            });

            modelBuilder.Entity<AdoptionReportTracking>(entity =>
            {
                entity.Property(e => e.AdoptionReportTrackingId)
                    .HasColumnName("adoption_report_tracking_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.AdoptionReportTrackingImgUrl)
                    .HasColumnName("adoption_report_tracking_img_url")
                    .IsUnicode(false);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(1000);

                entity.Property(e => e.InsertedAt)
                    .HasColumnName("inserted_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.InsertedBy).HasColumnName("inserted_by");

                entity.Property(e => e.PetProfileId).HasColumnName("pet_profile_id");

                entity.HasOne(d => d.PetProfile)
                    .WithMany(p => p.AdoptionReportTracking)
                    .HasForeignKey(d => d.PetProfileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_AdoptionReportTracking_PetProfile");
            });

            modelBuilder.Entity<Center>(entity =>
            {
                entity.Property(e => e.CenterId)
                    .HasColumnName("center_id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasColumnName("address")
                    .HasMaxLength(200);

                entity.Property(e => e.CenterImgUrl)
                    .HasColumnName("center_img_url")
                    .IsUnicode(false);

                entity.Property(e => e.CenterName)
                    .IsRequired()
                    .HasColumnName("center_name")
                    .HasMaxLength(100);

                entity.Property(e => e.CenterStatus).HasColumnName("center_status");

                entity.Property(e => e.InsertedAt)
                    .HasColumnName("inserted_at")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Lat).HasColumnName("lat");

                entity.Property(e => e.Lng).HasColumnName("lng");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasColumnName("phone")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

                entity.HasOne(d => d.CenterNavigation)
                    .WithOne(p => p.Center)
                    .HasForeignKey<Center>(d => d.CenterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Center_CenterRegistrationForm");
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
                    .HasMaxLength(200);

                entity.Property(e => e.CenterName)
                    .IsRequired()
                    .HasColumnName("center_name")
                    .HasMaxLength(100);

                entity.Property(e => e.CenterRegistrationStatus).HasColumnName("center_registration_status");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(1000);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.ImageUrl)
                    .HasColumnName("image_url")
                    .IsUnicode(false);

                entity.Property(e => e.InsertedAt)
                    .HasColumnName("inserted_at")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Lat).HasColumnName("lat");

                entity.Property(e => e.Lng).HasColumnName("lng");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasColumnName("phone")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.RejectedReason)
                    .HasColumnName("rejected_reason")
                    .HasMaxLength(200);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<FinderForm>(entity =>
            {
                entity.Property(e => e.FinderFormId)
                    .HasColumnName("finder_form_id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.CanceledReason)
                    .HasColumnName("canceled_reason")
                    .HasMaxLength(200);

                entity.Property(e => e.FinderDescription)
                    .HasColumnName("finder_description")
                    .HasMaxLength(1000);

                entity.Property(e => e.FinderFormImgUrl)
                    .IsRequired()
                    .HasColumnName("finder_form_img_url")
                    .IsUnicode(false);

                entity.Property(e => e.FinderFormStatus).HasColumnName("finder_form_status");

                entity.Property(e => e.FinderFormVidUrl)
                    .HasColumnName("finder_form_vid_url")
                    .IsUnicode(false);

                entity.Property(e => e.InsertedAt)
                    .HasColumnName("inserted_at")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InsertedBy).HasColumnName("inserted_by");

                entity.Property(e => e.Lat).HasColumnName("lat");

                entity.Property(e => e.Lng).HasColumnName("lng");

                entity.Property(e => e.PetAttribute).HasColumnName("pet_attribute");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasColumnName("phone")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");
            });

            modelBuilder.Entity<NotificationToken>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .ValueGeneratedNever();

                entity.Property(e => e.ApplicationName)
                    .IsRequired()
                    .HasColumnName("application_name")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DeviceToken)
                    .HasColumnName("device_token")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.NotificationToken)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NotificationToken_User");
            });

            modelBuilder.Entity<PetBreed>(entity =>
            {
                entity.Property(e => e.PetBreedId)
                    .HasColumnName("pet_breed_id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.PetBreedName)
                    .IsRequired()
                    .HasColumnName("pet_breed_name")
                    .HasMaxLength(50);

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
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PetProfile>(entity =>
            {
                entity.Property(e => e.PetProfileId)
                    .HasColumnName("pet_profile_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CenterId).HasColumnName("center_id");

                entity.Property(e => e.InsertedAt)
                    .HasColumnName("inserted_at")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InsertedBy).HasColumnName("inserted_by");

                entity.Property(e => e.PetAge).HasColumnName("pet_age");

                entity.Property(e => e.PetBreedId).HasColumnName("pet_breed_id");

                entity.Property(e => e.PetFurColorId).HasColumnName("pet_fur_color_id");

                entity.Property(e => e.PetGender).HasColumnName("pet_gender");

                entity.Property(e => e.PetImgUrl)
                    .IsRequired()
                    .HasColumnName("pet_img_url")
                    .IsUnicode(false);

                entity.Property(e => e.PetName)
                    .IsRequired()
                    .HasColumnName("pet_name")
                    .HasMaxLength(50);

                entity.Property(e => e.PetProfileDescription)
                    .IsRequired()
                    .HasColumnName("pet_profile_description")
                    .HasMaxLength(1000);

                entity.Property(e => e.PetStatus).HasColumnName("pet_status");

                entity.Property(e => e.RescueDocumentId).HasColumnName("rescue_document_id");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdatedBy).HasColumnName("updated_by");

                entity.HasOne(d => d.Center)
                    .WithMany(p => p.PetProfile)
                    .HasForeignKey(d => d.CenterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PetProfile_Center");

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

                entity.HasOne(d => d.RescueDocument)
                    .WithMany(p => p.PetProfile)
                    .HasForeignKey(d => d.RescueDocumentId)
                    .HasConstraintName("FK_PetProfile_PetDocument");
            });

            modelBuilder.Entity<PetTracking>(entity =>
            {
                entity.Property(e => e.PetTrackingId)
                    .HasColumnName("pet_tracking_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasColumnName("description")
                    .HasMaxLength(1000);

                entity.Property(e => e.InsertedAt)
                    .HasColumnName("inserted_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.InsertedBy).HasColumnName("inserted_by");

                entity.Property(e => e.IsSterilized).HasColumnName("is_sterilized");

                entity.Property(e => e.IsVaccinated).HasColumnName("is_vaccinated");

                entity.Property(e => e.PetProfileId).HasColumnName("pet_profile_id");

                entity.Property(e => e.PetTrackingImgUrl)
                    .HasColumnName("pet_tracking_img_url")
                    .IsUnicode(false);

                entity.Property(e => e.Weight).HasColumnName("weight");

                entity.HasOne(d => d.PetProfile)
                    .WithMany(p => p.PetTracking)
                    .HasForeignKey(d => d.PetProfileId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PetTracking_PetProfile");
            });

            modelBuilder.Entity<PetType>(entity =>
            {
                entity.Property(e => e.PetTypeId)
                    .HasColumnName("pet_type_id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.PetTypeName)
                    .IsRequired()
                    .HasColumnName("pet_type_name")
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PickerForm>(entity =>
            {
                entity.Property(e => e.PickerFormId)
                    .HasColumnName("picker_form_id")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.InsertedAt)
                    .HasColumnName("inserted_at")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.InsertedBy).HasColumnName("inserted_by");

                entity.Property(e => e.PickerDescription)
                    .HasColumnName("picker_description")
                    .HasMaxLength(1000);

                entity.Property(e => e.PickerImageUrl)
                    .IsRequired()
                    .HasColumnName("picker_image_url")
                    .IsUnicode(false);
            });

            modelBuilder.Entity<RescueDocument>(entity =>
            {
                entity.HasIndex(e => e.FinderFormId)
                    .HasName("IX_PetDocument_1")
                    .IsUnique();

                entity.HasIndex(e => e.PickerFormId)
                    .HasName("IX_PetDocument_2")
                    .IsUnique();

                entity.Property(e => e.RescueDocumentId)
                    .HasColumnName("rescue_document_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CenterId).HasColumnName("center_id");

                entity.Property(e => e.FinderFormId).HasColumnName("finder_form_id");

                entity.Property(e => e.PickerFormId).HasColumnName("picker_form_id");

                entity.Property(e => e.RescueDocumentStatus).HasColumnName("rescue_document_status");

                entity.HasOne(d => d.FinderForm)
                    .WithOne(p => p.RescueDocument)
                    .HasForeignKey<RescueDocument>(d => d.FinderFormId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PetDocument_FinderForm");

                entity.HasOne(d => d.PickerForm)
                    .WithOne(p => p.RescueDocument)
                    .HasForeignKey<RescueDocument>(d => d.PickerFormId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PetDocument_PickerForm");
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

                entity.Property(e => e.IsBelongToCenter)
                    .HasColumnName("is_belong_to_center")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Password)
                    .HasColumnName("password")
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserEmail)
                    .IsRequired()
                    .HasColumnName("user_email")
                    .HasMaxLength(150)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<UserProfile>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .ValueGeneratedNever();

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

                entity.Property(e => e.InsertedAt)
                    .HasColumnName("inserted_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasColumnName("phone")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("datetime");

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
                    .HasColumnType("datetime");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("datetime");

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

            modelBuilder.Entity<VolunteerRegistrationForm>(entity =>
            {
                entity.Property(e => e.VolunteerRegistrationFormId)
                    .HasColumnName("volunteer_registration_form_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CenterId).HasColumnName("center_id");

                entity.Property(e => e.Dob)
                    .HasColumnName("dob")
                    .HasColumnType("date");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasColumnName("first_name")
                    .HasMaxLength(50);

                entity.Property(e => e.Gender).HasColumnName("gender");

                entity.Property(e => e.InsertedAt)
                    .HasColumnName("inserted_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasColumnName("last_name")
                    .HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasColumnName("phone")
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.RejectedReason)
                    .HasColumnName("rejected_reason")
                    .HasMaxLength(200);

                entity.Property(e => e.UpdatedAt)
                    .HasColumnName("updated_at")
                    .HasColumnType("datetime");

                entity.Property(e => e.VolunteerRegistrationFormImageUrl)
                    .HasColumnName("volunteer_registration_form_image_url")
                    .IsUnicode(false);

                entity.Property(e => e.VolunteerRegistrationFormStatus).HasColumnName("volunteer_registration_form_status");

                entity.HasOne(d => d.Center)
                    .WithMany(p => p.VolunteerRegistrationForm)
                    .HasForeignKey(d => d.CenterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_VolunteerRegistrationForm_Center");
            });

            modelBuilder.Entity<WorkingHistory>(entity =>
            {
                entity.Property(e => e.WorkingHistoryId)
                    .HasColumnName("working_history_id")
                    .ValueGeneratedNever();

                entity.Property(e => e.CenterId).HasColumnName("center_id");

                entity.Property(e => e.DateEnded)
                    .HasColumnName("date_ended")
                    .HasColumnType("datetime");

                entity.Property(e => e.DateStarted)
                    .HasColumnName("date_started")
                    .HasColumnType("datetime");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(1000);

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasColumnName("role_name")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Center)
                    .WithMany(p => p.WorkingHistory)
                    .HasForeignKey(d => d.CenterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WorkingHistory_Center");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.WorkingHistory)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WorkingHistory_User");
            });
        }
    }
}
