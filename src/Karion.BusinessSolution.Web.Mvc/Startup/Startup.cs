using System;
using System.IO;
using Abp.AspNetCore;
using Abp.AspNetCore.SignalR.Hubs;
using Abp.AspNetZeroCore.Web.Authentication.JwtBearer;
using Abp.Castle.Logging.Log4Net;
using Abp.Hangfire;
using Abp.PlugIns;
using Castle.Facilities.Logging;
using GraphQL.Server;
using GraphQL.Server.Ui.Playground;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Karion.BusinessSolution.Authorization;
using Karion.BusinessSolution.Configuration;
using Karion.BusinessSolution.Configure;
using Karion.BusinessSolution.EntityFrameworkCore;
using Karion.BusinessSolution.Identity;
using Karion.BusinessSolution.Schemas;
using Karion.BusinessSolution.Web.Chat.SignalR;
using Karion.BusinessSolution.Web.Common;
using Karion.BusinessSolution.Web.Resources;
using Swashbuckle.AspNetCore.Swagger;
using Karion.BusinessSolution.Web.IdentityServer;
using Karion.BusinessSolution.Web.Swagger;
using Stripe;
using System.Reflection;
using Abp.AspNetCore.Mvc.Antiforgery;
using HealthChecks.UI.Client;
using IdentityServer4.Configuration;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Karion.BusinessSolution.Web.HealthCheck;
using Karion.BusinessSolution.Web.ThanhToanKhongChamSocket.SignalR;
using Owl.reCAPTCHA;
using HealthChecksUISettings = HealthChecks.UI.Configuration.Settings;

namespace Karion.BusinessSolution.Web.Startup
{
    public class Startup
    {
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public Startup(IWebHostEnvironment env)
        {
            _appConfiguration = env.GetAppConfiguration();
            _hostingEnvironment = env;
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // MVC
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AbpAutoValidateAntiforgeryTokenAttribute());
            })
#if DEBUG
                .AddRazorRuntimeCompilation()
#endif
                .AddNewtonsoftJson();

            IdentityRegistrar.Register(services);
            var wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Common", "FireBase");
            var firebaseJsonPath = Path.Combine(wwwRootPath, "bvqy7.json");
            FirebaseHelper.InitializeFirebase(firebaseJsonPath);
            
            //Identity server
            if (bool.Parse(_appConfiguration["IdentityServer:IsEnabled"]))
            {
                IdentityServerRegistrar.Register(services, _appConfiguration, options =>
                    options.UserInteraction = new UserInteractionOptions()
                    {
                        LoginUrl = "/Account/Login",
                        LogoutUrl = "/Account/LogOut",
                        ErrorUrl = "/Error"
                    });
            }

            AuthConfigurer.Configure(services, _appConfiguration);

            if (WebConsts.SwaggerUiEnabled)
            {
                //Swagger - Enable this line and the related lines in Configure method to enable swagger UI
                services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo() { Title = "BusinessSolution API", Version = "v1" });
                    options.DocInclusionPredicate((docName, description) => true);
                    options.ParameterFilter<SwaggerEnumParameterFilter>();
                    options.SchemaFilter<SwaggerEnumSchemaFilter>();
                    options.OperationFilter<SwaggerOperationIdFilter>();
                    options.OperationFilter<SwaggerOperationFilter>();
                    options.CustomDefaultSchemaIdSelector();
                    options.CustomSchemaIds(type => type.FullName);
                }).AddSwaggerGenNewtonsoftSupport();
            }
            services.AddHostedService<AttendanceNotificationHostedService>();
            //Recaptcha
            services.AddreCAPTCHAV3(x =>
            {
                x.SiteKey = _appConfiguration["Recaptcha:SiteKey"];
                x.SiteSecret = _appConfiguration["Recaptcha:SecretKey"];
            });

            if (WebConsts.HangfireDashboardEnabled)
            {
                //Hangfire (Enable to use Hangfire instead of default job manager)
                services.AddHangfire(config =>
                {
                    config.UseSqlServerStorage(_appConfiguration.GetConnectionString("Default"));
                });
            }

            services.AddScoped<IWebResourceManager, WebResourceManager>();

            services.AddSignalR();

            if (WebConsts.GraphQL.Enabled)
            {
                services.AddAndConfigureGraphQL();
            }

            services.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.Zero;
            });

            if (bool.Parse(_appConfiguration["HealthChecks:HealthChecksEnabled"]))
            {
                services.AddAbpZeroHealthCheck();

                var healthCheckUISection = _appConfiguration.GetSection("HealthChecks")?.GetSection("HealthChecksUI");

                if (bool.Parse(healthCheckUISection["HealthChecksUIEnabled"]))
                {
                    services.Configure<HealthChecksUISettings>(settings =>
                    {
                        healthCheckUISection.Bind(settings, c => c.BindNonPublicProperties = true);
                    });
                    services.AddHealthChecksUI();
                }
            }

            services.Configure<RazorViewEngineOptions>(options =>
            {
                options.ViewLocationExpanders.Add(new RazorViewLocationExpander());
            });
            // services.AddHostedService<MobileAppServices.MobileAppServices.MobileAppServices>();
            //Configure Abp and Dependency Injection
            return services.AddAbp<BusinessSolutionWebMvcModule>(options =>
            {
                //Configure Log4Net logging
                options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig(_hostingEnvironment.IsDevelopment()
                        ? "log4net.config"
                        : "log4net.Production.config")
                );

                options.PlugInSources.AddFolder(Path.Combine(_hostingEnvironment.WebRootPath, "Plugins"), SearchOption.AllDirectories);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            //Initializes ABP framework.
            app.UseAbp(options =>
            {
                options.UseAbpRequestLocalization = false; //used below: UseAbpRequestLocalization
            });
            app.UseHttpsRedirection();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithRedirects("~/Error?statusCode={0}");
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication();

            if (bool.Parse(_appConfiguration["Authentication:JwtBearer:IsEnabled"]))
            {
                app.UseJwtTokenMiddleware();
            }

            if (bool.Parse(_appConfiguration["IdentityServer:IsEnabled"]))
            {
                app.UseJwtTokenMiddleware("IdentityBearer");
                app.UseIdentityServer();
            }

            app.UseAuthorization();

            using (var scope = app.ApplicationServices.CreateScope())
            {
                if (scope.ServiceProvider.GetService<DatabaseCheckHelper>().Exist(_appConfiguration["ConnectionStrings:Default"]))
                {
                    app.UseAbpRequestLocalization();
                }
            }

            if (WebConsts.HangfireDashboardEnabled)
            {
                app.UseHangfireDashboard("/hangfire", new DashboardOptions
                {
                    Authorization = new[] { new AbpHangfireAuthorizationFilter(AppPermissions.Pages_Administration_HangfireDashboard) }
                });
                app.UseHangfireServer();
            }

            if (bool.Parse(_appConfiguration["Payment:Stripe:IsActive"]))
            {
                StripeConfiguration.ApiKey = _appConfiguration["Payment:Stripe:SecretKey"];
            }

            if (WebConsts.GraphQL.Enabled)
            {
                app.UseGraphQL<MainSchema>();
                if (WebConsts.GraphQL.PlaygroundEnabled)
                {
                    app.UseGraphQLPlayground(
                        new GraphQLPlaygroundOptions()); //to explorer API navigate https://*DOMAIN*/ui/playground
                }
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<AbpCommonHub>("/signalr");
                endpoints.MapHub<ChatHub>("/signalr-chat");
                endpoints.MapHub<ThanhToanKhongChamHub>("/signalr-ttkc"); //dùng để gọi ivoke trong file hub.cs

                endpoints.MapControllerRoute("defaultWithArea", "{area}/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

                if (bool.Parse(_appConfiguration["HealthChecks:HealthChecksEnabled"]))
                {
                    endpoints.MapHealthChecks("/health", new HealthCheckOptions()
                    {
                        Predicate = _ => true,
                        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                    });
                }
            });

            if (bool.Parse(_appConfiguration["HealthChecks:HealthChecksEnabled"]))
            {
                if (bool.Parse(_appConfiguration["HealthChecks:HealthChecksUI:HealthChecksUIEnabled"]))
                {
                    app.UseHealthChecksUI();
                }
            }

            if (WebConsts.SwaggerUiEnabled)
            {
                // Enable middleware to serve generated Swagger as a JSON endpoint
                app.UseSwagger();
                //Enable middleware to serve swagger - ui assets(HTML, JS, CSS etc.)
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint(_appConfiguration["App:SwaggerEndPoint"], "BusinessSolution API V1");
                    options.IndexStream = () => Assembly.GetExecutingAssembly()
                        .GetManifestResourceStream("Karion.BusinessSolution.Web.wwwroot.swagger.ui.index.html");
                    options.InjectBaseUrl(_appConfiguration["App:WebSiteRootAddress"]);
                }); //URL: /swagger
            }
            
        }
    }
}
