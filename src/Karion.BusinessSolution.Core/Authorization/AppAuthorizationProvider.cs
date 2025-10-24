using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Karion.BusinessSolution.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public AppAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public AppAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

            var leaveRequests = pages.CreateChildPermission(AppPermissions.Pages_LeaveRequests, L("LeaveRequests"));
            leaveRequests.CreateChildPermission(AppPermissions.Pages_LeaveRequests_Create, L("CreateNewLeaveRequest"));
            leaveRequests.CreateChildPermission(AppPermissions.Pages_LeaveRequests_Edit, L("EditLeaveRequest"));
            leaveRequests.CreateChildPermission(AppPermissions.Pages_LeaveRequests_Delete, L("DeleteLeaveRequest"));



            var shifts = pages.CreateChildPermission(AppPermissions.Pages_Shifts, L("Shifts"));
            shifts.CreateChildPermission(AppPermissions.Pages_Shifts_Create, L("CreateNewShift"));
            shifts.CreateChildPermission(AppPermissions.Pages_Shifts_Edit, L("EditShift"));
            shifts.CreateChildPermission(AppPermissions.Pages_Shifts_Delete, L("DeleteShift"));



            var attendances = pages.CreateChildPermission(AppPermissions.Pages_Attendances, L("Attendances"));
            attendances.CreateChildPermission(AppPermissions.Pages_Attendances_Create, L("CreateNewAttendance"));
            attendances.CreateChildPermission(AppPermissions.Pages_Attendances_Edit, L("EditAttendance"));
            attendances.CreateChildPermission(AppPermissions.Pages_Attendances_Delete, L("DeleteAttendance"));



            var registrationTransfereds = pages.CreateChildPermission(AppPermissions.Pages_RegistrationTransfereds, L("RegistrationTransfereds"));
            registrationTransfereds.CreateChildPermission(AppPermissions.Pages_RegistrationTransfereds_Create, L("CreateNewRegistrationTransfered"));
            registrationTransfereds.CreateChildPermission(AppPermissions.Pages_RegistrationTransfereds_Edit, L("EditRegistrationTransfered"));
            registrationTransfereds.CreateChildPermission(AppPermissions.Pages_RegistrationTransfereds_Delete, L("DeleteRegistrationTransfered"));



            var chiTietThanhToans = pages.CreateChildPermission(AppPermissions.Pages_ChiTietThanhToans, L("ChiTietThanhToans"));
            chiTietThanhToans.CreateChildPermission(AppPermissions.Pages_ChiTietThanhToans_Create, L("CreateNewChiTietThanhToan"));
            chiTietThanhToans.CreateChildPermission(AppPermissions.Pages_ChiTietThanhToans_Edit, L("EditChiTietThanhToan"));
            chiTietThanhToans.CreateChildPermission(AppPermissions.Pages_ChiTietThanhToans_Delete, L("DeleteChiTietThanhToan"));



            var hanetFaceDetecteds = pages.CreateChildPermission(AppPermissions.Pages_HanetFaceDetecteds, L("HanetFaceDetecteds"), multiTenancySides: MultiTenancySides.Host);
            hanetFaceDetecteds.CreateChildPermission(AppPermissions.Pages_HanetFaceDetecteds_Create, L("CreateNewHanetFaceDetected"), multiTenancySides: MultiTenancySides.Host);
            hanetFaceDetecteds.CreateChildPermission(AppPermissions.Pages_HanetFaceDetecteds_Edit, L("EditHanetFaceDetected"), multiTenancySides: MultiTenancySides.Host);
            hanetFaceDetecteds.CreateChildPermission(AppPermissions.Pages_HanetFaceDetecteds_Delete, L("DeleteHanetFaceDetected"), multiTenancySides: MultiTenancySides.Host);



            var hanetTenantLogs = pages.CreateChildPermission(AppPermissions.Pages_HanetTenantLogs, L("HanetTenantLogs"), multiTenancySides: MultiTenancySides.Host);
            hanetTenantLogs.CreateChildPermission(AppPermissions.Pages_HanetTenantLogs_Create, L("CreateNewHanetTenantLog"), multiTenancySides: MultiTenancySides.Host);
            hanetTenantLogs.CreateChildPermission(AppPermissions.Pages_HanetTenantLogs_Edit, L("EditHanetTenantLog"), multiTenancySides: MultiTenancySides.Host);
            hanetTenantLogs.CreateChildPermission(AppPermissions.Pages_HanetTenantLogs_Delete, L("DeleteHanetTenantLog"), multiTenancySides: MultiTenancySides.Host);



            var hanetTenantDeviceDatases = pages.CreateChildPermission(AppPermissions.Pages_HanetTenantDeviceDatases, L("HanetTenantDeviceDatases"), multiTenancySides: MultiTenancySides.Host);
            hanetTenantDeviceDatases.CreateChildPermission(AppPermissions.Pages_HanetTenantDeviceDatases_Create, L("CreateNewHanetTenantDeviceDatas"), multiTenancySides: MultiTenancySides.Host);
            hanetTenantDeviceDatases.CreateChildPermission(AppPermissions.Pages_HanetTenantDeviceDatases_Edit, L("EditHanetTenantDeviceDatas"), multiTenancySides: MultiTenancySides.Host);
            hanetTenantDeviceDatases.CreateChildPermission(AppPermissions.Pages_HanetTenantDeviceDatases_Delete, L("DeleteHanetTenantDeviceDatas"), multiTenancySides: MultiTenancySides.Host);



            var hanetTenantPlaceDatases = pages.CreateChildPermission(AppPermissions.Pages_HanetTenantPlaceDatases, L("HanetTenantPlaceDatases"), multiTenancySides: MultiTenancySides.Host);
            hanetTenantPlaceDatases.CreateChildPermission(AppPermissions.Pages_HanetTenantPlaceDatases_Create, L("CreateNewHanetTenantPlaceDatas"), multiTenancySides: MultiTenancySides.Host);
            hanetTenantPlaceDatases.CreateChildPermission(AppPermissions.Pages_HanetTenantPlaceDatases_Edit, L("EditHanetTenantPlaceDatas"), multiTenancySides: MultiTenancySides.Host);
            hanetTenantPlaceDatases.CreateChildPermission(AppPermissions.Pages_HanetTenantPlaceDatases_Delete, L("DeleteHanetTenantPlaceDatas"), multiTenancySides: MultiTenancySides.Host);



            var thongTinBacSiMoRongs = pages.CreateChildPermission(AppPermissions.Pages_ThongTinBacSiMoRongs, L("ThongTinBacSiMoRongs"));
            thongTinBacSiMoRongs.CreateChildPermission(AppPermissions.Pages_ThongTinBacSiMoRongs_Create, L("CreateNewThongTinBacSiMoRong"));
            thongTinBacSiMoRongs.CreateChildPermission(AppPermissions.Pages_ThongTinBacSiMoRongs_Edit, L("EditThongTinBacSiMoRong"));
            thongTinBacSiMoRongs.CreateChildPermission(AppPermissions.Pages_ThongTinBacSiMoRongs_Delete, L("DeleteThongTinBacSiMoRong"));



            var nguoiBenhNotifications = pages.CreateChildPermission(AppPermissions.Pages_NguoiBenhNotifications, L("NguoiBenhNotifications"), multiTenancySides: MultiTenancySides.Host);
            nguoiBenhNotifications.CreateChildPermission(AppPermissions.Pages_NguoiBenhNotifications_Create, L("CreateNewNguoiBenhNotification"), multiTenancySides: MultiTenancySides.Host);
            nguoiBenhNotifications.CreateChildPermission(AppPermissions.Pages_NguoiBenhNotifications_Edit, L("EditNguoiBenhNotification"), multiTenancySides: MultiTenancySides.Host);
            nguoiBenhNotifications.CreateChildPermission(AppPermissions.Pages_NguoiBenhNotifications_Delete, L("DeleteNguoiBenhNotification"), multiTenancySides: MultiTenancySides.Host);



            var techberConfigures = pages.CreateChildPermission(AppPermissions.Pages_TechberConfigures, L("TechberConfigures"), multiTenancySides: MultiTenancySides.Host);
            techberConfigures.CreateChildPermission(AppPermissions.Pages_TechberConfigures_Create, L("CreateNewTechberConfigure"), multiTenancySides: MultiTenancySides.Host);
            techberConfigures.CreateChildPermission(AppPermissions.Pages_TechberConfigures_Edit, L("EditTechberConfigure"), multiTenancySides: MultiTenancySides.Host);
            techberConfigures.CreateChildPermission(AppPermissions.Pages_TechberConfigures_Delete, L("DeleteTechberConfigure"), multiTenancySides: MultiTenancySides.Host);



            var bankCodes = pages.CreateChildPermission(AppPermissions.Pages_BankCodes, L("BankCodes"), multiTenancySides: MultiTenancySides.Host);
            bankCodes.CreateChildPermission(AppPermissions.Pages_BankCodes_Create, L("CreateNewBankCode"), multiTenancySides: MultiTenancySides.Host);
            bankCodes.CreateChildPermission(AppPermissions.Pages_BankCodes_Edit, L("EditBankCode"), multiTenancySides: MultiTenancySides.Host);
            bankCodes.CreateChildPermission(AppPermissions.Pages_BankCodes_Delete, L("DeleteBankCode"), multiTenancySides: MultiTenancySides.Host);



            var danhSachVersions = pages.CreateChildPermission(AppPermissions.Pages_DanhSachVersions, L("DanhSachVersions"), multiTenancySides: MultiTenancySides.Host);
            danhSachVersions.CreateChildPermission(AppPermissions.Pages_DanhSachVersions_Create, L("CreateNewDanhSachVersion"), multiTenancySides: MultiTenancySides.Host);
            danhSachVersions.CreateChildPermission(AppPermissions.Pages_DanhSachVersions_Edit, L("EditDanhSachVersion"), multiTenancySides: MultiTenancySides.Host);
            danhSachVersions.CreateChildPermission(AppPermissions.Pages_DanhSachVersions_Delete, L("DeleteDanhSachVersion"), multiTenancySides: MultiTenancySides.Host);



            var versions = pages.CreateChildPermission(AppPermissions.Pages_Versions, L("Versions"), multiTenancySides: MultiTenancySides.Host);
            versions.CreateChildPermission(AppPermissions.Pages_Versions_Create, L("CreateNewVersion"), multiTenancySides: MultiTenancySides.Host);
            versions.CreateChildPermission(AppPermissions.Pages_Versions_Edit, L("EditVersion"), multiTenancySides: MultiTenancySides.Host);
            versions.CreateChildPermission(AppPermissions.Pages_Versions_Delete, L("DeleteVersion"), multiTenancySides: MultiTenancySides.Host);



            var nguoiThans = pages.CreateChildPermission(AppPermissions.Pages_NguoiThans, L("NguoiThans"), multiTenancySides: MultiTenancySides.Host);
            nguoiThans.CreateChildPermission(AppPermissions.Pages_NguoiThans_Create, L("CreateNewNguoiThan"), multiTenancySides: MultiTenancySides.Host);
            nguoiThans.CreateChildPermission(AppPermissions.Pages_NguoiThans_Edit, L("EditNguoiThan"), multiTenancySides: MultiTenancySides.Host);
            nguoiThans.CreateChildPermission(AppPermissions.Pages_NguoiThans_Delete, L("DeleteNguoiThan"), multiTenancySides: MultiTenancySides.Host);



            var bacSiChuyenKhoas = pages.CreateChildPermission(AppPermissions.Pages_BacSiChuyenKhoas, L("BacSiChuyenKhoas"));
            bacSiChuyenKhoas.CreateChildPermission(AppPermissions.Pages_BacSiChuyenKhoas_Create, L("CreateNewBacSiChuyenKhoa"));
            bacSiChuyenKhoas.CreateChildPermission(AppPermissions.Pages_BacSiChuyenKhoas_Edit, L("EditBacSiChuyenKhoa"));
            bacSiChuyenKhoas.CreateChildPermission(AppPermissions.Pages_BacSiChuyenKhoas_Delete, L("DeleteBacSiChuyenKhoa"));



            var nguoiBenhs = pages.CreateChildPermission(AppPermissions.Pages_NguoiBenhs, L("NguoiBenhs"), multiTenancySides: MultiTenancySides.Host);
            nguoiBenhs.CreateChildPermission(AppPermissions.Pages_NguoiBenhs_Create, L("CreateNewNguoiBenh"), multiTenancySides: MultiTenancySides.Host);
            nguoiBenhs.CreateChildPermission(AppPermissions.Pages_NguoiBenhs_Edit, L("EditNguoiBenh"), multiTenancySides: MultiTenancySides.Host);
            nguoiBenhs.CreateChildPermission(AppPermissions.Pages_NguoiBenhs_Delete, L("DeleteNguoiBenh"), multiTenancySides: MultiTenancySides.Host);



            var nguoiBenhTests = pages.CreateChildPermission(AppPermissions.Pages_NguoiBenhTests, L("NguoiBenhTests"), multiTenancySides: MultiTenancySides.Host);
            nguoiBenhTests.CreateChildPermission(AppPermissions.Pages_NguoiBenhTests_Create, L("CreateNewNguoiBenhTest"), multiTenancySides: MultiTenancySides.Host);
            nguoiBenhTests.CreateChildPermission(AppPermissions.Pages_NguoiBenhTests_Edit, L("EditNguoiBenhTest"), multiTenancySides: MultiTenancySides.Host);
            nguoiBenhTests.CreateChildPermission(AppPermissions.Pages_NguoiBenhTests_Delete, L("DeleteNguoiBenhTest"), multiTenancySides: MultiTenancySides.Host);

            var tiepNhanBenhNhan = pages.CreateChildPermission(AppPermissions.Pages_TiepNhanBenhNhan, L("TiepNhanBenhNhan"), multiTenancySides: MultiTenancySides.Tenant);
            tiepNhanBenhNhan.CreateChildPermission(AppPermissions.Pages_TiepNhanBenhNhan_LeTan, L("LeTanTiepNhanBenhNhan"), multiTenancySides: MultiTenancySides.Tenant);
            tiepNhanBenhNhan.CreateChildPermission(AppPermissions.Pages_TiepNhanBenhNhan_BacSiChiDinh, L("BacSiChiDinhTiepNhanBenhNhan"), multiTenancySides: MultiTenancySides.Tenant);
            tiepNhanBenhNhan.CreateChildPermission(AppPermissions.Pages_TiepNhanBenhNhan_KhuVucThanhToan, L("KhuVucThanhToanTiepNhanBenhNhan"), multiTenancySides: MultiTenancySides.Tenant);
            
            pages.CreateChildPermission(AppPermissions.HISApiAccess, L("HISApiAccess"), multiTenancySides: MultiTenancySides.Tenant);


            var lichHenKhams = pages.CreateChildPermission(AppPermissions.Pages_LichHenKhams, L("LichHenKhams"));
            lichHenKhams.CreateChildPermission(AppPermissions.Pages_LichHenKhams_Create, L("CreateNewLichHenKham"));
            lichHenKhams.CreateChildPermission(AppPermissions.Pages_LichHenKhams_Edit, L("EditLichHenKham"));
            lichHenKhams.CreateChildPermission(AppPermissions.Pages_LichHenKhams_Delete, L("DeleteLichHenKham"));
            lichHenKhams.CreateChildPermission(AppPermissions.Pages_LichHenKhams_CapNhatDichVu, L("CapNhatDichVuLichHenKham"));

            var bacSiDichVus = pages.CreateChildPermission(AppPermissions.Pages_BacSiDichVus, L("BacSiDichVus"));
            bacSiDichVus.CreateChildPermission(AppPermissions.Pages_BacSiDichVus_Create, L("CreateNewBacSiDichVu"));
            bacSiDichVus.CreateChildPermission(AppPermissions.Pages_BacSiDichVus_Edit, L("EditBacSiDichVu"));
            bacSiDichVus.CreateChildPermission(AppPermissions.Pages_BacSiDichVus_Delete, L("DeleteBacSiDichVu"));
            
            var giaDichVus = pages.CreateChildPermission(AppPermissions.Pages_GiaDichVus, L("GiaDichVus"));
            giaDichVus.CreateChildPermission(AppPermissions.Pages_GiaDichVus_Create, L("CreateNewGiaDichVu"));
            giaDichVus.CreateChildPermission(AppPermissions.Pages_GiaDichVus_Edit, L("EditGiaDichVu"));
            giaDichVus.CreateChildPermission(AppPermissions.Pages_GiaDichVus_Delete, L("DeleteGiaDichVu"));



            var chuyenKhoas = pages.CreateChildPermission(AppPermissions.Pages_ChuyenKhoas, L("ChuyenKhoas"));
            chuyenKhoas.CreateChildPermission(AppPermissions.Pages_ChuyenKhoas_Create, L("CreateNewChuyenKhoa"));
            chuyenKhoas.CreateChildPermission(AppPermissions.Pages_ChuyenKhoas_Edit, L("EditChuyenKhoa"));
            chuyenKhoas.CreateChildPermission(AppPermissions.Pages_ChuyenKhoas_Delete, L("DeleteChuyenKhoa"));



            var dichVus = pages.CreateChildPermission(AppPermissions.Pages_DichVus, L("DichVus"));
            dichVus.CreateChildPermission(AppPermissions.Pages_DichVus_Create, L("CreateNewDichVu"));
            dichVus.CreateChildPermission(AppPermissions.Pages_DichVus_Edit, L("EditDichVu"));
            dichVus.CreateChildPermission(AppPermissions.Pages_DichVus_Delete, L("DeleteDichVu"));



            var thongTinDonVies = pages.CreateChildPermission(AppPermissions.Pages_ThongTinDonVies, L("ThongTinDonVies"));
            thongTinDonVies.CreateChildPermission(AppPermissions.Pages_ThongTinDonVies_Create, L("CreateNewThongTinDonVi"));
            thongTinDonVies.CreateChildPermission(AppPermissions.Pages_ThongTinDonVies_Edit, L("EditThongTinDonVi"));
            thongTinDonVies.CreateChildPermission(AppPermissions.Pages_ThongTinDonVies_Delete, L("DeleteThongTinDonVi"));



            var publicTokens = pages.CreateChildPermission(AppPermissions.Pages_PublicTokens, L("PublicTokens"));
            publicTokens.CreateChildPermission(AppPermissions.Pages_PublicTokens_Create, L("CreateNewPublicToken"));
            publicTokens.CreateChildPermission(AppPermissions.Pages_PublicTokens_Edit, L("EditPublicToken"));
            publicTokens.CreateChildPermission(AppPermissions.Pages_PublicTokens_Delete, L("DeletePublicToken"));


            pages.CreateChildPermission(AppPermissions.Pages_DemoUiComponents, L("DemoUiComponents"));

            var administration = pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

       
            


            var roles = administration.CreateChildPermission(AppPermissions.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Delete, L("DeletingRole"));

            var users = administration.CreateChildPermission(AppPermissions.Pages_Administration_Users, L("Users"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangePermissions, L("ChangingPermissions"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Impersonation, L("LoginForUsers"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Unlock, L("Unlock"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs, L("AuditLogs"));

            var organizationUnits = administration.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits, L("OrganizationUnits"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree, L("ManagingOrganizationTree"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers, L("ManagingMembers"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageRoles, L("ManagingRoles"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_UiCustomization, L("VisualSettings"));

            var webhooks = administration.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription, L("Webhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Create, L("CreatingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Edit, L("EditingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_ChangeActivity, L("ChangingWebhookActivity"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Detail, L("DetailingSubscription"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ListSendAttempts, L("ListingSendAttempts"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ResendWebhook, L("ResendingWebhook"));

            var dynamicParameters = administration.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameters, L("DynamicParameters"));
            dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameters_Create, L("CreatingDynamicParameters"));
            dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameters_Edit, L("EditingDynamicParameters"));
            dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameters_Delete, L("DeletingDynamicParameters"));

            var dynamicParameterValues = dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameterValue, L("DynamicParameterValue"));
            dynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameterValue_Create, L("CreatingDynamicParameterValue"));
            dynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameterValue_Edit, L("EditingDynamicParameterValue"));
            dynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameterValue_Delete, L("DeletingDynamicParameterValue"));

            var entityDynamicParameters = dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameters, L("EntityDynamicParameters"));
            entityDynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameters_Create, L("CreatingEntityDynamicParameters"));
            entityDynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameters_Edit, L("EditingEntityDynamicParameters"));
            entityDynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameters_Delete, L("DeletingEntityDynamicParameters"));

            var entityDynamicParameterValues = dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameterValue, L("EntityDynamicParameterValue"));
            entityDynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameterValue_Create, L("CreatingEntityDynamicParameterValue"));
            entityDynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameterValue_Edit, L("EditingEntityDynamicParameterValue"));
            entityDynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameterValue_Delete, L("DeletingEntityDynamicParameterValue"));

            //TENANT-SPECIFIC PERMISSIONS

            pages.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Tenant);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement, L("Subscription"), multiTenancySides: MultiTenancySides.Tenant);

            //HOST-SPECIFIC PERMISSIONS

            var editions = pages.CreateChildPermission(AppPermissions.Pages_Editions, L("Editions"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Create, L("CreatingNewEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Edit, L("EditingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Delete, L("DeletingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_MoveTenantsToAnotherEdition, L("MoveTenantsToAnotherEdition"), multiTenancySides: MultiTenancySides.Host);

            var tenants = pages.CreateChildPermission(AppPermissions.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Create, L("CreatingNewTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Edit, L("EditingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_ChangeFeatures, L("ChangingFeatures"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Delete, L("DeletingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Impersonation, L("LoginForTenants"), multiTenancySides: MultiTenancySides.Host);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Host);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Maintenance, L("Maintenance"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_HangfireDashboard, L("HangfireDashboard"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Host);

            var languages = administration.CreateChildPermission(AppPermissions.Pages_Administration_Languages, L("Languages"), multiTenancySides: MultiTenancySides.Host);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Create, L("CreatingNewLanguage"), multiTenancySides: MultiTenancySides.Host);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Edit, L("EditingLanguage"), multiTenancySides: MultiTenancySides.Host);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Delete, L("DeletingLanguages"), multiTenancySides: MultiTenancySides.Host);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeTexts, L("ChangingTexts"), multiTenancySides: MultiTenancySides.Host);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, BusinessSolutionConsts.LocalizationSourceName);
        }
    }
}
