using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Karion.BusinessSolution.Authorization.Users;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using FirebaseAdmin.Messaging;
using Karion.BusinessSolution.QuanLyDanhMuc; // Thêm dòng này!
using Microsoft.EntityFrameworkCore;

public class AttendanceNotificationHostedService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public AttendanceNotificationHostedService(IServiceScopeFactory scopeFactory, IUnitOfWorkManager unitOfWorkManager)
    {
        _scopeFactory = scopeFactory;
        _unitOfWorkManager = unitOfWorkManager;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var now = TimeZoneInfo.ConvertTime(DateTime.Now,
                    TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));

                if (now.Hour == 7 && now.Minute == 15)
                {
                    await SendAttendanceNotification("morning");
                }
                if (now.Hour == 17 && now.Minute == 20)
                {
                    await SendAttendanceNotification("evening");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AttendanceNotificationHostedService error: {ex}");
            }
            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }

    private async Task SendAttendanceNotification(string type)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            // Bắt đầu UnitOfWork thủ công!
            using (var uow = _unitOfWorkManager.Begin())
            {
                try
                {
                    var serviceProvider = scope.ServiceProvider;
                    var nguoiBenhRepository = serviceProvider.GetRequiredService<IRepository<NguoiBenh>>();

                    string title, body;
                    if (type == "morning")
                    {
                        title = "Thông báo điểm danh";
                        body = "Đã đến giờ điểm danh, vui lòng điểm danh vào giờ làm!";
                    }
                    else if (type == "evening")
                    {
                        title = "Thông báo điểm danh";
                        body = "Vui lòng chấm công giờ về!";
                    }
                    else
                    {
                        return;
                    }

                    // Truy vấn và gửi thông báo hoàn toàn trong scope + UnitOfWork
                    var deviceTokens = await nguoiBenhRepository.GetAll()
                        .Where(x => x.IsNhanVien && !string.IsNullOrEmpty(x.DeviceToken))
                        .Select(x => x.DeviceToken)
                        .ToListAsync();

                    Console.WriteLine($"[DEBUG] {DateTime.Now}: Got {deviceTokens.Count} tokens in scope");

                    int sendSuccess = 0, sendFail = 0;
                    foreach (var token in deviceTokens)
                    {
                        try
                        {
                            await FirebaseHelper.SendFCM(token, title, body);
                            sendSuccess++;
                        }
                        catch (FirebaseMessagingException fcmEx)
                        {
                            sendFail++;
                            Console.WriteLine($"[DEBUG] FCM error token: {token}, error: {fcmEx.Message}");
                            
                            if (fcmEx.Message.Contains("Requested entity was not found"))
                            {
                                await RemoveDeviceTokenFromDb(token);
                            }
                        }
                        catch (Exception ex)
                        {
                            sendFail++;
                            Console.WriteLine($"[DEBUG] FCM error (other): {ex}");
                        }
                    }
                    Console.WriteLine($"[{DateTime.Now}] {type}: Success {sendSuccess}, Fail {sendFail}");

                    await uow.CompleteAsync();
                }
                catch (Exception repoEx)
                {
                    Console.WriteLine($"[DEBUG] Repo scope error: {repoEx}");
                    throw;
                }
            }
        }
    }
    private async Task RemoveDeviceTokenFromDb(string deviceToken)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var nguoiBenhRepository = scope.ServiceProvider.GetRequiredService<IRepository<NguoiBenh>>();
            var nguoiBenh = await nguoiBenhRepository
                .FirstOrDefaultAsync(x => x.DeviceToken == deviceToken);

            if (nguoiBenh != null)
            {
                nguoiBenh.DeviceToken = null;
                await nguoiBenhRepository.UpdateAsync(nguoiBenh);
            }
        }
    }
}