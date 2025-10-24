
using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using Karion.BusinessSolution.Authorization;

namespace Karion.BusinessSolution.Web.Areas.App.Startup
{
    public class AppNavigationProvider : NavigationProvider
    {
        public const string MenuName = "App";

        public override void SetNavigation(INavigationProviderContext context)
        {
            var menu = context.Manager.Menus[MenuName] =
                new MenuDefinition(MenuName, new FixedLocalizableString("Main Menu"));
            var thongKeBaoCao = new MenuItemDefinition(
                AppPageNames.Common.BacSiChuyenKhoas,
                L("ThongKeBaoCaos"),
                url: "App/ThongKeBaoCaos",
                icon: "fas fa-user-md",
                permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Administration_Host_Dashboard)
            );
            var nguoiThan = new MenuItemDefinition(
                AppPageNames.Host.NguoiThans,
                L("NhanViens"),
                url: "App/NguoiThans",
                icon: "fas fa-people-arrows",
                permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_NguoiThans)
            );
            var bacSiChuyenKhoa = new MenuItemDefinition(
                AppPageNames.Common.BacSiChuyenKhoas,
                L("BacSiChuyenKhoas"),
                url: "App/BacSiChuyenKhoas",
                icon: "fas fa-user-md",
                permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_BacSiChuyenKhoas)
            );
            var bacSiDichVu = new MenuItemDefinition(
                AppPageNames.Common.BacSiDichVus,
                L("BacSiDichVus"),
                url: "App/BacSiDichVus",
                icon: "fas fa-user-md",
                permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_BacSiDichVus)
            );
            var giaDichVu = new MenuItemDefinition(
                AppPageNames.Common.GiaDichVus,
                L("GiaDichVus"),
                url: "App/GiaDichVus",
                icon: "flaticon-notepad",
                permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_GiaDichVus)
            );
            var chuyenKhoa = new MenuItemDefinition(
                AppPageNames.Common.ChuyenKhoas,
                L("ChuyenKhoas"),
                url: "App/ChuyenKhoas",
                icon: "fas fa-capsules",
                permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_ChuyenKhoas)
            );
            var dichVu = new MenuItemDefinition(
                AppPageNames.Common.DichVus,
                L("DichVus"),
                url: "App/DichVus",
                icon: "fas fa-briefcase-medical",
                permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_DichVus)
            );

            var publicToken = new MenuItemDefinition(
                AppPageNames.Common.PublicTokens,
                L("PublicTokens"),
                url: "App/PublicTokens",
                icon: "fas fa-file-code",
                permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_PublicTokens)
            );
            var danhSachVersion = new MenuItemDefinition(
                AppPageNames.Host.DanhSachVersions,
                L("DanhSachVersions"),
                url: "App/DanhSachVersions",
                icon: "fas fa-cannabis",
                permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_DanhSachVersions)
            );
            var thongTinBacSiMoRong = new MenuItemDefinition(
                AppPageNames.Common.ThongTinBacSiMoRongs,
                L("ThongTinBacSiMoRongs"),
                url: "App/ThongTinBacSiMoRongs",
                icon: "fas fa-user-md",
                permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_ThongTinBacSiMoRongs)
            );
            // .AddItem(new MenuItemDefinition(
            //         AppPageNames.Host.NguoiBenhTests,
            //         L("NguoiBenhTests"),
            //         url: "App/NguoiBenhTests",
            //         icon: "flaticon-more",
            //         permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_NguoiBenhTests)
            //     )
            // )


            menu
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Host.Dashboard,
                        L("Dashboard"),
                        url: "App/HostDashboard",
                        icon: "fas fa-heartbeat",
                        permissionDependency: new SimplePermissionDependency(AppPermissions
                            .Pages_Administration_Host_Dashboard)
                    )
                )
                .AddItem(thongKeBaoCao)
                // .AddItem(new MenuItemDefinition(
                //         AppPageNames.Host.HanetFaceDetecteds,
                //         L("HanetFaceDetecteds"),
                //         url: "App/HanetFaceDetecteds",
                //         icon: "flaticon-more",
                //         permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_HanetFaceDetecteds)
                //     )
                // )
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Host.NguoiBenhs,
                        L("NguoiBenhs"),
                        url: "App/NguoiBenhs",
                        icon: "fas fa-user-injured",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_NguoiBenhs)
                    )
                )
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Host.Attendances,
                        L("Attendances"),
                        url: "App/Attendances",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Attendances)
                    )
                )
                // .AddItem(new MenuItemDefinition(
                //         AppPageNames.Host.NguoiBenhNotifications,
                //         L("NguoiBenhNotifications"),
                //         url: "App/NguoiBenhNotifications",
                //         icon: "flaticon-more",
                //         permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_NguoiBenhNotifications)
                //     )
                // )
                //.AddItem(new MenuItemDefinition(
                //        AppPageNames.Common.LichHenKhams,
                //        L("LichHenKhams"),
                //        url: "App/LichHenKhams",
                //        icon: "fa-solid fa-stethoscope",
                //        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_LichHenKhams)
                //    )
                //)

                //.AddItem(new MenuItemDefinition(
                //                        AppPageNames.Common.ChiTietThanhToans,
                //                        L("ChiTietThanhToans"),
                //                        url: "App/ChiTietThanhToans",
                //                        icon: "flaticon-more",
                //                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_ChiTietThanhToans)
                //                    )
                //                )
                // .AddItem(new MenuItemDefinition(
                //         AppPageNames.Tenant.Dashboard,
                //         L("TiepNhanBenhNhan"),
                //         url: "App/TiepNhanBenhNhan",
                //         icon: "flaticon2-calendar-3",
                //         permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_TiepNhanBenhNhan)
                //     )
                // )
                //.AddItem(new MenuItemDefinition(
                //        "",
                //        L("TuyChinhBenhVien"),
                //        icon: "flaticon-cogwheel-1"
                //    )
                //    .AddItem(chuyenKhoa)
                //    //.AddItem(dichVu)
                //    .AddItem(nguoiThan)
                //    //.AddItem(publicToken)
                //    .AddItem(danhSachVersion)
                //    //.AddItem(giaDichVu)
                //    .AddItem(bacSiChuyenKhoa)
                //    //.AddItem(bacSiDichVu)
                //    //.AddItem(thongTinBacSiMoRong)
                //    //.AddItem(thongTinDonVi)
                //)
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Host.Tenants,
                        L("Tenants"),
                        url: "App/Tenants",
                        icon: "flaticon-list-3",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Tenants)
                    )
                ).AddItem(new MenuItemDefinition(
                        AppPageNames.Host.Editions,
                        L("Editions"),
                        url: "App/Editions",
                        icon: "flaticon-app",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Editions)
                    )
                )
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Tenant.Dashboard,
                        L("Dashboard"),
                        url: "App/TenantDashboard",
                        icon: "flaticon-line-graph",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Tenant_Dashboard)
                    )
                ).AddItem(new MenuItemDefinition(
                        AppPageNames.Common.Administration,
                        L("Administration"),
                        icon: "flaticon-interface-8"
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.OrganizationUnits,
                            L("OrganizationUnits"),
                            url: "App/OrganizationUnits",
                            icon: "flaticon-map",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_OrganizationUnits)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Roles,
                            L("Roles"),
                            url: "App/Roles",
                            icon: "flaticon-suitcase",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Roles)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Users,
                            L("Users"),
                            url: "App/Users",
                            icon: "flaticon-users",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Users)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Languages,
                            L("Languages"),
                            url: "App/Languages",
                            icon: "flaticon-tabs",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Languages)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.AuditLogs,
                            L("AuditLogs"),
                            url: "App/AuditLogs",
                            icon: "flaticon-folder-1",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_AuditLogs)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Host.Maintenance,
                            L("Maintenance"),
                            url: "App/Maintenance",
                            icon: "flaticon-lock",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Host_Maintenance)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Tenant.SubscriptionManagement,
                            L("Subscription"),
                            url: "App/SubscriptionManagement",
                            icon: "flaticon-refresh",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Tenant_SubscriptionManagement)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.UiCustomization,
                            L("VisualSettings"),
                            url: "App/UiCustomization",
                            icon: "flaticon-medical",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_UiCustomization)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.WebhookSubscriptions,
                            L("WebhookSubscriptions"),
                            url: "App/WebhookSubscription",
                            icon: "flaticon2-world",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_WebhookSubscription)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.DynamicEntityParameters,
                            L("DynamicParameters"),
                            icon: "flaticon-interface-8"
                        ).AddItem(new MenuItemDefinition(
                                AppPageNames.Common.DynamicParameters,
                                L("Definitions"),
                                url: "App/DynamicParameter",
                                icon: "flaticon-map",
                                permissionDependency: new SimplePermissionDependency(AppPermissions
                                    .Pages_Administration_DynamicParameters)
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                AppPageNames.Common.EntityDynamicParameters,
                                L("EntityDynamicParameters"),
                                url: "App/EntityDynamicParameter",
                                icon: "flaticon-map",
                                permissionDependency: new SimplePermissionDependency(AppPermissions
                                    .Pages_Administration_EntityDynamicParameters)
                            )
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Host.Settings,
                            L("Settings"),
                            url: "App/HostSettings",
                            icon: "flaticon-settings",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Host_Settings)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Tenant.Settings,
                            L("Settings"),
                            url: "App/Settings",
                            icon: "flaticon-settings",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Tenant_Settings)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                        AppPageNames.Tenant.Settings,
                        L("BankInfos"),
                        url: "App/BankCodes",
                        icon: "flaticon-piggy-bank",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_BankCodes)
                        )
                    )
                    // .AddItem(new MenuItemDefinition(
                    //         AppPageNames.Common.DemoUiComponents,
                    //         L("DemoUiComponents"),
                    //         url: "App/DemoUiComponents",
                    //         icon: "flaticon-shapes",
                    //         permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_DemoUiComponents)
                    //     )
                    // )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Host.TechberConfigures,
                            L("TechberConfigures"),
                            url: "App/TechberConfigures",
                            icon: "flaticon-more",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_TechberConfigures)
                        )
                    )
                );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, BusinessSolutionConsts.LocalizationSourceName);
        }
    }
}