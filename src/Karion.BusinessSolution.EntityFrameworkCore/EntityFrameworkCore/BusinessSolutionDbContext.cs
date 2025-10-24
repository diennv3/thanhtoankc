using Karion.BusinessSolution.QuanLyDiemDanh;
using Karion.BusinessSolution.HISConnect;
using Karion.BusinessSolution.HanetTenant;
using Karion.BusinessSolution.VersionControl;
using Karion.BusinessSolution.QuanLyDanhMuc;
using Abp.IdentityServer4;
using Abp.Organizations;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Karion.BusinessSolution.Authorization.Delegation;
using Karion.BusinessSolution.Authorization.Roles;
using Karion.BusinessSolution.Authorization.Users;
using Karion.BusinessSolution.Chat;
using Karion.BusinessSolution.Editions;
using Karion.BusinessSolution.Friendships;
using Karion.BusinessSolution.MultiTenancy;
using Karion.BusinessSolution.MultiTenancy.Accounting;
using Karion.BusinessSolution.MultiTenancy.Payments;
using Karion.BusinessSolution.Storage;
using Karion.BusinessSolution.TBHostConfigure;

namespace Karion.BusinessSolution.EntityFrameworkCore
{
    public class BusinessSolutionDbContext : AbpZeroDbContext<Tenant, Role, User, BusinessSolutionDbContext>, IAbpPersistedGrantDbContext
    {
        public virtual DbSet<Shift> Shifts { get; set; }

        public virtual DbSet<ThongTinDonVi> ThongTinDonVies { get; set; }

        public virtual DbSet<Attendance> Attendances { get; set; }

        public virtual DbSet<RegistrationTransfered> RegistrationTransfereds { get; set; }

        public virtual DbSet<ChiTietThanhToan> ChiTietThanhToans { get; set; }

        public virtual DbSet<HanetFaceDetected> HanetFaceDetecteds { get; set; }

        public virtual DbSet<HanetTenantLog> HanetTenantLogs { get; set; }

        public virtual DbSet<HanetTenantDeviceDatas> HanetTenantDeviceDatases { get; set; }

        public virtual DbSet<HanetTenantPlaceDatas> HanetTenantPlaceDatases { get; set; }

        public virtual DbSet<ThongTinBacSiMoRong> ThongTinBacSiMoRongs { get; set; }

        public virtual DbSet<TechberConfigure> TechberConfigures { get; set; }

        public virtual DbSet<NguoiBenhNotification> NguoiBenhNotifications { get; set; }

        public virtual DbSet<BankCode> BankCodes { get; set; }

        public virtual DbSet<DanhSachVersion> DanhSachVersions { get; set; }

        public virtual DbSet<Version> Versions { get; set; }

        public virtual DbSet<BacSiChuyenKhoa> BacSiChuyenKhoas { get; set; }

        

        public virtual DbSet<LichHenKham> LichHenKhams { get; set; }

        public virtual DbSet<NguoiThan> NguoiThans { get; set; }

        public virtual DbSet<NguoiBenh> NguoiBenhs { get; set; }

        public virtual DbSet<BacSiDichVu> BacSiDichVus { get; set; }

        public virtual DbSet<GiaDichVu> GiaDichVus { get; set; }

        public virtual DbSet<ChuyenKhoa> ChuyenKhoas { get; set; }

        public virtual DbSet<DichVu> DichVus { get; set; }


        public virtual DbSet<PublicToken> PublicTokens { get; set; }

        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        public virtual DbSet<SubscriptionPaymentExtensionData> SubscriptionPaymentExtensionDatas { get; set; }

        public virtual DbSet<UserDelegation> UserDelegations { get; set; }

        public BusinessSolutionDbContext(DbContextOptions<BusinessSolutionDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           
           
           
           
           
           
           
           
           
           
           
           
           
           
           
           
            modelBuilder.Entity<Shift>(s =>
            {
                s.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<ThongTinDonVi>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<Attendance>(a =>
            {
                a.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<RegistrationTransfered>(r =>
            {
                r.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<ChiTietThanhToan>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<ThongTinBacSiMoRong>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<BacSiChuyenKhoa>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<LichHenKham>(l =>
            {
                l.HasIndex(e => new { e.TenantId });
            });

 
 modelBuilder.Entity<BacSiDichVu>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<GiaDichVu>(g =>
            {
                g.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<ChuyenKhoa>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<DichVu>(d =>
            {
                d.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<PublicToken>(p =>
            {
                p.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<BinaryObject>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { PaymentId = e.ExternalPaymentId, e.Gateway });
            });

            modelBuilder.Entity<SubscriptionPaymentExtensionData>(b =>
            {
                b.HasQueryFilter(m => !m.IsDeleted)
                    .HasIndex(e => new { e.SubscriptionPaymentId, e.Key, e.IsDeleted })
                    .IsUnique();
            });

            modelBuilder.Entity<UserDelegation>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.SourceUserId });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId });
            });

            modelBuilder.ConfigurePersistedGrantEntity();
        }
    }
}
