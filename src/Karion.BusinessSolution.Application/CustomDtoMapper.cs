using Karion.BusinessSolution.QuanLyDiemDanh.Dtos;
using Karion.BusinessSolution.QuanLyDiemDanh;
using Karion.BusinessSolution.HISConnect.Dtos;
using Karion.BusinessSolution.HISConnect;
using Karion.BusinessSolution.HanetTenant.Dtos;
using Karion.BusinessSolution.HanetTenant;
using Karion.BusinessSolution.TBHostConfigure.Dtos;
using Karion.BusinessSolution.TBHostConfigure;
using Karion.BusinessSolution.VersionControl.Dtos;
using Karion.BusinessSolution.VersionControl;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.QuanLyDanhMuc;
using Abp.Application.Editions;
using Abp.Application.Features;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.DynamicEntityParameters;
using Abp.EntityHistory;
using Abp.Localization;
using Abp.Notifications;
using Abp.Organizations;
using Abp.UI.Inputs;
using Abp.Webhooks;
using AutoMapper;
using Karion.BusinessSolution.Auditing.Dto;
using Karion.BusinessSolution.Authorization.Accounts.Dto;
using Karion.BusinessSolution.Authorization.Delegation;
using Karion.BusinessSolution.Authorization.Permissions.Dto;
using Karion.BusinessSolution.Authorization.Roles;
using Karion.BusinessSolution.Authorization.Roles.Dto;
using Karion.BusinessSolution.Authorization.Users;
using Karion.BusinessSolution.Authorization.Users.Delegation.Dto;
using Karion.BusinessSolution.Authorization.Users.Dto;
using Karion.BusinessSolution.Authorization.Users.Importing.Dto;
using Karion.BusinessSolution.Authorization.Users.Profile.Dto;
using Karion.BusinessSolution.Chat;
using Karion.BusinessSolution.Chat.Dto;
using Karion.BusinessSolution.DynamicEntityParameters.Dto;
using Karion.BusinessSolution.Editions;
using Karion.BusinessSolution.Editions.Dto;
using Karion.BusinessSolution.Friendships;
using Karion.BusinessSolution.Friendships.Cache;
using Karion.BusinessSolution.Friendships.Dto;
using Karion.BusinessSolution.Localization.Dto;
using Karion.BusinessSolution.MultiTenancy;
using Karion.BusinessSolution.MultiTenancy.Dto;
using Karion.BusinessSolution.MultiTenancy.HostDashboard.Dto;
using Karion.BusinessSolution.MultiTenancy.Payments;
using Karion.BusinessSolution.MultiTenancy.Payments.Dto;
using Karion.BusinessSolution.Notifications.Dto;
using Karion.BusinessSolution.Organizations.Dto;
using Karion.BusinessSolution.Sessions.Dto;
using Karion.BusinessSolution.WebHooks.Dto;

namespace Karion.BusinessSolution
{
    internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<CreateOrEditShiftDto, Shift>().ReverseMap();
            configuration.CreateMap<ShiftDto, Shift>().ReverseMap();
            configuration.CreateMap<CreateOrEditThongTinDonViDto, ThongTinDonVi>().ReverseMap();
            configuration.CreateMap<ThongTinDonViDto, ThongTinDonVi>().ReverseMap();
            configuration.CreateMap<CreateOrEditAttendanceDto, Attendance>().ReverseMap();
            configuration.CreateMap<AttendanceDto, Attendance>().ReverseMap();
            configuration.CreateMap<CreateOrEditRegistrationTransferedDto, RegistrationTransfered>().ReverseMap();
            configuration.CreateMap<RegistrationTransferedDto, RegistrationTransfered>().ReverseMap();
            configuration.CreateMap<CreateOrEditChiTietThanhToanDto, ChiTietThanhToan>().ReverseMap();
            configuration.CreateMap<ChiTietThanhToanDto, ChiTietThanhToan>().ReverseMap();
            configuration.CreateMap<CreateOrEditHanetFaceDetectedDto, HanetFaceDetected>().ReverseMap();
            configuration.CreateMap<HanetFaceDetectedDto, HanetFaceDetected>().ReverseMap();
            configuration.CreateMap<CreateOrEditHanetTenantLogDto, HanetTenantLog>().ReverseMap();
            configuration.CreateMap<HanetTenantLogDto, HanetTenantLog>().ReverseMap();
            configuration.CreateMap<CreateOrEditHanetTenantDeviceDatasDto, HanetTenantDeviceDatas>().ReverseMap();
            configuration.CreateMap<HanetTenantDeviceDatasDto, HanetTenantDeviceDatas>().ReverseMap();
            configuration.CreateMap<CreateOrEditHanetTenantPlaceDatasDto, HanetTenantPlaceDatas>().ReverseMap();
            configuration.CreateMap<HanetTenantPlaceDatasDto, HanetTenantPlaceDatas>().ReverseMap();
            configuration.CreateMap<CreateOrEditThongTinBacSiMoRongDto, ThongTinBacSiMoRong>().ReverseMap();
            configuration.CreateMap<ThongTinBacSiMoRongDto, ThongTinBacSiMoRong>().ReverseMap();
            configuration.CreateMap<CreateOrEditTechberConfigureDto, TechberConfigure>().ReverseMap();
            configuration.CreateMap<TechberConfigureDto, TechberConfigure>().ReverseMap();
            configuration.CreateMap<CreateOrEditNguoiBenhNotificationDto, NguoiBenhNotification>().ReverseMap();
            configuration.CreateMap<NguoiBenhNotificationDto, NguoiBenhNotification>().ReverseMap();
            configuration.CreateMap<CreateOrEditBankCodeDto, BankCode>().ReverseMap();
            configuration.CreateMap<BankCodeDto, BankCode>().ReverseMap();
            configuration.CreateMap<CreateOrEditDanhSachVersionDto, DanhSachVersion>().ReverseMap();
            configuration.CreateMap<DanhSachVersionDto, DanhSachVersion>().ReverseMap();
            configuration.CreateMap<CreateOrEditVersionDto, Version>().ReverseMap();
            configuration.CreateMap<VersionDto, Version>().ReverseMap();
            configuration.CreateMap<CreateOrEditBacSiChuyenKhoaDto, BacSiChuyenKhoa>().ReverseMap();
            configuration.CreateMap<BacSiChuyenKhoaDto, BacSiChuyenKhoa>().ReverseMap();
           
            configuration.CreateMap<CreateOrEditLichHenKhamDto, LichHenKham>().ReverseMap();
            configuration.CreateMap<LichHenKhamDto, LichHenKham>().ReverseMap();
            configuration.CreateMap<CreateOrEditNguoiThanDto, NguoiThan>().ReverseMap();
            configuration.CreateMap<NguoiThanDto, NguoiThan>().ReverseMap();
            configuration.CreateMap<CreateOrEditNguoiBenhDto, NguoiBenh>().ReverseMap();
            configuration.CreateMap<NguoiBenhDto, NguoiBenh>().ReverseMap();
            configuration.CreateMap<CreateOrEditBacSiDichVuDto, BacSiDichVu>().ReverseMap();
            configuration.CreateMap<BacSiDichVuDto, BacSiDichVu>().ReverseMap();
            configuration.CreateMap<CreateOrEditGiaDichVuDto, GiaDichVu>().ReverseMap();
            configuration.CreateMap<GiaDichVuDto, GiaDichVu>().ReverseMap();
            configuration.CreateMap<CreateOrEditChuyenKhoaDto, ChuyenKhoa>().ReverseMap();
            configuration.CreateMap<ChuyenKhoaDto, ChuyenKhoa>().ReverseMap();
            configuration.CreateMap<CreateOrEditDichVuDto, DichVu>().ReverseMap();
            configuration.CreateMap<DichVuDto, DichVu>().ReverseMap();
            configuration.CreateMap<CreateOrEditPublicTokenDto, PublicToken>().ReverseMap();
            configuration.CreateMap<PublicTokenDto, PublicToken>().ReverseMap();
            //Inputs
            configuration.CreateMap<CheckboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<SingleLineStringInputType, FeatureInputTypeDto>();
            configuration.CreateMap<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<IInputType, FeatureInputTypeDto>()
                .Include<CheckboxInputType, FeatureInputTypeDto>()
                .Include<SingleLineStringInputType, FeatureInputTypeDto>()
                .Include<ComboboxInputType, FeatureInputTypeDto>();
            configuration.CreateMap<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<ILocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>()
                .Include<StaticLocalizableComboboxItemSource, LocalizableComboboxItemSourceDto>();
            configuration.CreateMap<LocalizableComboboxItem, LocalizableComboboxItemDto>();
            configuration.CreateMap<ILocalizableComboboxItem, LocalizableComboboxItemDto>()
                .Include<LocalizableComboboxItem, LocalizableComboboxItemDto>();

            //Chat
            configuration.CreateMap<ChatMessage, ChatMessageDto>();
            configuration.CreateMap<ChatMessage, ChatMessageExportDto>();

            //Feature
            configuration.CreateMap<FlatFeatureSelectDto, Feature>().ReverseMap();
            configuration.CreateMap<Feature, FlatFeatureDto>();

            //Role
            configuration.CreateMap<RoleEditDto, Role>().ReverseMap();
            configuration.CreateMap<Role, RoleListDto>();
            configuration.CreateMap<UserRole, UserListRoleDto>();

            //Edition
            configuration.CreateMap<EditionEditDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<EditionCreateDto, SubscribableEdition>();
            configuration.CreateMap<EditionSelectDto, SubscribableEdition>().ReverseMap();
            configuration.CreateMap<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<Edition, EditionInfoDto>().Include<SubscribableEdition, EditionInfoDto>();

            configuration.CreateMap<SubscribableEdition, EditionListDto>();
            configuration.CreateMap<Edition, EditionEditDto>();
            configuration.CreateMap<Edition, SubscribableEdition>();
            configuration.CreateMap<Edition, EditionSelectDto>();


            //Payment
            configuration.CreateMap<SubscriptionPaymentDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPaymentListDto, SubscriptionPayment>().ReverseMap();
            configuration.CreateMap<SubscriptionPayment, SubscriptionPaymentInfoDto>();

            //Permission
            configuration.CreateMap<Permission, FlatPermissionDto>();
            configuration.CreateMap<Permission, FlatPermissionWithLevelDto>();

            //Language
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageListDto>();
            configuration.CreateMap<NotificationDefinition, NotificationSubscriptionWithDisplayNameDto>();
            configuration.CreateMap<ApplicationLanguage, ApplicationLanguageEditDto>()
                .ForMember(ldto => ldto.IsEnabled, options => options.MapFrom(l => !l.IsDisabled));

            //Tenant
            configuration.CreateMap<Tenant, RecentTenant>();
            configuration.CreateMap<Tenant, TenantLoginInfoDto>();
            configuration.CreateMap<Tenant, TenantListDto>();
            configuration.CreateMap<TenantEditDto, Tenant>().ReverseMap();
            configuration.CreateMap<CurrentTenantInfoDto, Tenant>().ReverseMap();

            //User
            configuration.CreateMap<User, UserEditDto>()
                .ForMember(dto => dto.Password, options => options.Ignore())
                .ReverseMap()
                .ForMember(user => user.Password, options => options.Ignore());
            configuration.CreateMap<User, UserLoginInfoDto>();
            configuration.CreateMap<User, UserListDto>();
            configuration.CreateMap<User, ChatUserDto>();
            configuration.CreateMap<User, OrganizationUnitUserListDto>();
            configuration.CreateMap<Role, OrganizationUnitRoleListDto>();
            configuration.CreateMap<CurrentUserProfileEditDto, User>().ReverseMap();
            configuration.CreateMap<UserLoginAttemptDto, UserLoginAttempt>().ReverseMap();
            configuration.CreateMap<ImportUserDto, User>();

            //AuditLog
            configuration.CreateMap<AuditLog, AuditLogListDto>();
            configuration.CreateMap<EntityChange, EntityChangeListDto>();
            configuration.CreateMap<EntityPropertyChange, EntityPropertyChangeDto>();

            //Friendship
            configuration.CreateMap<Friendship, FriendDto>();
            configuration.CreateMap<FriendCacheItem, FriendDto>();

            //OrganizationUnit
            configuration.CreateMap<OrganizationUnit, OrganizationUnitDto>();

            //Webhooks
            configuration.CreateMap<WebhookSubscription, GetAllSubscriptionsOutput>();
            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOutput>()
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.WebhookName,
                    options => options.MapFrom(l => l.WebhookEvent.WebhookName))
                .ForMember(webhookSendAttemptListDto => webhookSendAttemptListDto.Data,
                    options => options.MapFrom(l => l.WebhookEvent.Data));

            configuration.CreateMap<WebhookSendAttempt, GetAllSendAttemptsOfWebhookEventOutput>();

            configuration.CreateMap<DynamicParameter, DynamicParameterDto>().ReverseMap();
            configuration.CreateMap<DynamicParameterValue, DynamicParameterValueDto>().ReverseMap();
            configuration.CreateMap<EntityDynamicParameter, EntityDynamicParameterDto>()
                .ForMember(dto => dto.DynamicParameterName,
                    options => options.MapFrom(entity => entity.DynamicParameter.ParameterName));
            configuration.CreateMap<EntityDynamicParameterDto, EntityDynamicParameter>();

            configuration.CreateMap<EntityDynamicParameterValue, EntityDynamicParameterValueDto>().ReverseMap();
            //User Delegations
            configuration.CreateMap<CreateUserDelegationDto, UserDelegation>();


            /* ADD YOUR OWN CUSTOM AUTOMAPPER MAPPINGS HERE */
        }
    }
}
