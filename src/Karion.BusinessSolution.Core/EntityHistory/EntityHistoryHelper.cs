using Karion.BusinessSolution.HanetTenant;
using Karion.BusinessSolution.TBHostConfigure;
using Karion.BusinessSolution.VersionControl;
using Karion.BusinessSolution.QuanLyDanhMuc;
using System;
using System.Linq;
using Abp.Organizations;
using Karion.BusinessSolution.Authorization.Roles;
using Karion.BusinessSolution.MultiTenancy;

namespace Karion.BusinessSolution.EntityHistory
{
    public static class EntityHistoryHelper
    {
        public const string EntityHistoryConfigurationName = "EntityHistory";

        public static readonly Type[] HostSideTrackedTypes =
        {
            typeof(ChiTietThanhToan),
            typeof(NguoiBenhNotification),
            typeof(HanetFaceDetected),
            typeof(HanetTenantDeviceDatas),
            typeof(HanetTenantPlaceDatas),
            typeof(ThongTinBacSiMoRong),
            typeof(TechberConfigure),
            typeof(BankCode),
            typeof(DanhSachVersion),
           
            typeof(BacSiChuyenKhoa),
            typeof(LichHenKham),
            typeof(NguoiThan),
            typeof(NguoiBenh),
            typeof(BacSiDichVu),
            typeof(GiaDichVu),
            typeof(ChuyenKhoa),
            typeof(DichVu),
            typeof(ThongTinDonVi),
            typeof(PublicToken),
            typeof(OrganizationUnit), typeof(Role), typeof(Tenant)
        };

        public static readonly Type[] TenantSideTrackedTypes =
        {
            typeof(ChiTietThanhToan),
            typeof(ThongTinBacSiMoRong),
            typeof(BacSiChuyenKhoa),
            typeof(LichHenKham),
            typeof(NguoiThan),
            typeof(NguoiBenh),
            typeof(BacSiDichVu),
            typeof(GiaDichVu),
            typeof(ChuyenKhoa),
            typeof(DichVu),
            typeof(ThongTinDonVi),
            typeof(PublicToken),
            typeof(OrganizationUnit), typeof(Role)
        };

        public static readonly Type[] TrackedTypes =
            HostSideTrackedTypes
                .Concat(TenantSideTrackedTypes)
                .GroupBy(type => type.FullName)
                .Select(types => types.First())
                .ToArray();
    }
}
