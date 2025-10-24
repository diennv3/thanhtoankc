using System;
using DlibDotNet;
using DlibDotNet.Dnn;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net.Http;
using Abp.Linq.Extensions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Net.Mail;
using Abp.UI;
using DlibDotNet.Extensions;
using Karion.BusinessSolution.Authorization.Users;
using Karion.BusinessSolution.Dto;
using Karion.BusinessSolution.Extension.Karion.BusinessSolution.Common;
using Karion.BusinessSolution.MultiTenancy;
using Karion.BusinessSolution.QuanLyDanhMuc;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.VersionControl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Karion.BusinessSolution.QuanLyDiemDanh;
using Karion.BusinessSolution.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Karion.BusinessSolution.MobileAppServices.MobileAppServices
{
    public class MobileAppServices : BusinessSolutionAppServiceBase, IMobileAppServices
    {
        private readonly IRepository<NguoiBenh> _nguoiBenhRepository;
        private readonly IRepository<Attendance> _attendanceRepository;
        private readonly IRepository<NguoiThan> _nguoiThanRepository;
        private readonly IRepository<ThongTinDonVi> _thongTinDonViRepository;
        private readonly IRepository<User, long> _userRepository;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IRepository<ChuyenKhoa> _chuyenKhoaRepository;
        private readonly IRepository<DichVu> _dichVuRepository;
        private readonly IRepository<BacSiChuyenKhoa> _bacSiChuyenKhoaRepository;
        private readonly IRepository<BacSiDichVu> _bacSiDichVuRepository;
        private readonly IRepository<GiaDichVu> _giaDichVuRepository;
        private readonly IRepository<DanhSachVersion> _danhSachVersionRepository;
        private readonly IRepository<LichHenKham> _lichHenKhamRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IEmailSender _emailSender;
        private readonly IRepository<ThongTinBacSiMoRong> _thongTinBacSiMoRongRepository;
        private readonly IHanetAppservices _hanetAppservices;
        private readonly IRepository<NguoiBenhNotification, long> _nguoiBenhNotificationRepository;
        private readonly IRepository<BinaryObject, Guid> _binaryObjectRepository;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer _timer;
        private bool _isRunning;
        private readonly IRepository<Shift> _shiftRepository;
        public MobileAppServices(IRepository<NguoiBenh> nguoiBenhRepository,
            IRepository<NguoiThan> nguoiThanRepository,
            IRepository<ThongTinDonVi> thongTinDonViRepository,
            IRepository<Attendance> attendanceRepository,
            IRepository<User, long> userRepository,
            IRepository<Tenant> tenantRepository,
            IRepository<ChuyenKhoa> chuyenKhoaRepository,
            IUnitOfWorkManager unitOfWorkManager,
            IRepository<DichVu> dichVuRepository,
            IRepository<BacSiChuyenKhoa> bacSiChuyenKhoaRepository,
            IRepository<BacSiDichVu> bacSiDichVuRepository,
            IRepository<GiaDichVu> giaDichVuRepository,
            IRepository<DanhSachVersion> danhSachVersionRepository,
            IEmailSender emailSender,
            IRepository<LichHenKham> lichHenKhamRepository,
            IRepository<ThongTinBacSiMoRong> thongTinBacSiMoRongRepository,
            IHanetAppservices hanetAppservices,
            IBinaryObjectManager binaryObjectManager,
            IRepository<BinaryObject, Guid> binaryObjectRepository,
            IRepository<NguoiBenhNotification, long> nguoiBenhNotificationRepository,
            IServiceScopeFactory scopeFactory,
            IRepository<Shift> shiftRepository
        )
        {
            _shiftRepository = shiftRepository;
            _nguoiBenhRepository = nguoiBenhRepository;
            _nguoiThanRepository = nguoiThanRepository;
            _attendanceRepository = attendanceRepository;
            _thongTinDonViRepository = thongTinDonViRepository;
            _userRepository = userRepository;
            _tenantRepository = tenantRepository;
            _chuyenKhoaRepository = chuyenKhoaRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _dichVuRepository = dichVuRepository;
            _bacSiChuyenKhoaRepository = bacSiChuyenKhoaRepository;
            _bacSiDichVuRepository = bacSiDichVuRepository;
            _giaDichVuRepository = giaDichVuRepository;
            _emailSender = emailSender;
            _lichHenKhamRepository = lichHenKhamRepository;
            _danhSachVersionRepository = danhSachVersionRepository;
            _thongTinBacSiMoRongRepository = thongTinBacSiMoRongRepository;
            _hanetAppservices = hanetAppservices;
            _nguoiBenhNotificationRepository = nguoiBenhNotificationRepository;
            _binaryObjectManager = binaryObjectManager;
            _binaryObjectRepository = binaryObjectRepository;
            _scopeFactory = scopeFactory;
        }

        private MobileDto.CheckTokenDto CheckValidToken(string token, int id, string username)
        {
            NguoiBenh nguoiBenh = _nguoiBenhRepository.FirstOrDefault(
                p => p.Token.ToLower().Equals(token.ToLower()) && p.Id == id
            );
            if (nguoiBenh.isNull())
            {
                return new MobileDto.CheckTokenDto()
                {
                    status = false,
                    errorCode = 401,
                    message = "Hết phiên đăng nhập! Vui lòng đăng nhập lại."
                };
            }

            if (!nguoiBenh.UserName.ToLower().clearSpace().Equals(username.ToLower().clearSpace()))
            {
                return new MobileDto.CheckTokenDto()
                {
                    status = false,
                    errorCode = 402,
                    message = "Invalid username!"
                };
            }

            if (nguoiBenh.TokenExpire < DateTime.Now)
            {
                return new MobileDto.CheckTokenDto()
                {
                    status = false,
                    errorCode = 403,
                    message = "Hết phiên đăng nhập! Vui lòng đăng nhập lại."
                };
            }

            return new MobileDto.CheckTokenDto()
            {
                message = "#N/A",
                status = true,
                errorCode = 0,
                userid = nguoiBenh.Id
            };
        }

        private async Task<NguoiBenh> GetLoginResultAsync(string usernameOrEmailAddress,
            string password)
        {
            string t = password.ToMd5();
            var nguoibenh = await _nguoiBenhRepository.FirstOrDefaultAsync(p =>
                (p.UserName.ToLower().Equals(usernameOrEmailAddress) ||
                 p.EmailAddress.ToLower().Equals(usernameOrEmailAddress)) && p.Password.ToLower().Equals(t.ToLower()) &&
                p.IsActive);

            return nguoibenh;
        }

        [HttpPost]
        //[MobileAuthorization]
        public async Task<MobileDto.MobileLoginResult> MobileLogin(MobileDto.MobileLoginModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.AccountName) || string.IsNullOrEmpty(model.PassWord))
                {
                    {
                        return new MobileDto.MobileLoginResult()
                        {
                            status = false,
                            errorCode = 404,
                            message = "Thông tin tài khoản hoặc mật khẩu không chính xác!",
                            token = "",
                            userid = 0,
                            username = ""
                        };
                    }
                }

                var loginResult = await GetLoginResultAsync(model.AccountName, model.PassWord);

                if (loginResult != null)
                {
                    //string accesstoken = DateTime.Now.ToLongDateString().ToMd5().ToLower();
                    long unixTime = ((DateTimeOffset) DateTime.Now).ToUnixTimeSeconds();
                    var saltString = new StringBuilder();
                    saltString.Append(model.AccountName);
                    saltString.Append(unixTime.ToString());
                    string accesstoken = saltString.ToString().ToMd5().ToLower();
                    loginResult.Token = accesstoken;
                    loginResult.TokenExpire = DateTime.Now.AddDays(7);
                    await _nguoiBenhRepository.UpdateAsync(loginResult);
                    return new MobileDto.MobileLoginResult()
                    {
                        status = true,
                        errorCode = 0,
                        token = accesstoken,
                        username = loginResult.UserName,
                        userid = loginResult.Id,
                        IsNhanVien = loginResult.IsNhanVien,
                        message = "Thành công!"
                    };
                }
                else
                {
                    return new MobileDto.MobileLoginResult()
                    {
                        status = false,
                        token = "",
                        userid = 0,
                        username = "",
                        message = "Thông tin tài khoản hoặc mật khẩu không chính xác!",
                        errorCode = 404
                    };
                }
            }
            catch (Exception ex)
            {
                return new MobileDto.MobileLoginResult()
                {
                    status = false,
                    token = "",
                    userid = 0,
                    username = "",
                    message = "Thất bại, Có lỗi xảy ra",
                    errorCode = 400
                };
            }
        }
        
        [HttpGet]
        public async Task<String> Verify(string code)
        {
            NguoiBenh nguoiBenh =
                await _nguoiBenhRepository.FirstOrDefaultAsync(p =>
                    p.EmailConfirmationCode.Equals(code) && !p.IsEmailConfirmed);
            if (!nguoiBenh.isNull())
            {
                var confirmationCode = (nguoiBenh.UserName.clearSpace().ToLower() + nguoiBenh.Password).ToMd5();
                if (confirmationCode.Equals(code))
                {
                    nguoiBenh.IsEmailConfirmed = true;
                    nguoiBenh.IsActive = true;
                    await _nguoiBenhRepository.UpdateAsync(nguoiBenh);
                    return "Kích hoạt tài khoản " + nguoiBenh.UserName + "Thành công!";
                }
            }

            return "Mã kích hoạt không hợp lệ!";
        }

        [HttpPost]
        public async Task<MobileDto.CommonResponse> Register(MobileDto.MobileRegisterDto input)
        {
            input.userName = input.userName.clearSpace().ToLower();
            if (!_nguoiBenhRepository.FirstOrDefault(p => p.UserName.Equals(input.userName)).isNull())
            {
                return new MobileDto.CommonResponse()
                {
                    status = false,
                    message = "Username đã được đăng ký",
                    errorCode = 405
                };
            }

            if (!_nguoiBenhRepository.FirstOrDefault(p => p.PhoneNumber.Equals(input.phoneNumber)).isNull())
            {
                return new MobileDto.CommonResponse()
                {
                    status = false,
                    message = "Số điện thoại đã được đăng ký",
                    errorCode = 406
                };
            }

            // if (!_nguoiBenhRepository.FirstOrDefault(p => p.EmailAddress.Equals(input.emailAddress)).isNull())
            // {
            //     return new MobileDto.CommonResponse()
            //     {
            //         status = false,
            //         message = "Email đã được đăng ký",
            //         errorCode = 407
            //     };
            // }

            try
            {
                var confirmationCode = (input.userName.clearSpace().ToLower() + input.password.ToMd5()).ToMd5();
                await _nguoiBenhRepository.InsertAsync(new NguoiBenh()
                {
                    Password = input.password.ToMd5().ToLower(),
                    NgaySinh = input.ngaySinh,
                    ThangSinh = input.thangSinh,
                    NamSinh = input.namSinh,
                    DiaChi = input.diaChi,
                    GioiTinh = input.gioiTinh,
                    CreationTime = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false,
                    IsPhoneNumberConfirmed = false,
                    EmailAddress = input.emailAddress.isNull() ? "" : input.emailAddress,
                    PhoneNumber = input.phoneNumber,
                    TokenExpire = DateTime.Now,
                    UserName = input.userName,
                    AccessFailedCount = 0,
                    HoVaTen = input.hoVaTen,
                    IsEmailConfirmed = false,
                    EmailConfirmationCode = confirmationCode,
                    SoTheBHYT = input.SoTheBHYT,
                    NoiDkBanDau = input.NoiDkBanDau,
                    GiaTriSuDungTuNgay = input.GiaTriSuDungTuNgay,
                    IsNhanVien = false
                }); 
                //await _emailSender.SendAsync(
                //    input.emailAddress,
                //    "Hệ thống Đăng ký Đặt lịch khám online",
                //    "Xin cám ơn bạn đã đăng ký tài khoản, hãy click vào đường link dưới đây để hoàn tất đăng ký và kích hoạt tài khoản: <a href='https://ttkc.techber.vn/api/services/app/MobileAppServices/Verify?code=" +
                //    confirmationCode +
                //    "' target='_blank'>https://ttkc.techber.vn/api/services/app/MobileAppServices/Verify?code=" +
                //    confirmationCode + "</a>"
                //);
                return new MobileDto.CommonResponse()
                {
                    status = true,
                    message = "Thành công!",
                    errorCode = 0
                };
            }
            catch (Exception e)
            {
                return new MobileDto.CommonResponse()
                {
                    status = false,
                    message = e.Message,
                    errorCode = 400
                };
            }
        }

        // [HttpPost]
        // public async Task<MobileDto.CommonResponse> UpdateInformation(MobileDto.MobileRegisterDto input)
        // {
        //     input.userName = input.userName.clearSpace().ToLower();
        //     if (!_nguoiBenhRepository.FirstOrDefault(p => p.UserName.Equals(input.userName)).isNull())
        //     {
        //         return new MobileDto.CommonResponse()
        //         {
        //             status = false,
        //             message = "Username đã được đăng ký"
        //         };
        //     }
        //
        //     if (!_nguoiBenhRepository.FirstOrDefault(p => p.PhoneNumber.Equals(input.phoneNumber)).isNull())
        //     {
        //         return new MobileDto.CommonResponse()
        //         {
        //             status = false,
        //             message = "Số điện thoại đã được đăng ký"
        //         };
        //     }
        //
        //     if (!_nguoiBenhRepository.FirstOrDefault(p => p.EmailAddress.Equals(input.emailAddress)).isNull())
        //     {
        //         return new MobileDto.CommonResponse()
        //         {
        //             status = false,
        //             message = "Email đã được đăng ký"
        //         };
        //     }
        //
        //     var confirmationCode = (input.userName.clearSpace().ToLower() + input.password.ToMd5()).ToMd5();
        //     await _nguoiBenhRepository.InsertAsync(new NguoiBenh()
        //     {
        //         Password = input.password.ToMd5(),
        //         NgaySinh = input.ngaySinh,
        //         ThangSinh = input.thangSinh,
        //         NamSinh = input.namSinh,
        //         DiaChi = input.diaChi,
        //         GioiTinh = input.gioiTinh,
        //         CreationTime = DateTime.Now,
        //         IsActive = true,
        //         IsDeleted = false,
        //         IsPhoneNumberConfirmed = false,
        //         EmailAddress = input.emailAddress,
        //         PhoneNumber = input.phoneNumber,
        //         TokenExpire = DateTime.Now,
        //         UserName = input.userName,
        //         AccessFailedCount = 0,
        //         HoVaTen = input.hoVaTen,
        //         IsEmailConfirmed = false,
        //         EmailConfirmationCode = confirmationCode,
        //     });
        //     //await _emailSender.SendAsync(
        //     //    input.emailAddress,
        //     //    "Hệ thống Đăng ký Đặt lịch khám online",
        //     //    "Xin cám ơn bạn đã đăng ký tài khoản, hãy click vào đường link dưới đây để hoàn tất đăng ký và kích hoạt tài khoản: <a href='https://ttkc.techber.vn/api/services/app/MobileAppServices/Verify?code=" +
        //     //    confirmationCode +
        //     //    "' target='_blank'>https://ttkc.techber.vn/api/services/app/MobileAppServices/Verify?code=" +
        //     //    confirmationCode + "</a>"
        //     //);
        //     return new MobileDto.CommonResponse()
        //     {
        //         status = true,
        //         message = "Thành công, vui lòng kiểm tra email để hoàn tất đăng ký và kích hoạt tài khoản"
        //     };
        // }
        [HttpPost]
        public async Task<MobileDto.GetTenantsResultDto> GetTenants(MobileDto.GetMenuDto input)
        {
            MobileDto.CheckTokenDto checkTokenDto = CheckValidToken(input.token, input.userid, input.username);
            if (checkTokenDto.status)
            {
                List<Tenant> tenants = _tenantRepository.GetAll().WhereIf(true, p => p.IsActive && p.Id != 1).ToList();
                var result = new List<MobileDto.TenantDto>();
                foreach (var VARIABLE in tenants)
                {
                    result.Add(new MobileDto.TenantDto()
                    {
                        tenantId = VARIABLE.Id,
                        tenantName = VARIABLE.Name
                    });
                }

                return new MobileDto.GetTenantsResultDto()
                {
                    status = true,
                    message = "Thành công",
                    tenants = result,
                    errorCode = 0
                };
            }
            else
            {
                return new MobileDto.GetTenantsResultDto()
                {
                    status = false,
                    message = checkTokenDto.message,
                    tenants = new List<MobileDto.TenantDto>(),
                    errorCode = checkTokenDto.errorCode
                };
            }
        }

        [HttpPost]
        public async Task<MobileDto.GetChuyenKhoaResultDto> GetChuyenKhoa(MobileDto.GetChuyenKhoaDto input)
        {
            MobileDto.CheckTokenDto checkTokenDto = CheckValidToken(input.token, input.userid, input.username);
            if (checkTokenDto.status)
            {
                _unitOfWorkManager.Current.SetTenantId(input.tenantId); //switch tenant sang bệnh viện tương ứng
                var filteredChuyenKhoas = _chuyenKhoaRepository.GetAll();
                var pagedAndFilteredChuyenKhoas = filteredChuyenKhoas
                    .OrderBy("id asc");
                var chuyenKhoas = from o in pagedAndFilteredChuyenKhoas
                    select new ChuyenKhoaDto
                    {
                        Ten = o.Ten,
                        MoTa = o.MoTa,
                        Id = o.Id
                    };

                return new MobileDto.GetChuyenKhoaResultDto()
                {
                    status = true,
                    message = "Thành công",
                    ChuyenKhoa = await chuyenKhoas.ToListAsync(),
                    errorCode = 0
                };
            }
            else
            {
                return new MobileDto.GetChuyenKhoaResultDto()
                {
                    status = false,
                    message = checkTokenDto.message,
                    ChuyenKhoa = new List<ChuyenKhoaDto>(),
                    errorCode = checkTokenDto.errorCode
                };
            }
        }

        [HttpPost]
        public async Task<MobileDto.GetDichVuResultDto> GetDichVu(MobileDto.GetChuyenKhoaDto input)
        {
            MobileDto.CheckTokenDto checkTokenDto = CheckValidToken(input.token, input.userid, input.username);
            if (checkTokenDto.status)
            {
                _unitOfWorkManager.Current.SetTenantId(input.tenantId); //switch tenant sang bệnh viện tương ứng
                var filteredChuyenKhoas = _dichVuRepository.GetAll();
                var pagedAndFilteredChuyenKhoas = filteredChuyenKhoas
                    .OrderBy("id asc");
                var chuyenKhoas = from o in pagedAndFilteredChuyenKhoas
                    select new DichVuDto()
                    {
                        Ten = o.Ten,
                        MoTa = o.MoTa,
                        Id = o.Id
                    };

                return new MobileDto.GetDichVuResultDto()
                {
                    status = true,
                    message = "Thành công",
                    DichVu = await chuyenKhoas.ToListAsync(),
                    errorCode = 0
                };
            }
            else
            {
                return new MobileDto.GetDichVuResultDto()
                {
                    status = false,
                    message = checkTokenDto.message,
                    DichVu = new List<DichVuDto>(),
                    errorCode = checkTokenDto.errorCode
                };
            }
        }

        [HttpPost]
        public async Task<MobileDto.GetThongTinChuyenKhoaResultDto> GetThongTinChuyenKhoa(
            MobileDto.GetThongTinChuyenKhoaDto input)
        {
            MobileDto.CheckTokenDto checkTokenDto = CheckValidToken(input.token, input.userid, input.username);
            if (checkTokenDto.status)
            {
                _unitOfWorkManager.Current.SetTenantId(input.tenantId); //switch tenant sang bệnh viện tương ứng
                var chuyenKhoa = await _chuyenKhoaRepository.FirstOrDefaultAsync(input.chuyenKhoaId);
                if (chuyenKhoa.isNull())
                {
                    return new MobileDto.GetThongTinChuyenKhoaResultDto()
                    {
                        status = false,
                        message = "Không tìm thấy chuyên khoa tương ứng!",
                        data = null,
                        danhSachBacSi = new List<MobileDto.ThongTinBacSiDto>(),
                        errorCode = 400
                    };
                }

                var filteredBacSiChuyenKhoas = _bacSiChuyenKhoaRepository.GetAll()
                    .WhereIf(true, p => p.ChuyenKhoaId.Equals(chuyenKhoa.Id));
                var pagedAndFilteredBacSiChuyenKhoas = filteredBacSiChuyenKhoas
                    .OrderBy("id asc");
                var bacSiChuyenKhoas = from o in pagedAndFilteredBacSiChuyenKhoas
                    join o1 in _userRepository.GetAll() on o.UserId equals o1.Id into j1
                    from s1 in j1.DefaultIfEmpty()
                    select new MobileDto.ThongTinBacSiDto()
                    {
                        Name = s1.Name,
                        Surname = s1.Surname,
                        EmailAddress = s1.EmailAddress,
                        PhoneNumber = s1.PhoneNumber,
                        Id = s1.Id
                    };

                return new MobileDto.GetThongTinChuyenKhoaResultDto()
                {
                    status = true,
                    message = "Thành công",
                    errorCode = 0,
                    data = new ChuyenKhoaDto()
                    {
                        Id = chuyenKhoa.Id,
                        Ten = chuyenKhoa.Ten,
                        MoTa = chuyenKhoa.MoTa
                    },
                    danhSachBacSi = await bacSiChuyenKhoas.ToListAsync()
                };
            }
            else
            {
                return new MobileDto.GetThongTinChuyenKhoaResultDto()
                {
                    status = false,
                    message = checkTokenDto.message,
                    errorCode = checkTokenDto.errorCode,
                    data = null,
                    danhSachBacSi = new List<MobileDto.ThongTinBacSiDto>()
                };
            }
        }

        [HttpPost]
        public async Task<MobileDto.GetThongTinDichVuResultDto> GetThongTinDichVu(MobileDto.GetThongTinDichVuDto input)
        {
            MobileDto.CheckTokenDto checkTokenDto = CheckValidToken(input.token, input.userid, input.username);
            if (checkTokenDto.status)
            {
                _unitOfWorkManager.Current.SetTenantId(input.tenantId); //switch tenant sang bệnh viện tương ứng
                var dichVu = await _dichVuRepository.FirstOrDefaultAsync(input.dichVuId);
                if (dichVu.isNull())
                {
                    return new MobileDto.GetThongTinDichVuResultDto()
                    {
                        status = false,
                        message = "Không tìm thấy dịch vụ tương ứng!",
                        errorCode = 400,
                        data = null,
                        danhSachBacSi = new List<MobileDto.ThongTinBacSiDto>()
                    };
                }

                var filteredBacSiChuyenKhoas = _bacSiDichVuRepository.GetAll()
                    .WhereIf(true, p => p.DichVuId.Equals(dichVu.Id));
                var pagedAndFilteredBacSiChuyenKhoas = filteredBacSiChuyenKhoas
                    .OrderBy("id asc");
                var bacSiChuyenKhoas = from o in pagedAndFilteredBacSiChuyenKhoas
                    join o1 in _userRepository.GetAll() on o.UserId equals o1.Id into j1
                    from s1 in j1.DefaultIfEmpty()
                    select new MobileDto.ThongTinBacSiDto()
                    {
                        Name = s1.Name,
                        Surname = s1.Surname,
                        EmailAddress = s1.EmailAddress,
                        PhoneNumber = s1.PhoneNumber,
                        Id = s1.Id
                    };

                var giaDichVu = await _giaDichVuRepository.GetAll()
                    .WhereIf(true, p => p.DichVuId.Equals(dichVu.Id) && p.NgayApDung <= DateTime.Now)
                    .OrderByDescending(p => p.NgayApDung).FirstOrDefaultAsync(); // lay giá của ngày áp dụng gần nhất

                return new MobileDto.GetThongTinDichVuResultDto()
                {
                    status = true,
                    message = "Thành công",
                    errorCode = 0,
                    data = new DichVuDto()
                    {
                        Id = dichVu.Id,
                        Ten = dichVu.Ten,
                        MoTa = dichVu.MoTa
                    },
                    danhSachBacSi = await bacSiChuyenKhoas.ToListAsync(),
                    GiaDichVu = (giaDichVu.isNull()) ? null : ObjectMapper.Map<GiaDichVuDto>(giaDichVu)
                };
            }
            else
            {
                return new MobileDto.GetThongTinDichVuResultDto()
                {
                    status = false,
                    message = checkTokenDto.message,
                    errorCode = checkTokenDto.errorCode,
                    data = null,
                    danhSachBacSi = new List<MobileDto.ThongTinBacSiDto>(),
                    GiaDichVu = null
                };
            }
        }

        [HttpPost]
        public async Task<MobileDto.CommonResponse> DangKyNguoiThan(MobileDto.DangKyNguoiThanDto input)
        {
            MobileDto.CheckTokenDto checkTokenDto = CheckValidToken(input.token, input.userid, input.username);
            if (checkTokenDto.status)
            {
                await _nguoiThanRepository.InsertAsync(new NguoiThan()
                {
                    Tuoi = !input.tuoi.isNull() ? input.tuoi : 0,
                    CreationTime = DateTime.Now,
                    DiaChi = input.diaChi,
                    GioiTinh = input.gioiTinh,
                    IsDeleted = false,
                    HoVaTen = input.hoVaTen,
                    MoiQuanHe = input.moiQuanHe,
                    NguoiBenhId = checkTokenDto.userid,
                    SoDienThoai = input.soDienThoai
                });
                return new MobileDto.CommonResponse()
                {
                    status = true,
                    message = "Đăng ký thành công!",
                    errorCode = 0
                };
            }
            else
            {
                return new MobileDto.CommonResponse()
                {
                    status = false,
                    message = checkTokenDto.message,
                    errorCode = checkTokenDto.errorCode
                };
            }
        }

        [HttpPost]
        public async Task<MobileDto.GetDanhSachNguoiThanDto> GetDanhSachNguoiThan(MobileDto.GetMenuDto input)
        {
            MobileDto.CheckTokenDto checkTokenDto = CheckValidToken(input.token, input.userid, input.username);
            if (checkTokenDto.status)
            {
                List<NguoiThan> nguoiThans =
                    await _nguoiThanRepository.GetAllListAsync(p => p.NguoiBenhId.Equals(checkTokenDto.userid));
                List<NguoiThanDto> nguoiThanDtos = ObjectMapper.Map<List<NguoiThanDto>>(nguoiThans);
                return new MobileDto.GetDanhSachNguoiThanDto()
                {
                    status = true,
                    errorCode = 0,
                    message = "",
                    nguoiThan = nguoiThanDtos
                };
            }
            else
            {
                return new MobileDto.GetDanhSachNguoiThanDto()
                {
                    status = false,
                    message = checkTokenDto.message,
                    errorCode = checkTokenDto.errorCode,
                    nguoiThan = new List<NguoiThanDto>()
                };
            }
        }

        [HttpPost]
        public async Task<MobileDto.CommonResponse> DangKyKham(MobileDto.DangKyKhamDto input)
        {
            using (var uow = UnitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
            {
                try
                {
                    MobileDto.CheckTokenDto checkTokenDto =
                        CheckValidToken(input.token, input.userid, input.username);
                    if (checkTokenDto.status)
                    {
                        using (_unitOfWorkManager.Current.SetTenantId(input.tenantId)
                        ) //switch tenant sang bệnh viện tương ứng
                        {
                            await _lichHenKhamRepository.InsertAsync(new LichHenKham()
                                {
                                    CreationTime = DateTime.Now,
                                    IsDeleted = false,
                                    TenantId = input.tenantId,
                                    BacSiId = (!input.bacSiId.isNull() && input.bacSiId > 0) ? input.bacSiId : null,
                                    ChuyenKhoaId = input.chuyenKhoaId,
                                    IsDaKham = false,
                                    NgayHenKham = input.ngayHenKham,
                                    NguoiThanId = (!input.nguoiThanId.isNull() && input.nguoiThanId > 0)
                                        ? input.nguoiThanId
                                        : null,
                                    MoTaTrieuChung = input.moTaTrieuChung,
                                    IsDaThanhToan = false,
                                    IsCoBHYT = input.IsCoBHYT,
                                    PhuongThucThanhToan = 2,
                                    BHYTValidDate = DateTime.Now,
                                    NoiDangKyKCBDauTien = input.NoiDangKyKCBDauTien,
                                    SoTheBHYT = input.SoTheBHYT,
                                    NguoiBenhId = checkTokenDto.userid,
                                    Flag = 1,
                                    KhungKham = (!input.KhungKham.isNull() && input.KhungKham > 0)
                                        ? input.KhungKham
                                        : null
                                });

                            var thongTinDonVi =
                                await _thongTinDonViRepository.FirstOrDefaultAsync(p => p.Key.Equals("Address"));
                            if (thongTinDonVi.isNull())
                            {
                                throw new Exception("Thông tin đơn vị chưa có cấu hình địa chỉ");
                            }
                            else
                            {
                                var diaChi = thongTinDonVi.Value ?? "";

                                var nguoiBenhNoti = new NguoiBenhNotification();
                                nguoiBenhNoti.NoiDungTinNhan =
                                    $"Bạn có lịch hẹn khám vào ngày {input.ngayHenKham.ToShortDateString()} tại cơ sở {diaChi}";
                                nguoiBenhNoti.ThoiGianGui = DateTime.Now;
                                nguoiBenhNoti.TieuDe = "Thông tin đặt lịch khám";
                                nguoiBenhNoti.CreationTime = DateTime.Now;
                                nguoiBenhNoti.CreatorUserId = 1;
                                nguoiBenhNoti.NguoiBenhId = input.userid;

                                await _nguoiBenhNotificationRepository.InsertAsync(nguoiBenhNoti);
                            }
                            
                            await uow.CompleteAsync();

                            return new MobileDto.CommonResponse()
                            {
                                status = true,
                                message = "Thành công",
                                errorCode = 0
                            };
                        }
                    }
                    else
                    {
                        await uow.CompleteAsync();
                        return new MobileDto.CommonResponse()
                        {
                            status = false,
                            message = checkTokenDto.message,
                            errorCode = checkTokenDto.errorCode
                        };
                    }
                }
                catch (Exception e)
                {
                    return new MobileDto.CommonResponse()
                    {
                        status = false,
                        message = "Có lỗi xảy ra: "+ e.Message,
                        errorCode = 400
                    };
                }
            }
        }

        protected async Task<List<MobileDto.LichHenKhamContainer>> ProcessLichHenKham(List<int> tenants,
            MobileDto.GetDanhSachDangKyKhamDto input)
        {
            List<MobileDto.LichHenKhamContainer> lichHenKhamContainers = new List<MobileDto.LichHenKhamContainer>();
            foreach (var VARIABLE1 in tenants)
            {
                Tenant tenant = _tenantRepository.FirstOrDefault(VARIABLE1);
                _unitOfWorkManager.Current.SetTenantId(VARIABLE1);
                var filterLichHenKhams = await _lichHenKhamRepository.GetAll()
                    .WhereIf(input.tuNgay.HasValue, p => p.NgayHenKham >= input.tuNgay)
                    .WhereIf(input.denNgay.HasValue, p => p.NgayHenKham <= input.denNgay)
                    .WhereIf(true, p => p.NguoiBenhId == input.userid)
                    .WhereIf(true, p => p.Flag != TechberConsts.FLAG_HUY_LHK)
                    .ToListAsync();
                
                //Lấy dữ liệu từ bảng Thông tin đơn vị
                var khamSession =
                    await _thongTinDonViRepository.FirstOrDefaultAsync(p => p.Key.Equals("KhamSession"));
                int khamSessionValue = Int32.Parse(khamSession.Value);
                var gioBatDauLamViecSang =
                    await _thongTinDonViRepository.FirstOrDefaultAsync(p => p.Key.Equals("GioBatDauLamViecSang"));
                var gioBatDauLamViecSangValue = gioBatDauLamViecSang.Value ?? "";
                var gioKetThucLamViecSang =
                    await _thongTinDonViRepository.FirstOrDefaultAsync(p => p.Key.Equals("GioKetThucLamViecSang"));
                var gioKetThucLamViecSangValue = gioKetThucLamViecSang.Value ?? "";
                var gioBatDauLamViecChieu =
                    await _thongTinDonViRepository.FirstOrDefaultAsync(p => p.Key.Equals("GioBatDauLamViecChieu"));
                var gioBatDauLamViecChieuValue = gioBatDauLamViecChieu.Value ?? "";
                var gioKetThucLamViecChieu =
                    await _thongTinDonViRepository.FirstOrDefaultAsync(p => p.Key.Equals("GioKetThucLamViecChieu"));
                var gioKetThucLamViecChieuValue = gioKetThucLamViecChieu.Value ?? "";
                
                //Tính thời gian làm việc
                string thoiGianBatDauLamViecSang = "06/18/2022 " + gioBatDauLamViecSangValue;
                string thoiGianKetThucLamViecSang = "06/18/2022 " + gioKetThucLamViecSangValue;
                string thoiGianBatDauLamViecChieu = "06/18/2022 " + gioBatDauLamViecChieuValue;
                string thoiGianKetThucLamViecChieu = "06/18/2022 " + gioKetThucLamViecChieuValue;
                TimeSpan tongThoiGianLamViecSang = Convert.ToDateTime(thoiGianKetThucLamViecSang) - Convert.ToDateTime(thoiGianBatDauLamViecSang);
                var tongThoiGianLamViecSangValue = (int)tongThoiGianLamViecSang.TotalMinutes;
                TimeSpan tongThoiGianLamViecChieu = Convert.ToDateTime(thoiGianKetThucLamViecChieu) - Convert.ToDateTime(thoiGianBatDauLamViecChieu);
                var tongThoiGianLamViecChieuValue = tongThoiGianLamViecChieu.TotalMinutes;
                
                foreach (var VARIABLE in filterLichHenKhams)
                {
                    var GioKham = "";
                    //Lấy giờ khám
                    if (VARIABLE.KhungKham <= (tongThoiGianLamViecSangValue / khamSessionValue))
                        //nếu thuộc khung khám sáng
                    {
                        DateTime GioKhamTemp = Convert.ToDateTime(thoiGianBatDauLamViecSang);
                        for (var khungKham = 1 ; khungKham <= tongThoiGianLamViecSangValue/khamSessionValue; khungKham ++)
                        {
                            if (khungKham == VARIABLE.KhungKham)
                            {
                                GioKham = GioKhamTemp.ToString("HH:mm");
                                break;
                            }
                            GioKhamTemp = GioKhamTemp.AddMinutes(15);
                        }
                    }
                    if ((tongThoiGianLamViecSangValue / khamSessionValue) < VARIABLE.KhungKham && 
                        VARIABLE.KhungKham <= ((tongThoiGianLamViecSangValue + tongThoiGianLamViecChieuValue)/khamSessionValue))
                   //nếu thuộc khung khám chiều
                    {
                        DateTime GioKhamTemp = Convert.ToDateTime(thoiGianBatDauLamViecChieu);
                        for (var khungKham = (tongThoiGianLamViecSangValue / khamSessionValue)+1;
                            khungKham <= (tongThoiGianLamViecSangValue + tongThoiGianLamViecChieuValue) /
                            khamSessionValue;
                            khungKham++)
                        {
                            if (khungKham == VARIABLE.KhungKham)
                            {
                                GioKham = GioKhamTemp.ToString("HH:mm");
                                break;
                            }
                            GioKhamTemp = GioKhamTemp.AddMinutes(15);
                        }
                    }


                    if (VARIABLE.ChuyenKhoaId.HasValue && VARIABLE.BacSiId.HasValue)
                    {
                        User user = await _userRepository.FirstOrDefaultAsync((long) VARIABLE.BacSiId);
                        ChuyenKhoa chuyenKhoa =
                            await _chuyenKhoaRepository.FirstOrDefaultAsync((int) VARIABLE.ChuyenKhoaId);
                        ThongTinBacSiMoRong thongTinBacSiMoRong =
                            await _thongTinBacSiMoRongRepository.FirstOrDefaultAsync(p => p.UserId == VARIABLE.BacSiId);
                        lichHenKhamContainers.Add(new MobileDto.LichHenKhamContainer()
                        {
                            NgayHenKham = VARIABLE.NgayHenKham,
                            MoTaTrieuChung = VARIABLE.MoTaTrieuChung,
                            IsCoBHYT = VARIABLE.IsCoBHYT,
                            SoTheBHYT = VARIABLE.SoTheBHYT,
                            NoiDangKyKCBDauTien = VARIABLE.NoiDangKyKCBDauTien,
                            BHYTValidDate = VARIABLE.BHYTValidDate,
                            PhuongThucThanhToan = VARIABLE.PhuongThucThanhToan,
                            IsDaKham = VARIABLE.IsDaKham,
                            IsDaThanhToan = VARIABLE.IsDaThanhToan,
                            TimeHoanThanhKham = VARIABLE.TimeHoanThanhKham,
                            TimeHoanThanhThanhToan = VARIABLE.TimeHoanThanhThanhToan,
                            ChiDinhDichVuSerialize = VARIABLE.ChiDinhDichVuSerialize,
                            Flag = VARIABLE.Flag,
                            QRString = VARIABLE.QRString,
                            BacSiId = VARIABLE.BacSiId,
                            TongTienDaThanhToan = VARIABLE.TongTienDaThanhToan,
                            TienThua = VARIABLE.TienThua,
                            ThuNganId = VARIABLE.ThuNganId,
                            NguoiBenhId = VARIABLE.NguoiBenhId,
                            NguoiThanId = VARIABLE.NguoiThanId,
                            ChuyenKhoaId = VARIABLE.ChuyenKhoaId,
                            TenChuyenKhoa = chuyenKhoa.Ten,
                            MoTaChuyenKhoa = chuyenKhoa.MoTa,
                            Name = user.Name,
                            Surname = user.Surname,
                            EmailAddress = user.EmailAddress,
                            PhoneNumber = user.PhoneNumber,
                            TieuSu = thongTinBacSiMoRong.TieuSu,
                            ChucDanh = thongTinBacSiMoRong.ChucDanh,
                            Type = "Khám chuyên khoa",
                            TenantId = tenant.Id,
                            TenantName = tenant.Name,
                            LichHenKhamId = VARIABLE.Id,
                            KhungKham = VARIABLE.KhungKham,
                            GioKham = GioKham
                        });
                    }
                    else
                    {
                        ChuyenKhoa chuyenKhoa =
                            await _chuyenKhoaRepository.FirstOrDefaultAsync((int) VARIABLE.ChuyenKhoaId);
                        lichHenKhamContainers.Add(new MobileDto.LichHenKhamContainer()
                        {
                            NgayHenKham = VARIABLE.NgayHenKham,
                            MoTaTrieuChung = VARIABLE.MoTaTrieuChung,
                            IsCoBHYT = VARIABLE.IsCoBHYT,
                            SoTheBHYT = VARIABLE.SoTheBHYT,
                            NoiDangKyKCBDauTien = VARIABLE.NoiDangKyKCBDauTien,
                            BHYTValidDate = VARIABLE.BHYTValidDate,
                            PhuongThucThanhToan = VARIABLE.PhuongThucThanhToan,
                            IsDaKham = VARIABLE.IsDaKham,
                            IsDaThanhToan = VARIABLE.IsDaThanhToan,
                            TimeHoanThanhKham = VARIABLE.TimeHoanThanhKham,
                            TimeHoanThanhThanhToan = VARIABLE.TimeHoanThanhThanhToan,
                            ChiDinhDichVuSerialize = VARIABLE.ChiDinhDichVuSerialize,
                            Flag = VARIABLE.Flag,
                            QRString = VARIABLE.QRString,
                            BacSiId = VARIABLE.BacSiId,
                            TongTienDaThanhToan = VARIABLE.TongTienDaThanhToan,
                            TienThua = VARIABLE.TienThua,
                            ThuNganId = VARIABLE.ThuNganId,
                            NguoiBenhId = VARIABLE.NguoiBenhId,
                            NguoiThanId = VARIABLE.NguoiThanId,
                            ChuyenKhoaId = VARIABLE.ChuyenKhoaId,
                            TenChuyenKhoa = chuyenKhoa.Ten,
                            MoTaChuyenKhoa = chuyenKhoa.MoTa,
                            Name = null,
                            Surname = null,
                            EmailAddress = null,
                            PhoneNumber = null,
                            TieuSu = null,
                            ChucDanh = null,
                            Type = "Khám chuyên khoa",
                            TenantId = tenant.Id,
                            TenantName = tenant.Name,
                            LichHenKhamId = VARIABLE.Id,
                            KhungKham = VARIABLE.KhungKham,
                            GioKham = GioKham
                        });
                    }
                }
            }

            return lichHenKhamContainers.OrderByDescending(p => p.NgayHenKham).ToList();
        }

        [HttpPost]
        public async Task<MobileDto.GetDanhSachDangKyKhamResultDto> GetDanhSachDangKyKham(
            MobileDto.GetDanhSachDangKyKhamDto input)
        {
            MobileDto.CheckTokenDto checkTokenDto = CheckValidToken(input.token, input.userid, input.username);
            if (checkTokenDto.status)
            {
                List<LichHenKhamDto> lichHenKhamDtos = new List<LichHenKhamDto>();
                if (input.tenantId.HasValue)
                {
                    return new MobileDto.GetDanhSachDangKyKhamResultDto()
                    {
                        message = "Thành công",
                        errorCode = 0,
                        status = true,
                        lichHenKhamList = await ProcessLichHenKham(new List<int>()
                        {
                            (int) input.tenantId
                        }, input)
                    };
                }

                var tenants = await _tenantRepository.GetAll().Select(p => p.Id).ToListAsync();
                return new MobileDto.GetDanhSachDangKyKhamResultDto()
                {
                    message = "",
                    status = true,
                    errorCode = 0,
                    lichHenKhamList = await ProcessLichHenKham(tenants, input)
                };
            }
            else
            {
                return new MobileDto.GetDanhSachDangKyKhamResultDto()
                {
                    status = false,
                    message = checkTokenDto.message,
                    errorCode = checkTokenDto.errorCode
                };
            }
        }

        [HttpPost]
        public async Task<MobileDto.VersionDto> CheckVersion()
        {
            List<DanhSachVersion> danhSachVersion = await _danhSachVersionRepository.GetAllListAsync(p => p.IsActive);
            List<MobileDto.Version> versionDtos = new List<MobileDto.Version>();
            foreach (var VARIABLE in danhSachVersion)
            {
                versionDtos.Add(new MobileDto.Version()
                {
                    type = VARIABLE.Name,
                    version = VARIABLE.VersionNumber
                });
            }

            if (danhSachVersion.isEmpty())
            {
                return new MobileDto.VersionDto()
                {
                    status = false,
                    message = "Không có phiên bản hỗ trợ",
                    errorCode = 400
                };
            }

            return new MobileDto.VersionDto()
            {
                status = true,
                errorCode = 0,
                message = "Thành công",
                appVersions = versionDtos.ToList()
            };
        }

        protected ushort NapasQR_CRCHash(byte[] bytes)
        {
            const ushort poly = 4129;
            ushort[] table = new ushort[256];
            ushort initialValue = 0xffff;
            ushort temp, a;
            ushort crc = initialValue;
            for (int i = 0; i < table.Length; ++i)
            {
                temp = 0;
                a = (ushort) (i << 8);
                for (int j = 0; j < 8; ++j)
                {
                    if (((temp ^ a) & 0x8000) != 0)
                        temp = (ushort) ((temp << 1) ^ poly);
                    else
                        temp <<= 1;
                    a <<= 1;
                }

                table[i] = temp;
            }

            for (int i = 0; i < bytes.Length; ++i)
            {
                crc = (ushort) ((crc << 8) ^ table[((crc >> 8) ^ (0xff & bytes[i]))]);
            }

            return crc;
        }

        protected const string InitCodeDynamicQRToBankAccount_init = "000201010212";
        protected const string InitCodeDynamicQRToBankAccount_type = "QRIBFTTA";
        protected const string InitCodeDynamicQRToBankAccount_curency_VND = "704";
        protected const string AIDNapas = "A000000727";

        [HttpPost]
        public string PaymentQRGenerator(MobileDto.QRInputDto input)
        {
            string hashCodeQR = InitCodeDynamicQRToBankAccount_init;
            MobileDto.QRStructure bankAccountBlock = new MobileDto.QRStructure()
            {
                id = 38
            };
            MobileDto.QRStructure bankAccountBlock_napasAID = new MobileDto.QRStructure()
            {
                id = 0,
                data = AIDNapas
            };
            MobileDto.QRStructure bankAccountBlock_recieverBankInfo = new MobileDto.QRStructure()
            {
                id = 1
            };
            MobileDto.QRStructure bankAccountBlock_bankCode = new MobileDto.QRStructure()
            {
                id = 0,
                data = input.bankCode
            };
            MobileDto.QRStructure bankAccountBlock_bankAccount = new MobileDto.QRStructure()
            {
                id = 1,
                data = input.bankAccount
            };
            MobileDto.QRStructure bankAccountBlock_transferType = new MobileDto.QRStructure()
            {
                id = 2,
                data = InitCodeDynamicQRToBankAccount_type
            };
            MobileDto.QRStructure curency_block = new MobileDto.QRStructure()
            {
                id = 53,
                data = InitCodeDynamicQRToBankAccount_curency_VND
            };
            MobileDto.QRStructure amount_block = new MobileDto.QRStructure()
            {
                id = 54,
                data = input.amount.ToString()
            };
            MobileDto.QRStructure country_block = new MobileDto.QRStructure()
            {
                id = 58,
                data = "VN"
            };
            MobileDto.QRStructure note_block = new MobileDto.QRStructure()
            {
                id = 62,
                data = new MobileDto.QRStructure()
                {
                    id = 8,
                    data = "TBR-TTKC " + input.transactionUid + " " + input.noiDung
                }.dataString()
            };
            bankAccountBlock_recieverBankInfo.data = bankAccountBlock_bankCode.dataString() +
                                                     bankAccountBlock_bankAccount.dataString();
            bankAccountBlock.data =
                bankAccountBlock_napasAID.dataString() + bankAccountBlock_recieverBankInfo.dataString() +
                bankAccountBlock_transferType.dataString();
            hashCodeQR += bankAccountBlock.dataString()
                          + curency_block.dataString()
                          + amount_block.dataString()
                          + country_block.dataString()
                          + note_block.dataString();

            //CRC
            hashCodeQR += "6304";
            return hashCodeQR + NapasQR_CRCHash(Encoding.ASCII.GetBytes(hashCodeQR)).ToString("X");
        }

        public async Task<MobileDto.CommonResponse> DoiMatKhau(MobileDto.DoiMatKhauDto input)
        {
            MobileDto.CheckTokenDto checkTokenDto = CheckValidToken(input.token, input.userid, input.username);
            if (checkTokenDto.status)
            {
                try
                {
                    NguoiBenh nguoiBenh = await _nguoiBenhRepository.FirstOrDefaultAsync(p => p.Id == input.userid);
                    var oldPassword = input.OldPassWord.ToMd5().ToLower();
                    if (nguoiBenh.Password.ToLower() != oldPassword)
                    {
                        return new MobileDto.CommonResponse()
                        {
                            status = false,
                            message = "Mật khẩu cũ không đúng",
                            errorCode = 408
                        };
                    }
                    else
                    {
                        nguoiBenh.Password = input.NewPassWord.ToMd5().ToLower();
                        await _nguoiBenhRepository.UpdateAsync(nguoiBenh);
                        return new MobileDto.CommonResponse()
                        {
                            status = true,
                            message = "Thành công!",
                            errorCode = 0
                        };
                    }
                }
                catch (Exception e)
                {
                    return new MobileDto.CommonResponse()
                    {
                        status = false,
                        message = "Có lỗi xảy ra",
                        errorCode = 400
                    };
                }
            }
            else
            {
                return new MobileDto.CommonResponse()
                {
                    status = false,
                    message = checkTokenDto.message,
                    errorCode = checkTokenDto.errorCode
                };
            }
        }

        public async Task<MobileDto.CommonResponse> CapNhatThongTinNguoiBenh(
            MobileDto.CapNhatThongTinNguoiBenhDto input)
        {
            MobileDto.CheckTokenDto checkTokenDto = CheckValidToken(input.token, input.userid, input.username);
            if (checkTokenDto.status)
            {
                try
                {
                    NguoiBenh nguoiBenh = await _nguoiBenhRepository.FirstOrDefaultAsync(p => p.Id == input.userid);

                    nguoiBenh.HoVaTen = input.HoVaTen;
                    nguoiBenh.NgaySinh = input.NgaySinh;
                    nguoiBenh.GioiTinh = input.GioiTinh;
                    nguoiBenh.DiaChi = input.DiaChi;
                    nguoiBenh.PhoneNumber = input.PhoneNumber;
                    nguoiBenh.EmailAddress = input.EmailAddress.isNull() ? "" : input.EmailAddress;
                    nguoiBenh.ProfilePicture = input.ProfilePicture;
                    nguoiBenh.ThangSinh = input.ThangSinh;
                    nguoiBenh.NamSinh = input.NamSinh;
                    nguoiBenh.SoTheBHYT = input.SoTheBHYT;
                    nguoiBenh.NoiDkBanDau = input.NoiDkBanDau;
                    nguoiBenh.MaDonViBHXH = input.MaDonViBHXH;
                    nguoiBenh.GiaTriSuDungTuNgay = input.GiaTriSuDungTuNgay;
                    nguoiBenh.ThoiDiemDuNam = input.ThoiDiemDuNam;

                    await _nguoiBenhRepository.UpdateAsync(nguoiBenh);

                    return new MobileDto.CommonResponse()
                    {
                        status = true,
                        message = "Thay đổi thông tin thành công!",
                        errorCode = 0
                    };
                }
                catch (Exception e)
                {
                    return new MobileDto.CommonResponse()
                    {
                        status = false,
                        message = "Có lỗi xảy ra!",
                        errorCode = 400
                    };
                }
            }
            else
            {
                return new MobileDto.GetTenantsResultDto()
                {
                    status = false,
                    message = checkTokenDto.message,
                    errorCode = checkTokenDto.errorCode,
                    tenants = new List<MobileDto.TenantDto>()
                };
            }
        }

        [HttpPost]
        public async Task<MobileDto.AvatarChangeResultDto> UpdateAnh(MobileDto.AvatarChangeDto input)
        {
            MobileDto.CheckTokenDto checkTokenDto = CheckValidToken(input.token, input.userid, input.username);
            if (checkTokenDto.status)
            {
                try
                {
                    NguoiBenh nguoiBenh = await _nguoiBenhRepository.FirstOrDefaultAsync(p => p.Id == input.userid);
                    if (!string.IsNullOrWhiteSpace(nguoiBenh.ProfilePicture))
                    {
                        return new MobileDto.AvatarChangeResultDto()
                        {
                            status = true,
                            message = "Tài khoản đã cập nhật ảnh",
                            errorCode = 0,
                            ImageUrl = nguoiBenh.ProfilePicture
                        };
                    }
                        
                    using (Image image = Image.FromStream(new MemoryStream(input.Data)))
                    {
                        var ttrrs = input.JpegFileName;
                        image.Save("wwwroot/Common/Images/UserProfilePicture/" + ttrrs, ImageFormat.Jpeg); // Or Png

                        bool isEditMode = !nguoiBenh.ProfilePicture.IsNullOrEmpty();

                        nguoiBenh.ProfilePicture = "https://chamcongvien7.techber.vn/Common/Images/UserProfilePicture/" + ttrrs;
                        await _nguoiBenhRepository.UpdateAsync(nguoiBenh);

                        await _hanetAppservices.RegisterUser(new NguoiBenhDto()
                        {
                            Id = nguoiBenh.Id,
                            Password = nguoiBenh.Password,
                            Token = nguoiBenh.Token,
                            DiaChi = nguoiBenh.DiaChi,
                            EmailAddress = nguoiBenh.EmailAddress,
                            GioiTinh = nguoiBenh.GioiTinh,
                            IsActive = nguoiBenh.IsActive,
                            NamSinh = nguoiBenh.NamSinh,
                            NgaySinh = nguoiBenh.NgaySinh,
                            PhoneNumber = nguoiBenh.PhoneNumber,
                            ProfilePicture = nguoiBenh.ProfilePicture,
                            ThangSinh = nguoiBenh.ThangSinh,
                            TokenExpire = nguoiBenh.TokenExpire,
                            UserName = nguoiBenh.UserName,
                            AccessFailedCount = nguoiBenh.AccessFailedCount,
                            EmailConfirmationCode = nguoiBenh.EmailConfirmationCode,
                            HoVaTen = nguoiBenh.HoVaTen,
                            IsEmailConfirmed = nguoiBenh.IsEmailConfirmed,
                            PasswordResetCode = nguoiBenh.PasswordResetCode,
                            IsPhoneNumberConfirmed = nguoiBenh.IsPhoneNumberConfirmed,
                            NoiDkBanDau = nguoiBenh.NoiDkBanDau,
                            ThoiDiemDuNam = nguoiBenh.ThoiDiemDuNam,
                            GiaTriSuDungTuNgay = nguoiBenh.GiaTriSuDungTuNgay,
                            SoTheBHYT = nguoiBenh.SoTheBHYT,
                            MaDonViBHXH = nguoiBenh.MaDonViBHXH
                        }, isEditMode);

                        return new MobileDto.AvatarChangeResultDto()
                        {
                            status = true,
                            message = "Thay đổi thông tin thành công!",
                            errorCode = 0,
                            ImageUrl = "https://chamcongv7.techber.vn/Common/Images/UserProfilePicture/" + ttrrs
                        };
                    }
                }
                catch (Exception e)
                {
                    return new MobileDto.AvatarChangeResultDto()
                    {
                        status = false,
                        message = "Có lỗi xảy ra!",
                        errorCode = 400
                    };
                }
            }
            else
            {
                return new MobileDto.AvatarChangeResultDto()
                {
                    status = false,
                    message = checkTokenDto.message,
                    errorCode = checkTokenDto.errorCode
                };
            }
        }

        [HttpPost]
        public async Task<MobileDto.KetQuaTraveThongTinNguoiBenhResultDto> GetThongTinNguoiBenh(
            MobileDto.GetThongTinNguoiBenh input)
        {
            NguoiBenh nguoiBenhFilter = _nguoiBenhRepository.FirstOrDefault(p => p.Token.Equals(input.Token));
            if (nguoiBenhFilter.isNull())//Token đã bị cập nhật
            {
                return new MobileDto.KetQuaTraveThongTinNguoiBenhResultDto()
                {
                    status = false,
                    message = "Hết phiên đăng nhập! Vui lòng đăng nhập lại.",
                    errorCode = 401
                };
            }
            MobileDto.CheckTokenDto checkTokenDto =
                CheckValidToken(nguoiBenhFilter.Token, nguoiBenhFilter.Id, nguoiBenhFilter.UserName);
            if (checkTokenDto.status)
            {
                try
                {
                    NguoiBenh nguoiBenh =
                        await _nguoiBenhRepository.FirstOrDefaultAsync(p => p.Id == nguoiBenhFilter.Id);
                    var ketqua = new MobileDto.ThongTinNguoiBenhResultDto()
                    {
                        HoVaTen = nguoiBenh.HoVaTen,
                        NgaySinh = nguoiBenh.NgaySinh,
                        GioiTinh = nguoiBenh.GioiTinh,
                        DiaChi = nguoiBenh.DiaChi,
                        PhoneNumber = nguoiBenh.PhoneNumber,
                        EmailAddress = nguoiBenh.EmailAddress,
                        ProfilePicture = nguoiBenh.ProfilePicture,
                        ThangSinh = nguoiBenh.ThangSinh,
                        NamSinh = nguoiBenh.NamSinh,
                        SoTheBHYT = nguoiBenh.SoTheBHYT,
                        NoiDkBanDau = nguoiBenh.NoiDkBanDau,
                        MaDonViBHXH = nguoiBenh.MaDonViBHXH,
                        GiaTriSuDungTuNgay = nguoiBenh.GiaTriSuDungTuNgay,
                        ThoiDiemDuNam = nguoiBenh.ThoiDiemDuNam
                    };

                    return new MobileDto.KetQuaTraveThongTinNguoiBenhResultDto()
                    {
                        status = true,
                        message = "Thành công!",
                        errorCode = 0,
                        ThongTinNguoiBenhResultDto = ketqua
                    };
                }
                catch (Exception e)
                {
                    return new MobileDto.KetQuaTraveThongTinNguoiBenhResultDto()
                    {
                        status = false,
                        message = "Có lỗi xảy ra",
                        errorCode = 400
                    };
                }
            }
            else
            {
                return new MobileDto.KetQuaTraveThongTinNguoiBenhResultDto()
                {
                    status = false,
                    message = checkTokenDto.message,
                    errorCode = checkTokenDto.errorCode,
                    tenants = new List<MobileDto.TenantDto>()
                };
            }
        }
        
        [HttpPost]
        public async Task<MobileDto.GetImageBacSiOutput> GetProfilePictureById(MobileDto.GetImageBacSi input)
        {
            if (input.Image == "")
            {
                return new MobileDto.GetImageBacSiOutput(string.Empty);
            }
            var guid = Guid.Parse(input.Image);
            return await GetProfilePictureByIdInternal(guid);
        }

        private async Task<MobileDto.GetImageBacSiOutput> GetProfilePictureByIdInternal(Guid profilePictureId)
        {
            var bytes = await GetProfilePictureByIdOrNull(profilePictureId);
            if (bytes == null)
            {
                return new MobileDto.GetImageBacSiOutput(string.Empty);
            }

            return new MobileDto.GetImageBacSiOutput(Convert.ToBase64String(bytes));
        }
        private async Task<byte[]> GetProfilePictureByIdOrNull(Guid profilePictureId)
        {
            var file = await _binaryObjectManager.GetOrNullAsync(profilePictureId);
            if (file == null)
            {
                return null;
            }

            return file.Bytes;
        }
        
        [HttpPost]
        public async Task<MobileDto.GetDanhSachBacSiChuyenKhoaTenantResultDto> GetDanhSachBacSiChuyenKhoaTenant(
            MobileDto.GetDanhSachBacSiChuyenKhoaTenantDto input)
        {
            MobileDto.CheckTokenDto checkTokenDto = CheckValidToken(input.token, input.userid, input.username);
            if (checkTokenDto.status)
            {
                try
                {
                    _unitOfWorkManager.Current.SetTenantId(input.tenantId);
                    var listBacSiChuyenKhoa = _bacSiChuyenKhoaRepository.GetAll()
                        .WhereIf(!input.ChuyenKhoaId.isNull() && input.ChuyenKhoaId > 0,
                            p => p.ChuyenKhoaId == input.ChuyenKhoaId);
                    if (await listBacSiChuyenKhoa.CountAsync() == 0)
                    {
                        return new MobileDto.GetDanhSachBacSiChuyenKhoaTenantResultDto()
                        {
                            status = false,
                            message = "Không có dữ liệu bác sĩ chuyên khoa này",
                            errorCode = 410
                        };
                    }
                    else
                    {
                        var query = (from o in listBacSiChuyenKhoa
                                join o1 in _userRepository.GetAll() on o.UserId equals o1.Id into j1
                                from s1 in j1.DefaultIfEmpty()
                                join o2 in _thongTinBacSiMoRongRepository.GetAll() on o.UserId equals o2.UserId into j2
                                from s2 in j2.DefaultIfEmpty()
                                //join o3 in _lichHenKhamRepository.GetAll() on o.UserId equals o3.BacSiId into j3
                                //from s3 in j3.DefaultIfEmpty()
                                select new MobileDto.DanhSachBacSiChuyenKhoaTenantDto()
                                {
                                    BacSiId = o.UserId,
                                    ChuyenKhoaId = o.ChuyenKhoaId,
                                    Name = s1.Name,
                                    Surname = s1.Surname,
                                    EmailAddress = s1.EmailAddress,
                                    PhoneNumber = s1.PhoneNumber,
                                    Image = !s2.Image.isNull() ? s2.Image : "",
                                    TieuSu = s2.TieuSu,
                                    ChucDanh = s2.ChucDanh
                                }
                            );
                        var listQuery = await query.ToListAsync();
                        foreach (var VARIABLE in listQuery)
                        {
                            VARIABLE.SoLanDat = _lichHenKhamRepository.GetAll()
                                .WhereIf(true, p => p.BacSiId == VARIABLE.BacSiId).Count();
                            MobileDto.GetImageBacSi inputImage = new MobileDto.GetImageBacSi()
                            {
                                Image = VARIABLE.Image
                            };
                            var ketqua = await GetProfilePictureById(inputImage);
                            VARIABLE.Image = ketqua.ProfilePicture;
                        }

                        if (!input.SoBacSiCanLayTrongDs.isNull() && input.SoBacSiCanLayTrongDs > 0)
                        {
                            return new MobileDto.GetDanhSachBacSiChuyenKhoaTenantResultDto()
                            {
                                status = true,
                                message = "Thành công",
                                errorCode = 0,
                                ListDanhSachBacSiChuyenKhoaTenantDto = listQuery.OrderByDescending(a => a.SoLanDat)
                                    .Take(input.SoBacSiCanLayTrongDs).ToList()
                            };
                        }
                        else
                        {
                            return new MobileDto.GetDanhSachBacSiChuyenKhoaTenantResultDto()
                            {
                                status = true,
                                message = "Thành công",
                                errorCode = 0,
                                ListDanhSachBacSiChuyenKhoaTenantDto = listQuery
                            };
                        }
                    }
                }
                catch (Exception e)
                {
                    return new MobileDto.GetDanhSachBacSiChuyenKhoaTenantResultDto()
                    {
                        status = false,
                        message = "Có lỗi xảy ra",
                        errorCode = 400
                    };
                }
            }
            else
            {
                return new MobileDto.GetDanhSachBacSiChuyenKhoaTenantResultDto()
                {
                    status = false,
                    message = checkTokenDto.message,
                    errorCode = checkTokenDto.errorCode,
                    tenants = new List<MobileDto.TenantDto>()
                };
            }
        }

        public Task<MobileDto.CommonResponse> LuuDeviceToken(MobileDto.CapNhatThongTinDeviceToken input)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        public async Task<MobileDto.GetThongBaoNguoiBenhResultDto> GetThongBaoNguoiBenh(MobileDto.GetMenuDto input)
        {
            MobileDto.CheckTokenDto checkTokenDto = CheckValidToken(input.token, input.userid, input.username);
            if (checkTokenDto.status)
            {
                try
                {
                    var listThongBaoNguoiBenh = _nguoiBenhNotificationRepository.GetAll()
                        .WhereIf(true, p => p.NguoiBenhId == input.userid)
                        .WhereIf(true, p => DateTime.Now.AddDays(-30) <= p.ThoiGianGui);
                    if (await listThongBaoNguoiBenh.CountAsync() == 0)
                    {
                        
                        return new MobileDto.GetThongBaoNguoiBenhResultDto()
                        {
                            status = true,
                            message = "Bạn không có thông báo!",
                            errorCode = 0
                        };
                    }
                    else
                    {
                        var querry = from o in listThongBaoNguoiBenh
                            select new MobileDto.GetThongBaoNguoiBenhResult()
                            {
                                IdThongBao = o.Id,
                                NguoiBenhId = o.NguoiBenhId,
                                TieuDe = o.TieuDe,
                                TrangThai = o.TrangThai,
                                ThoiGianGui = o.ThoiGianGui,
                                NoiDungTinNhan = o.NoiDungTinNhan
                            };
                        var listResult = await querry.ToListAsync();
                        return new MobileDto.GetThongBaoNguoiBenhResultDto()
                        {
                            status = true,
                            message = "Thành công",
                            errorCode = 0,
                            ListThongBaoNguoiBenh = listResult.OrderByDescending(o => o.IdThongBao).ToList()
                        };
                    }
                }
                catch (Exception e)
                {
                    return new MobileDto.GetThongBaoNguoiBenhResultDto()
                    {
                        status = false,
                        message = "Có lỗi xảy ra!",
                        errorCode = 400
                    };
                }
            }
            else
            {
                return new MobileDto.GetThongBaoNguoiBenhResultDto()
                {
                    status = false,
                    message = checkTokenDto.message,
                    errorCode = checkTokenDto.errorCode
                };
            }
        }

        public async Task<MobileDto.CommonResponse> SetTrangThaiThongBaoNguoiBenh(
            MobileDto.SetTrangThaiThongBaoNguoiBenhDto input)
        {
            MobileDto.CheckTokenDto checkTokenDto = CheckValidToken(input.token, input.userid, input.username);
            if (checkTokenDto.status)
            {
                try
                {
                    NguoiBenhNotification nguoiBenhNotification =
                        await _nguoiBenhNotificationRepository.FirstOrDefaultAsync(p =>
                            p.Id == input.IdThongBao && p.NguoiBenhId == input.userid);
                    nguoiBenhNotification.TrangThai = 1;

                    await _nguoiBenhNotificationRepository.UpdateAsync(nguoiBenhNotification);
                    return new MobileDto.CommonResponse()
                    {
                        status = true,
                        message = "Cập nhật thành công",
                        errorCode = 0
                    };
                }
                catch (Exception e)
                {
                    return new MobileDto.CommonResponse()
                    {
                        status = false,
                        errorCode = 400,
                        message = "Có lỗi xảy ra! "
                    };;
                }
            }
            else
            {
                return new MobileDto.CommonResponse()
                {
                    status = false,
                    message = checkTokenDto.message,
                    errorCode = checkTokenDto.errorCode
                };
            }
        }

        [HttpPost]
        public async Task<MobileDto.GetGioKhamTheoKhungResultDto> GetGioKhamTheoKhung(
            MobileDto.GetGioKhamTheoKhungDto input)
        {
            MobileDto.CheckTokenDto checkTokenDto = CheckValidToken(input.token, input.userid, input.username);
            if (checkTokenDto.status)
            {
                var ngayHenKham = input.NgayHenKham.Day;
                var thangHenKham = input.NgayHenKham.Month;
                var namHenKham = input.NgayHenKham.Year;
                try
                {
                    _unitOfWorkManager.Current.SetTenantId(input.tenantId);
                    var query = _lichHenKhamRepository.GetAll()
                            .WhereIf(true, p => p.ChuyenKhoaId == input.ChuyenKhoaId)
                            .WhereIf(true,
                                p => p.NgayHenKham.Day == ngayHenKham && p.NgayHenKham.Month == thangHenKham &&
                                     p.NgayHenKham.Year == namHenKham)
                            .WhereIf(true, p => p.Flag != TechberConsts.FLAG_HUY_LHK)
                        ;
                    var khamSession =
                        await _thongTinDonViRepository.FirstOrDefaultAsync(p => p.Key.Equals("KhamSession"));
                    int khamSessionValue = Int32.Parse(khamSession.Value);
                    var gioBatDauLamViecSang =
                        await _thongTinDonViRepository.FirstOrDefaultAsync(p => p.Key.Equals("GioBatDauLamViecSang"));
                    var gioBatDauLamViecSangValue = gioBatDauLamViecSang.Value ?? "";
                    var gioKetThucLamViecSang =
                        await _thongTinDonViRepository.FirstOrDefaultAsync(p => p.Key.Equals("GioKetThucLamViecSang"));
                    var gioKetThucLamViecSangValue = gioKetThucLamViecSang.Value ?? "";
                    var gioBatDauLamViecChieu =
                        await _thongTinDonViRepository.FirstOrDefaultAsync(p => p.Key.Equals("GioBatDauLamViecChieu"));
                    var gioBatDauLamViecChieuValue = gioBatDauLamViecChieu.Value ?? "";
                    var gioKetThucLamViecChieu =
                        await _thongTinDonViRepository.FirstOrDefaultAsync(p => p.Key.Equals("GioKetThucLamViecChieu"));
                    var gioKetThucLamViecChieuValue = gioKetThucLamViecChieu.Value ?? "";
                    var lichHenKham = (from o in query
                        select new LichHenKhamDto()
                        {
                            NgayHenKham = o.NgayHenKham,
                            MoTaTrieuChung = o.MoTaTrieuChung,
                            IsCoBHYT = o.IsCoBHYT,
                            SoTheBHYT = o.SoTheBHYT,
                            NoiDangKyKCBDauTien = o.NoiDangKyKCBDauTien,
                            BHYTValidDate = o.BHYTValidDate,
                            PhuongThucThanhToan = o.PhuongThucThanhToan,
                            IsDaKham = o.IsDaKham,
                            IsDaThanhToan = o.IsDaThanhToan,
                            TimeHoanThanhKham = o.TimeHoanThanhKham,
                            TimeHoanThanhThanhToan = o.TimeHoanThanhThanhToan,
                            ChiDinhDichVuSerialize = o.ChiDinhDichVuSerialize,
                            Flag = o.Flag,
                            QRString = o.QRString,
                            BacSiId = o.BacSiId,
                            TienThua = o.TienThua,
                            ThuNganId = o.ThuNganId,
                            NguoiBenhId = o.NguoiBenhId,
                            NguoiThanId = o.NguoiThanId,
                            ChuyenKhoaId = o.ChuyenKhoaId,
                            KhungKham = (int)o.KhungKham,
                            Id = o.Id
                        }).ToList();
                    
                    //Tính thời gian làm việc
                    string thoiGianBatDauLamViecSang = input.NgayHenKham.ToString("MM/dd/yyyy ") + gioBatDauLamViecSangValue;
                    string thoiGianKetThucLamViecSang = input.NgayHenKham.ToString("MM/dd/yyyy ") + gioKetThucLamViecSangValue;
                    string thoiGianBatDauLamViecChieu = input.NgayHenKham.ToString("MM/dd/yyyy ") + gioBatDauLamViecChieuValue;
                    string thoiGianKetThucLamViecChieu = input.NgayHenKham.ToString("MM/dd/yyyy ") + gioKetThucLamViecChieuValue;
                    TimeSpan tongThoiGianLamViecSang = Convert.ToDateTime(thoiGianKetThucLamViecSang) - Convert.ToDateTime(thoiGianBatDauLamViecSang);
                    var tongThoiGianLamViecSangValue = (int)tongThoiGianLamViecSang.TotalMinutes;
                    TimeSpan tongThoiGianLamViecChieu = Convert.ToDateTime(thoiGianKetThucLamViecChieu) - Convert.ToDateTime(thoiGianBatDauLamViecChieu);
                    var tongThoiGianLamViecChieuValue = tongThoiGianLamViecChieu.TotalMinutes;
                    int[] dem = new int[1000];
                    //Đếm phân tán
                    for (int khungKham = 1; khungKham <= tongThoiGianLamViecSangValue / khamSessionValue; khungKham++)
                    {
                        dem[khungKham] = 0;
                    }

                    for (int khungKham = (tongThoiGianLamViecSangValue / khamSessionValue) + 1 ; khungKham <= (tongThoiGianLamViecChieuValue + tongThoiGianLamViecSangValue) / khamSessionValue; khungKham++)
                    {
                        dem[khungKham] = 0;
                    }

                    foreach (var VARIABLE in lichHenKham)
                    {
                        dem[(int)VARIABLE.KhungKham] = 1;
                    }
                    //Xử lí vào list<GetGioKhamTheoKhung>
                    List<MobileDto.GioKhamTheoKhung> gioKhamTheoKhungSangs = new List<MobileDto.GioKhamTheoKhung>();
                    List<MobileDto.GioKhamTheoKhung> gioKhamTheoKhungChieus = new List<MobileDto.GioKhamTheoKhung>();
                    DateTime GioKhamSangTemp = Convert.ToDateTime(thoiGianBatDauLamViecSang);
                    DateTime GioKhamChieuTemp = Convert.ToDateTime(thoiGianBatDauLamViecChieu);
                    for (int khungKham = 1; khungKham <= tongThoiGianLamViecSangValue / khamSessionValue; khungKham++)
                    {
                        TimeSpan SoSanhThoiGianSang = Convert.ToDateTime(input.NgayHenKham.ToString("MM/dd/yyyy ") + GioKhamSangTemp.ToString("HH:mm")) - DateTime.Now;
                        if ((int)SoSanhThoiGianSang.TotalMinutes > 0)
                        {
                            if (dem[khungKham] == 1)
                            {
                                gioKhamTheoKhungSangs.Add(new MobileDto.GioKhamTheoKhung()
                                {
                                    KhungKham = khungKham,
                                    GioKham = GioKhamSangTemp.ToString("HH:mm"),
                                    IsCoTheDat = false,
                                    MoTa = "Đã đăt được đặt"
                                });
                            
                            }
                            else
                            {
                                gioKhamTheoKhungSangs.Add(new MobileDto.GioKhamTheoKhung()
                                {
                                    KhungKham = khungKham,
                                    GioKham = GioKhamSangTemp.ToString("HH:mm"),
                                    IsCoTheDat = true,
                                    MoTa = "Có thể đăt lịch"
                                });
                            }
                        }
                        else
                        {
                            gioKhamTheoKhungSangs.Add(new MobileDto.GioKhamTheoKhung()
                            {
                                KhungKham = khungKham,
                                GioKham = GioKhamSangTemp.ToString("HH:mm"),
                                IsCoTheDat = false,
                                MoTa = "Thời gian đặt không hợp lệ"
                            });
                        }
                        GioKhamSangTemp = GioKhamSangTemp.AddMinutes(15);
                    }
                    for (int khungKham = (tongThoiGianLamViecSangValue / khamSessionValue) + 1 ; khungKham <= (tongThoiGianLamViecChieuValue + tongThoiGianLamViecSangValue) / khamSessionValue; khungKham++)
                    {
                        TimeSpan SoSanhThoiGianChieu = Convert.ToDateTime(input.NgayHenKham.ToString("MM/dd/yyyy ") + GioKhamChieuTemp.ToString("HH:mm")) - DateTime.Now;
                        if ((int) SoSanhThoiGianChieu.TotalMinutes > 0)
                        {
                            if (dem[khungKham] == 1)
                            {
                                gioKhamTheoKhungChieus.Add(new MobileDto.GioKhamTheoKhung()
                                {
                                    KhungKham = khungKham,
                                    GioKham = GioKhamChieuTemp.ToString("HH:mm"),
                                    IsCoTheDat = false,
                                    MoTa = "Đã đăt được bởi người dùng khác"
                                });
                            
                            }
                            else
                            {
                                gioKhamTheoKhungChieus.Add(new MobileDto.GioKhamTheoKhung()
                                {
                                    KhungKham = khungKham,
                                    GioKham = GioKhamChieuTemp.ToString("HH:mm"),
                                    IsCoTheDat = true,
                                    MoTa = "Có thể đăt lịch"
                                });
                            }
                        }
                        else
                        {
                            gioKhamTheoKhungChieus.Add(new MobileDto.GioKhamTheoKhung()
                            {
                                KhungKham = khungKham,
                                GioKham = GioKhamChieuTemp.ToString("HH:mm"),
                                IsCoTheDat = false,
                                MoTa = "Thời gian đặt không hợp lệ"
                            });
                        }
                        
                        GioKhamChieuTemp = GioKhamChieuTemp.AddMinutes(15);
                    }
                    return new MobileDto.GetGioKhamTheoKhungResultDto()
                    {
                        status = true,
                        message = "Thành công",
                        errorCode = 0,
                        LichHenKham = lichHenKham,
                        KhamSession = khamSessionValue,
                        GioBatDauLamViecSang = gioBatDauLamViecSangValue,
                        GioKetThucLamViecSang = gioKetThucLamViecSangValue,
                        GioBatDauLamViecChieu = gioBatDauLamViecChieuValue,
                        GioKetThucLamViecChieu = gioKetThucLamViecChieuValue,
                        GioKhamTheoKhungSang = gioKhamTheoKhungSangs.OrderBy(o => o.KhungKham).ToList(),
                        GioKhamTheoKhungChieu = gioKhamTheoKhungChieus.OrderBy(o => o.KhungKham).ToList()
                    };
                }
                catch (Exception e)
                {
                    return new MobileDto.GetGioKhamTheoKhungResultDto()
                    {
                        status = false,
                        message = "Có lỗi xảy ra!",
                        errorCode = 400
                    };
                }
            }
            else
            {
                return new MobileDto.GetGioKhamTheoKhungResultDto()
                {
                    status = false,
                    message = checkTokenDto.message,
                    errorCode = checkTokenDto.errorCode
                };
            }
        }

         public async Task<MobileDto.CommonResponse> EditLichHenKham(MobileDto.EditLichHenKhamDto input)
        {
            MobileDto.CheckTokenDto checkTokenDto = CheckValidToken(input.token, input.userid, input.username);
            if (checkTokenDto.status)
            {
                try
                {
                    _unitOfWorkManager.Current.SetTenantId(input.tenantId);
                    LichHenKham lichHenKham = await _lichHenKhamRepository.FirstOrDefaultAsync(p =>
                        p.Id == input.LichHenKhamId && p.NguoiBenhId == input.userid);
                    if (lichHenKham.IsDaKham == true || lichHenKham.Flag == TechberConsts.FLAG_THU_NGAN)
                    {
                        return new MobileDto.CommonResponse()
                        {
                            status = false,
                            message = "Lịch khám đã hoàn thành, không thể chỉnh sửa!",
                            errorCode = 400
                        };
                    }    
                    else
                    {
                        lichHenKham.NgayHenKham = input.NgayHenKham;
                        lichHenKham.MoTaTrieuChung = input.MoTaTrieuChung;
                        lichHenKham.IsCoBHYT = input.IsCoBHYT;
                        lichHenKham.SoTheBHYT = input.SoTheBHYT;
                        lichHenKham.NoiDangKyKCBDauTien = input.NoiDangKyKCBDauTien;
                        lichHenKham.BHYTValidDate = input.BHYTValidDate;
                        lichHenKham.PhuongThucThanhToan = (!input.PhuongThucThanhToan.isNull() && input.PhuongThucThanhToan > 0) ? input.PhuongThucThanhToan : 2;
                        lichHenKham.BacSiId = (!input.BacSiId.isNull() && input.BacSiId > 0) ? input.BacSiId : null ;
                        lichHenKham.NguoiThanId = (!input.NguoiThanId.isNull() && input.NguoiThanId > 0) ? input.NguoiThanId : null ;
                        lichHenKham.ChuyenKhoaId = input.ChuyenKhoaId;
                        lichHenKham.KhungKham = input.KhungKham;

                        await _lichHenKhamRepository.UpdateAsync(lichHenKham);
                        return new MobileDto.CommonResponse()
                        {
                            status = false,
                            message = "Thành công",
                            errorCode = 0
                        };
                    }
                }
                catch (Exception e)
                {
                    return new MobileDto.CommonResponse()
                    {
                        status = false,
                        message = "Có lỗi xảy ra!",
                        errorCode = 400
                    };
                }
            }
            else
            {
                return new MobileDto.CommonResponse()
                {
                    status = false,
                    message = checkTokenDto.message,
                    errorCode = checkTokenDto.errorCode
                };
            }
        }

        public async Task<MobileDto.CommonResponse> HuyLichHenKham(MobileDto.HuyLichHenKhamDto input)
        {
            MobileDto.CheckTokenDto checkTokenDto = CheckValidToken(input.token, input.userid, input.username);
            if (checkTokenDto.status)
            {
                try
                {
                    _unitOfWorkManager.Current.SetTenantId(input.tenantId);
                    LichHenKham lichHenKham = await _lichHenKhamRepository.FirstOrDefaultAsync(p =>
                        p.Id == input.LichHenKhamId && p.NguoiBenhId == input.userid);
                    if (lichHenKham.IsDaKham == true)
                    {
                        return new MobileDto.CommonResponse()
                        {
                            status = false,
                            message = "Lịch khám đã hoàn thành, không thể hủy!",
                            errorCode = 400
                        };
                    }
                    else
                    {
                        lichHenKham.Flag = TechberConsts.FLAG_HUY_LHK;
                        _lichHenKhamRepository.Update(lichHenKham);
                        return new MobileDto.CommonResponse()
                        {
                            status = false,
                            message = "Thành công",
                            errorCode = 0
                        };
                    }
                }
                catch (Exception e)
                {
                    return new MobileDto.CommonResponse()
                    {
                        status = false,
                        message = "Có lỗi xảy ra!",
                        errorCode = 400
                    };
                }
            }
            else
            {
                return new MobileDto.CommonResponse()
                {
                    status = false,
                    message = checkTokenDto.message,
                    errorCode = checkTokenDto.errorCode
                };
            }
        }

        [HttpPost]
        public async Task<MobileDto.GetDanhSachNhanVienResultDto> GetAllNhanVien(MobileDto.GetMenuDto input)
        {
            // Check token giống như các API khác
            MobileDto.CheckTokenDto checkTokenDto = CheckValidToken(input.token, input.userid, input.username);
            if (checkTokenDto.status)
            {
                try
                {
                    // Lấy danh sách người bệnh là nhân viên
                    var nhanViens = await _nguoiBenhRepository.GetAllListAsync(p => p.IsNhanVien == true);
                    var result = nhanViens.Select(nv => new MobileDto.NguoiBenhDto
                    {
                        HoVaTen = nv.HoVaTen,
                        PhoneNumber = nv.PhoneNumber,
                        EmailAddress = nv.EmailAddress,
                        NgaySinh = nv.NgaySinh,
                        ThangSinh = nv.ThangSinh,
                        NamSinh = nv.NamSinh,
                        GioiTinh = nv.GioiTinh,
                        DiaChi = nv.DiaChi,
                        ProfilePicture = nv.ProfilePicture,
                        GiaTriSuDungTuNgay = nv.GiaTriSuDungTuNgay,
                        SoTheBHYT = nv.SoTheBHYT,
                        MaDonViBHXH = nv.MaDonViBHXH,
                        ThoiDiemDuNam = nv.ThoiDiemDuNam,
                        NoiDkBanDau = nv.NoiDkBanDau
                    }).ToList();

                    return new MobileDto.GetDanhSachNhanVienResultDto
                    {
                        status = true,
                        message = "Thành công",
                        errorCode = 0,
                        DanhSachNhanVien = result
                    };
                }
                catch (Exception ex)
                {
                    return new MobileDto.GetDanhSachNhanVienResultDto
                    {
                        status = false,
                        message = "Có lỗi xảy ra!",
                        errorCode = 400,
                        DanhSachNhanVien = new List<MobileDto.NguoiBenhDto>()
                    };
                }
            }
            else
            {
                return new MobileDto.GetDanhSachNhanVienResultDto
                {
                    status = false,
                    message = checkTokenDto.message,
                    errorCode = checkTokenDto.errorCode,
                    DanhSachNhanVien = new List<MobileDto.NguoiBenhDto>()
                };
            }
        }

        [HttpPost]
        public async Task<MobileDto.CommonResponse> DiemDanhNhanVien(MobileDto.DiemDanhInputDto input)
        {
            try
            {
                var checkToken = CheckValidToken(input.token, input.userid, input.username);
                if (!checkToken.status)
                    return new MobileDto.CommonResponse { status = false, errorCode = 401, message = checkToken.message };

                var nguoiBenh = await _nguoiBenhRepository.FirstOrDefaultAsync(checkToken.userid);
                if (nguoiBenh == null)
                    return new MobileDto.CommonResponse { status = false, errorCode = 404, message = "Không tìm thấy thông tin người dùng" };

                if (string.IsNullOrWhiteSpace(nguoiBenh.ProfilePicture))
                    return new MobileDto.CommonResponse { status = false, errorCode = 404, message = "Chưa cập nhật ảnh hồ sơ" };
                if (string.IsNullOrWhiteSpace(input.CapturedImageBase64))
                    return new MobileDto.CommonResponse { status = false, errorCode = 400, message = "Ảnh chụp không hợp lệ" };

                string convertBase64nguoiBenhProfilePicture;
                try
                {
                    convertBase64nguoiBenhProfilePicture = await ConvertImageUrlToBase64(nguoiBenh.ProfilePicture);
                    if (string.IsNullOrWhiteSpace(convertBase64nguoiBenhProfilePicture))
                        return new MobileDto.CommonResponse { status = false, errorCode = 415, message = "Ảnh hồ sơ trống sau khi chuyển đổi" };
                }
                catch (Exception ex)
                {
                    return new MobileDto.CommonResponse { status = false, errorCode = 415, message = $"Không thể tải ảnh hồ sơ: {ex.Message}" };
                }

                bool isMatched;
                double similarity;
                try
                {
                    isMatched = CompareImages(input.CapturedImageBase64, convertBase64nguoiBenhProfilePicture, out similarity);
                }
                catch (Exception ex)
                {
                    return new MobileDto.CommonResponse { status = false, errorCode = 414, message = $"Dữ liệu ảnh không hợp lệ: {ex.Message}" };
                }

                if (!isMatched)
                    return new MobileDto.CommonResponse { status = false, errorCode = 410, message = $"Không trùng khớp khuôn mặt (Mức độ: {similarity:F2}%)" };

                var keys = new[] { "Location", "GioLamViecSang", "GioLamViecChieu", "GioLamViecDem", "WorkDays", "LocationRadius" };
                var thongTinDonVi = await _thongTinDonViRepository.GetAll().Where(x => keys.Contains(x.Key)).ToListAsync();

                var locationInfo = thongTinDonVi.FirstOrDefault(x => x.Key == "Location");
                var gioSangInfo = thongTinDonVi.FirstOrDefault(x => x.Key == "GioLamViecSang");
                var gioChieuInfo = thongTinDonVi.FirstOrDefault(x => x.Key == "GioLamViecChieu");
                var gioDemInfo = thongTinDonVi.FirstOrDefault(x => x.Key == "GioLamViecDem");
                var workDaysInfo = thongTinDonVi.FirstOrDefault(x => x.Key == "WorkDays");
                var locationRadiusInfo = thongTinDonVi.FirstOrDefault(x => x.Key == "LocationRadius");

                if (locationInfo == null || gioSangInfo == null || gioChieuInfo == null)
                    return new MobileDto.CommonResponse { status = false, errorCode = 412, message = "Chưa cấu hình đầy đủ thông tin đơn vị" };

                static bool TryParseShiftString(string raw, out TimeSpan start, out TimeSpan end)
                {
                    start = default; end = default;
                    if (string.IsNullOrWhiteSpace(raw)) return false;
                    var parts = raw.Split('-', StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length != 2) return false;
                    if (!TimeSpan.TryParse(parts[0].Trim(), out start)) return false;
                    if (!TimeSpan.TryParse(parts[1].Trim(), out end)) return false;
                    return true;
                }

                static int? ParseWorkDaysMaskFromString(string raw)
                {
                    if (string.IsNullOrWhiteSpace(raw)) return null;
                    raw = raw.Replace(" ", "");
                    if (raw.Equals("Mon-Fri", StringComparison.OrdinalIgnoreCase))
                    {
                        int mask = (int)((WorkDayFlags.MONDAY) | (WorkDayFlags.TUESDAY) | (WorkDayFlags.WEDNESDAY) | (WorkDayFlags.THURSDAY) | (WorkDayFlags.FRIDAY));
                        return mask;
                    }
                    var parts = raw.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    int m = 0;
                    foreach (var p in parts)
                    {
                        var token = p.Trim();
                        if (token.StartsWith("Mon", StringComparison.OrdinalIgnoreCase)) m |= (int)WorkDayFlags.MONDAY;
                        else if (token.StartsWith("Tue", StringComparison.OrdinalIgnoreCase)) m |= (int)WorkDayFlags.TUESDAY;
                        else if (token.StartsWith("Wed", StringComparison.OrdinalIgnoreCase)) m |= (int)WorkDayFlags.WEDNESDAY;
                        else if (token.StartsWith("Thu", StringComparison.OrdinalIgnoreCase)) m |= (int)WorkDayFlags.THURSDAY;
                        else if (token.StartsWith("Fri", StringComparison.OrdinalIgnoreCase)) m |= (int)WorkDayFlags.FRIDAY;
                        else if (token.StartsWith("Sat", StringComparison.OrdinalIgnoreCase)) m |= (int)WorkDayFlags.SATURDAY;
                        else if (token.StartsWith("Sun", StringComparison.OrdinalIgnoreCase)) m |= (int)WorkDayFlags.SUNDAY;
                    }
                    return m == 0 ? (int?)null : m;
                }

                Shift effectiveShift = null;
                if (nguoiBenh.AssignedShiftId.HasValue)
                    effectiveShift = await _shiftRepository.FirstOrDefaultAsync(nguoiBenh.AssignedShiftId.Value);

                var now = DateTime.Now;
                var currentTime = now.TimeOfDay;

                TimeSpan shiftStart = TimeSpan.FromHours(6);
                TimeSpan shiftEnd = TimeSpan.FromHours(18);
                string shiftName = "Sáng";
                int? workDaysMask = null;
                int toleranceMinutes = 15;

                if (effectiveShift != null)
                {
                    shiftStart = effectiveShift.StartTime;
                    shiftEnd = effectiveShift.EndTime;
                    shiftName = string.IsNullOrWhiteSpace(effectiveShift.Name) ? "Ca" : effectiveShift.Name;
                    workDaysMask = effectiveShift.WorkDaysMask;
                    toleranceMinutes = effectiveShift.ToleranceMinutes;
                }
                else
                {
                    bool matched = false;
                    if (gioSangInfo != null && TryParseShiftString(gioSangInfo.Value, out var gs, out var ge) && IsTimeInShift(currentTime, gs, ge))
                    {
                        shiftStart = gs; shiftEnd = ge; shiftName = "Sáng"; matched = true;
                    }
                    else if (gioChieuInfo != null && TryParseShiftString(gioChieuInfo.Value, out var cs, out var ce) && IsTimeInShift(currentTime, cs, ce))
                    {
                        shiftStart = cs; shiftEnd = ce; shiftName = "Chiều"; matched = true;
                    }
                    else if (gioDemInfo != null && TryParseShiftString(gioDemInfo.Value, out var ns, out var ne) && IsTimeInShift(currentTime, ns, ne))
                    {
                        shiftStart = ns; shiftEnd = ne; shiftName = "Đêm"; matched = true;
                    }
                    else
                    {
                        if (gioSangInfo != null && TryParseShiftString(gioSangInfo.Value, out gs, out ge))
                        {
                            shiftStart = gs; shiftEnd = ge; shiftName = "Sáng";
                        }
                    }
                    if (workDaysMask == null && workDaysInfo != null)
                        workDaysMask = ParseWorkDaysMaskFromString(workDaysInfo.Value);
                }

                if (workDaysMask == null && workDaysInfo != null)
                    workDaysMask = ParseWorkDaysMaskFromString(workDaysInfo.Value);

                if (workDaysMask.HasValue && workDaysMask.Value != 0)
                {
                    var todayFlag = 1 << (int)now.DayOfWeek;
                    if ((workDaysMask.Value & todayFlag) == 0)
                        return new MobileDto.CommonResponse { status = false, errorCode = 425, message = "Hôm nay người này được nghỉ theo cấu hình WorkDays." };
                }

                DateTime shiftStartDt, shiftEndDt;
                var today = now.Date;
                if (shiftStart <= shiftEnd)
                {
                    shiftStartDt = today + shiftStart;
                    shiftEndDt = today + shiftEnd;
                }
                else
                {
                    if (now.TimeOfDay >= shiftStart)
                    {
                        shiftStartDt = today + shiftStart;
                        shiftEndDt = today.AddDays(1) + shiftEnd;
                    }
                    else
                    {
                        shiftStartDt = today.AddDays(-1) + shiftStart;
                        shiftEndDt = today + shiftEnd;
                    }
                }

                double locationRadiusKm = 0.2;
                if (locationRadiusInfo != null && double.TryParse(locationRadiusInfo.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out var parsedRadius))
                    locationRadiusKm = parsedRadius;

                if (!string.IsNullOrWhiteSpace(locationInfo?.Value) && input.Latitude != 0 && input.Longitude != 0)
                {
                    var parts = locationInfo.Value.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 2
                        && double.TryParse(parts[0].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var siteLat)
                        && double.TryParse(parts[1].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var siteLon))
                    {
                        var distanceKm = CalculateDistance(input.Latitude, input.Longitude, siteLat, siteLon);
                        if (distanceKm > locationRadiusKm)
                            return new MobileDto.CommonResponse { status = false, errorCode = 426, message = $"Ngoài phạm vi cho phép ({distanceKm:F3} km)" };
                    }
                }

                var attendancesInShift = await _attendanceRepository.GetAll()
                    .Where(x => x.NguoiBenhId == nguoiBenh.Id && x.CheckIn >= shiftStartDt && x.CheckIn <= shiftEndDt)
                    .OrderBy(x => x.CheckIn)
                    .ToListAsync();

                var openAttendance = attendancesInShift.LastOrDefault(a => !a.IsCheckOut);

                var shiftDeviceInfo = $"Shift:{shiftName};{input.DeviceInfo ?? ""}";

                bool isWithinLocation = true;
                if (input.Latitude != 0 && input.Longitude != 0 && !string.IsNullOrWhiteSpace(locationInfo?.Value))
                {
                    var parts = locationInfo.Value.Split(',', StringSplitOptions.RemoveEmptyEntries);
                    if (parts.Length >= 2
                        && double.TryParse(parts[0].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var siteLat)
                        && double.TryParse(parts[1].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out var siteLon))
                    {
                        var distanceKm = CalculateDistance(input.Latitude, input.Longitude, siteLat, siteLon);
                        isWithinLocation = distanceKm <= locationRadiusKm;
                    }
                }

                if (openAttendance == null)
                {
                    bool isLate = now > shiftStartDt.AddMinutes(toleranceMinutes);

                    var attendance = new Attendance
                    {
                        NguoiBenhId = nguoiBenh.Id,
                        CheckIn = now,
                        CheckInLatitude = input.Latitude,
                        CheckInLongitude = input.Longitude,
                        IsCheckInFaceMatched = true,
                        CheckInFaceMatchPercentage = similarity,
                        CheckInDeviceInfo = shiftDeviceInfo,
                        IsWithinLocation = isWithinLocation,
                        IsLateCheckIn = isLate,
                        PhotoPath = input.CapturedImageBase64,
                        IsOvertime = false,
                        OvertimeStart = null,
                        OvertimeEnd = null,
                        ShiftId = effectiveShift?.Id,
                        ShiftName = effectiveShift?.Name ?? shiftName,
                        IsCheckOut = false,
                        CheckOut = null,
                        IsHalfDay = false
                    };

                    await _attendanceRepository.InsertAsync(attendance);

                    var msg = now > shiftStartDt.AddMinutes(toleranceMinutes) ? $"Check-in ca {shiftName} thành công (trễ giờ)" : $"Check-in ca {shiftName} thành công";
                    return new MobileDto.CommonResponse { status = true, errorCode = 0, message = msg };
                }
                else
                {
                    var checkInTime = openAttendance.CheckIn;
                    var workedDuration = now - checkInTime;
                    var shiftDurationHours = (shiftEndDt - shiftStartDt).TotalHours;
                    var isHalfDay = workedDuration.TotalHours < (shiftDurationHours / 2.0);

                    openAttendance.IsCheckOut = true;
                    openAttendance.CheckOut = now;
                    openAttendance.PhotoPath = input.CapturedImageBase64;
                    openAttendance.IsHalfDay = isHalfDay;

                    if (now > shiftEndDt)
                    {
                        openAttendance.IsOvertime = true;
                        openAttendance.OvertimeStart = shiftEndDt;
                        openAttendance.OvertimeEnd = now;
                    }

                    await _attendanceRepository.UpdateAsync(openAttendance);

                    if (isHalfDay)
                        return new MobileDto.CommonResponse { status = true, errorCode = 0, message = $"Kết thúc ca {shiftName} (nửa ngày). Tổng: {workedDuration.TotalHours:F2} giờ" };

                    if (openAttendance.IsOvertime && openAttendance.OvertimeStart.HasValue && openAttendance.OvertimeEnd.HasValue)
                    {
                        var otHours = (openAttendance.OvertimeEnd.Value - openAttendance.OvertimeStart.Value).TotalHours;
                        return new MobileDto.CommonResponse { status = true, errorCode = 0, message = $"Kết thúc ca {shiftName}. Có tăng ca {otHours:F2} giờ. Tổng: {workedDuration.TotalHours:F2} giờ" };
                    }

                    return new MobileDto.CommonResponse { status = true, errorCode = 0, message = $"Kết thúc ca {shiftName} thành công. Tổng: {workedDuration.TotalHours:F2} giờ" };
                }
            }
            catch (UserFriendlyException uex)
            {
                return new MobileDto.CommonResponse { status = false, errorCode = 400, message = uex.Message };
            }
            catch (Exception)
            {
                return new MobileDto.CommonResponse { status = false, errorCode = 500, message = "Lỗi hệ thống" };
            }

            static bool IsTimeInShift(TimeSpan now, TimeSpan start, TimeSpan end)
            {
                if (start <= end) return now >= start && now <= end;
                return now >= start || now <= end;
            }
        }

        // [HttpPost]
        // public async Task<MobileDto.GetLichSuDiemDanhResultDto> GetLichSuDiemDanhById(MobileDto.GetLichSuDiemDanhDto input)
        // {
        //     MobileDto.CheckTokenDto checkTokenDto = CheckValidToken(input.token, input.userid, input.username);
        //
        //     if (checkTokenDto.status)
        //     {
        //         try
        //         {
        //             var fromDate = input.tuNgay ?? DateTime.Now.AddDays(-30);
        //             var toDate = input.denNgay ?? DateTime.Now;
        //
        //             var query = _attendanceRepository.GetAll()
        //                 .Where(p => p.NguoiBenhId == input.userid)
        //                 .Where(p => p.CheckIn.Date >= fromDate.Date && p.CheckIn.Date <= toDate.Date);
        //
        //             var count = await query.CountAsync();
        //
        //             if (count == 0)
        //             {
        //                 return new MobileDto.GetLichSuDiemDanhResultDto()
        //                 {
        //                     status = true,
        //                     message = "Không có lịch sử điểm danh!",
        //                     errorCode = 0
        //                 };
        //             }
        //
        //             var listResult = await query
        //                 .OrderByDescending(p => p.CheckIn)
        //                 .Select(o => new MobileDto.LichSuDiemDanhDto()
        //                 {
        //                     CheckIn = o.CheckIn,
        //                     CheckInLatitude = o.CheckInLatitude,
        //                     CheckInLongitude = o.CheckInLongitude,
        //                     CheckInDeviceInfo = o.CheckInDeviceInfo
        //                 })
        //                 .ToListAsync();
        //
        //             return new MobileDto.GetLichSuDiemDanhResultDto()
        //             {
        //                 status = true,
        //                 message = "Thành công",
        //                 errorCode = 0,
        //                 LichSuDiemDanh = listResult
        //             };
        //         }
        //         catch (Exception ex) 
        //         {
        //             return new MobileDto.GetLichSuDiemDanhResultDto()
        //             {
        //                 status = false,
        //                 message = "Có lỗi xảy ra!" + ex.Message,
        //                 errorCode = 400
        //             };
        //         }
        //     }
        //     else
        //     {
        //         return new MobileDto.GetLichSuDiemDanhResultDto()
        //         {
        //             status = false,
        //             message = checkTokenDto.message,
        //             errorCode = checkTokenDto.errorCode
        //         };
        //     }
        // }
        [HttpPost]
        public async Task<MobileDto.GetLichSuDiemDanhResultDto> GetLichSuDiemDanhById(MobileDto.GetLichSuDiemDanhDto input)
        {
            var checkTokenDto = CheckValidToken(input.token, input.userid, input.username);
            if (!checkTokenDto.status)
            {
                return new MobileDto.GetLichSuDiemDanhResultDto
                {
                    status = false,
                    message = checkTokenDto.message,
                    errorCode = checkTokenDto.errorCode
                };
            }

            try
            {
                var fromDate = input.tuNgay ?? DateTime.Now.AddDays(-30);
                var toDate = input.denNgay ?? DateTime.Now;

                var thongTinDonVi = await _thongTinDonViRepository.GetAll()
                    .Where(x => x.Key == "Location" || x.Key == "GioLamViecSang" || x.Key == "GioLamViecChieu")
                    .ToListAsync();

                var gioSangInfo = thongTinDonVi.FirstOrDefault(x => x.Key == "GioLamViecSang");
                var gioChieuInfo = thongTinDonVi.FirstOrDefault(x => x.Key == "GioLamViecChieu");

                if (gioSangInfo == null || gioChieuInfo == null)
                {
                    return new MobileDto.GetLichSuDiemDanhResultDto
                    {
                        status = false,
                        errorCode = 412,
                        message = "Chưa cấu hình đầy đủ thông tin giờ làm việc"
                    };
                }

                var gioSangParts = gioSangInfo.Value.Split('-', StringSplitOptions.RemoveEmptyEntries);
                var gioChieuParts = gioChieuInfo.Value.Split('-', StringSplitOptions.RemoveEmptyEntries);
                TimeSpan startMorning = TimeSpan.Parse(gioSangParts[0].Trim());
                TimeSpan endMorning = TimeSpan.Parse(gioSangParts[1].Trim());
                TimeSpan startAfternoon = TimeSpan.Parse(gioChieuParts[0].Trim());
                TimeSpan endAfternoon = TimeSpan.Parse(gioChieuParts[1].Trim());

                var query = _attendanceRepository.GetAll()
                    .Where(p => p.NguoiBenhId == input.userid)
                    .Where(p => p.CheckIn.Date >= fromDate.Date && p.CheckIn.Date <= toDate.Date);

                var count = await query.CountAsync();
                if (count == 0)
                {
                    return new MobileDto.GetLichSuDiemDanhResultDto
                    {
                        status = true,
                        message = "Không có lịch sử điểm danh!",
                        errorCode = 0,
                        LichSuDiemDanh = new List<MobileDto.LichSuDiemDanhDto>(),
                        SoNgayLamTheoCa = 0,
                        SoNgayLamHanhChinh = 0,
                        DanhSachNgayLoai = new List<MobileDto.NgayLoaiDto>(),
                        DanhSachNgayThongKe = new List<MobileDto.NgayThongKeDto>(),
                        DiMuon = new MobileDto.DiMuon(),
                        VeSom = new MobileDto.VeSom(),
                        SoNgayNghi = 0,
                        DanhSachNgayNghi = new List<DateTime>()
                    };
                }

                var listResult = await query
                    .OrderBy(p => p.CheckIn)
                    .Select(o => new MobileDto.LichSuDiemDanhDto
                    {
                        CheckIn = o.CheckIn,
                        CheckInLatitude = o.CheckInLatitude,
                        CheckInLongitude = o.CheckInLongitude,
                        CheckInDeviceInfo = o.CheckInDeviceInfo,
                        IsLateCheckIn = o.IsLateCheckIn,
                        ShiftId = o.ShiftId,
                        ShiftName = o.ShiftName
                    })
                    .ToListAsync();

                var shiftIds = listResult.Where(x => x.ShiftId.HasValue).Select(x => x.ShiftId.Value).Distinct().ToList();
                var shiftDict = new Dictionary<int, Karion.BusinessSolution.QuanLyDanhMuc.Shift>();
                if (shiftIds.Any())
                {
                    var shifts = await _shiftRepository.GetAll().Where(s => shiftIds.Contains(s.Id)).ToListAsync();
                    shiftDict = shifts.ToDictionary(s => s.Id, s => s);
                }

                var nguoiBenh = await _nguoiBenhRepository.FirstOrDefaultAsync(input.userid);
                Karion.BusinessSolution.QuanLyDanhMuc.Shift assignedShift = null;
                if (nguoiBenh?.AssignedShiftId != null)
                {
                    var asgId = nguoiBenh.AssignedShiftId.Value;
                    if (!shiftDict.ContainsKey(asgId))
                    {
                        var s = await _shiftRepository.FirstOrDefaultAsync(asgId);
                        if (s != null) shiftDict[asgId] = s;
                    }
                    shiftDict.TryGetValue(nguoiBenh.AssignedShiftId.Value, out assignedShift);
                }

                foreach (var rec in listResult)
                {
                    if (rec.ShiftId.HasValue && shiftDict.TryGetValue(rec.ShiftId.Value, out var s))
                    {
                        rec.WorkMode = "TheoCa";
                        rec.ShiftName = string.IsNullOrWhiteSpace(s.Name) ? rec.ShiftName : s.Name;
                    }
                    else if (assignedShift != null)
                    {
                        rec.WorkMode = "HanhChinh";
                        rec.ShiftName = assignedShift.Name;
                    }
                    else
                    {
                        rec.WorkMode = "HanhChinh";
                        if (string.IsNullOrWhiteSpace(rec.ShiftName)) rec.ShiftName = "Hành chính";
                    }
                }

                var wentWorkDays = listResult.Select(x => x.CheckIn.Date).Distinct().ToList();
                var allDays = Enumerable.Range(0, (toDate.Date - fromDate.Date).Days + 1)
                    .Select(offset => fromDate.Date.AddDays(offset)).ToList();
                var absentDays = allDays.Where(day => !wentWorkDays.Contains(day)).ToList();

                int soNgayDiMuon = 0;
                double tongGioDiMuon = 0;
                int soNgayVeSom = 0;
                double tongGioVeSom = 0;

                int soNgayLamTheoCa = 0;
                int soNgayLamHanhChinh = 0;
                var danhSachNgayLoai = new List<MobileDto.NgayLoaiDto>();
                var danhSachNgayThongKe = new List<MobileDto.NgayThongKeDto>();

                var groupByDate = listResult.GroupBy(x => x.CheckIn.Date).ToList();
                foreach (var group in groupByDate)
                {
                    var date = group.Key;
                    var isTheoCa = group.Any(g => g.ShiftId.HasValue);
                    var dayType = isTheoCa ? "TheoCa" : "HanhChinh";
                    if (isTheoCa) soNgayLamTheoCa++; else soNgayLamHanhChinh++;
                    danhSachNgayLoai.Add(new MobileDto.NgayLoaiDto { Ngay = date, Loai = dayType });

                    var ordered = group.OrderBy(x => x.CheckIn).ToList();
                    var firstRec = ordered.First();
                    var lastRec = ordered.Last();

                    (TimeSpan start, TimeSpan end, int tolerance, Karion.BusinessSolution.QuanLyDanhMuc.Shift shiftObj) GetShiftForRecord(MobileDto.LichSuDiemDanhDto rec)
                    {
                        if (rec.ShiftId.HasValue && shiftDict.TryGetValue(rec.ShiftId.Value, out var sRec))
                            return (sRec.StartTime, sRec.EndTime, sRec.ToleranceMinutes, sRec);
                        if (assignedShift != null)
                            return (assignedShift.StartTime, assignedShift.EndTime, assignedShift.ToleranceMinutes, assignedShift);
                        return (startMorning, endAfternoon, 15, null);
                    }

                    var (fsStart, fsEnd, fsTol, fsShiftObj) = GetShiftForRecord(firstRec);
                    DateTime fsStartDt, fsEndDt;
                    if (fsStart <= fsEnd)
                    {
                        fsStartDt = date + fsStart;
                        fsEndDt = date + fsEnd;
                    }
                    else
                    {
                        fsStartDt = date + fsStart;
                        fsEndDt = date.AddDays(1) + fsEnd;
                    }

                    var firstCheckDt = firstRec.CheckIn;
                    DateTime normalizedFirstCheck = firstCheckDt;
                    if (fsStart > fsEnd && firstCheckDt.TimeOfDay <= fsEnd) normalizedFirstCheck = firstCheckDt.Date.AddDays(1) + firstCheckDt.TimeOfDay;

                    int lateMinutes = 0;
                    if (normalizedFirstCheck > fsStartDt)
                    {
                        lateMinutes = (int)Math.Round((normalizedFirstCheck - fsStartDt).TotalMinutes);
                        soNgayDiMuon++;
                        tongGioDiMuon += (normalizedFirstCheck - fsStartDt).TotalHours;
                    }

                    var (lsStart, lsEnd, lsTol, lsShiftObj) = GetShiftForRecord(lastRec);
                    DateTime lsStartDt, lsEndDt;
                    if (lsStart <= lsEnd)
                    {
                        lsStartDt = date + lsStart;
                        lsEndDt = date + lsEnd;
                    }
                    else
                    {
                        lsStartDt = date + lsStart;
                        lsEndDt = date.AddDays(1) + lsEnd;
                    }

                    var lastCheckDt = lastRec.CheckIn;
                    DateTime normalizedLastCheck = lastCheckDt;
                    if (lsStart > lsEnd && lastCheckDt.TimeOfDay <= lsEnd) normalizedLastCheck = lastCheckDt.Date.AddDays(1) + lastCheckDt.TimeOfDay;

                    int earlyMinutes = 0;
                    double overtimeHours = 0;
                    if (normalizedLastCheck < lsEndDt)
                    {
                        earlyMinutes = (int)Math.Round((lsEndDt - normalizedLastCheck).TotalMinutes);
                        soNgayVeSom++;
                        tongGioVeSom += (lsEndDt - normalizedLastCheck).TotalHours;
                    }
                    else if (normalizedLastCheck > lsEndDt)
                    {
                        overtimeHours = (normalizedLastCheck - lsEndDt).TotalHours;
                    }

                    double workedHours = (ordered.Last().CheckIn - ordered.First().CheckIn).TotalHours;
                    var ngayThongKe = new MobileDto.NgayThongKeDto
                    {
                        Ngay = date,
                        Loai = dayType,
                        FirstCheck = ordered.First().CheckIn,
                        LastCheck = ordered.Last().CheckIn,
                        LateMinutes = lateMinutes,
                        EarlyMinutes = earlyMinutes,
                        OvertimeHours = Math.Round(overtimeHours, 2),
                        WorkedHours = Math.Round(Math.Max(0, workedHours), 2),
                        ShiftName = dayType == "TheoCa" ? (ordered.First().ShiftName ?? ordered.Last().ShiftName) : "Hành chính",
                        WorkMode = dayType
                    };
                    danhSachNgayThongKe.Add(ngayThongKe);
                }

                return new MobileDto.GetLichSuDiemDanhResultDto
                {
                    status = true,
                    message = "Thành công",
                    errorCode = 0,
                    LichSuDiemDanh = listResult,
                    DiMuon = new MobileDto.DiMuon { SoNgayDiMuon = soNgayDiMuon, TongGioDiMuon = Math.Round(tongGioDiMuon, 2) },
                    VeSom = new MobileDto.VeSom { SoNgayVeSom = soNgayVeSom, TongGioVeSom = Math.Round(tongGioVeSom, 2) },
                    SoNgayNghi = absentDays.Count,
                    DanhSachNgayNghi = absentDays,
                    SoNgayLamTheoCa = soNgayLamTheoCa,
                    SoNgayLamHanhChinh = soNgayLamHanhChinh,
                    DanhSachNgayLoai = danhSachNgayLoai,
                    DanhSachNgayThongKe = danhSachNgayThongKe
                };
            }
            catch (Exception ex)
            {
                return new MobileDto.GetLichSuDiemDanhResultDto
                {
                    status = false,
                    message = "Có lỗi xảy ra! " + ex.Message,
                    errorCode = 400
                };
            }
        }
        
        [HttpPost]
        public async Task<MobileDto.ThongKeDiemDanhResultDto> BaoCaoDiemDanh(MobileDto.ThongKeDiemDanhInputDto input)
        {
            var checkToken = CheckValidToken(input.token, input.userid, input.username);
            if (!checkToken.status)
                return new MobileDto.ThongKeDiemDanhResultDto { status = false, errorCode = 401, message = checkToken.message };

            try
            {
                var fromDate = input.tuNgay ?? DateTime.Now.AddMonths(-1);
                var toDate = input.denNgay ?? DateTime.Now;

                var records = await _attendanceRepository.GetAll()
                    .Where(x => x.NguoiBenhId == input.userid && x.CheckIn.Date >= fromDate.Date && x.CheckIn.Date <= toDate.Date)
                    .OrderBy(x => x.CheckIn)
                    .ToListAsync();

                var thongTinDonVi = await _thongTinDonViRepository.GetAll()
                    .Where(x => x.Key == "GioLamViecSang" || x.Key == "GioLamViecChieu")
                    .ToListAsync();

                var gioSangParts = thongTinDonVi.FirstOrDefault(x => x.Key == "GioLamViecSang")?.Value?.Split('-');
                var gioChieuParts = thongTinDonVi.FirstOrDefault(x => x.Key == "GioLamViecChieu")?.Value?.Split('-');
                if (gioSangParts == null || gioChieuParts == null)
                    return new MobileDto.ThongKeDiemDanhResultDto { status = false, errorCode = 412, message = "Chưa cấu hình giờ làm việc" };

                TimeSpan startMorning = TimeSpan.Parse(gioSangParts[0]);
                TimeSpan endMorning = TimeSpan.Parse(gioSangParts[1]);
                TimeSpan startAfternoon = TimeSpan.Parse(gioChieuParts[0]);
                TimeSpan endAfternoon = TimeSpan.Parse(gioChieuParts[1]);

                var grouped = new Dictionary<string, List<Attendance>>();
                foreach (var rec in records)
                {
                    string key = input.loaiBaoCao switch
                    {
                        LOAI_BAO_CAO.NGAY => rec.CheckIn.Date.ToString("yyyy-MM-dd"),
                        LOAI_BAO_CAO.THANG => rec.CheckIn.ToString("MM/yyyy"),
                        LOAI_BAO_CAO.NAM => rec.CheckIn.ToString("yyyy"),
                        LOAI_BAO_CAO.QUY =>
                            $"Q{((rec.CheckIn.Month - 1) / 3 + 1)}/{rec.CheckIn.Year}",
                        _ => rec.CheckIn.Date.ToString("yyyy-MM-dd")
                    };

                    if (!grouped.ContainsKey(key))
                        grouped[key] = new List<Attendance>();
                    grouped[key].Add(rec);
                }

                var resultList = new List<MobileDto.ThongKeDiemDanhTheoKyDto>();

                foreach (var group in grouped.OrderBy(x => x.Key))
                {
                    var list = group.Value.OrderBy(x => x.CheckIn).ToList();
                    int soLanCheckIn = list.Count;
                    int soNgayDiMuon = 0;
                    double tongGioDiMuon = 0;
                    int soNgayVeSom = 0;
                    double tongGioVeSom = 0;

                    var byDate = list.GroupBy(x => x.CheckIn.Date);
                    foreach (var byDay in byDate)
                    {
                        var times = byDay.Select(x => x.CheckIn.TimeOfDay).OrderBy(x => x).ToList();
                        if (times.Count == 1)
                        {
                            if (times[0] >= startMorning && times[0] <= endMorning)
                            {
                                if (times[0] > startMorning)
                                {
                                    soNgayDiMuon++;
                                    tongGioDiMuon += (times[0] - startMorning).TotalHours;
                                }
                            }
                            else if (times[0] >= startAfternoon && times[0] <= endAfternoon)
                            {
                                if (times[0] < endAfternoon)
                                {
                                    soNgayVeSom++;
                                    tongGioVeSom += (endAfternoon - times[0]).TotalHours;
                                }
                            }
                        }
                        else
                        {
                            var sang = times.First();
                            var chieu = times.Last();
                            if (sang >= startMorning && sang <= endMorning)
                            {
                                if (sang > startMorning)
                                {
                                    soNgayDiMuon++;
                                    tongGioDiMuon += (sang - startMorning).TotalHours;
                                }
                            }
                            if (chieu >= startAfternoon && chieu <= endAfternoon)
                            {
                                if (chieu < endAfternoon)
                                {
                                    soNgayVeSom++;
                                    tongGioVeSom += (endAfternoon - chieu).TotalHours;
                                }
                            }
                        }
                    }

                    resultList.Add(new MobileDto.ThongKeDiemDanhTheoKyDto
                    {
                        Ky = group.Key,
                        SoLanCheckIn = soLanCheckIn,
                        SoNgayDiMuon = soNgayDiMuon,
                        TongGioDiMuon = Math.Round(tongGioDiMuon, 2),
                        SoNgayVeSom = soNgayVeSom,
                        TongGioVeSom = Math.Round(tongGioVeSom, 2)
                    });
                }
                int tongNgayDiMuon = resultList.Sum(x => x.SoNgayDiMuon);
                int tongNgayVeSom = resultList.Sum(x => x.SoNgayVeSom);
                int tongNgayDungGio = resultList.Sum(x => x.SoLanCheckIn) - tongNgayDiMuon - tongNgayVeSom;

                return new MobileDto.ThongKeDiemDanhResultDto
                {
                    status = true,
                    errorCode = 0,
                    message = "Thành công",
                    DanhSach = resultList,
                    TongHop = new MobileDto.TongHopDiemDanhDto
                    {
                        TongNgayDiMuon = tongNgayDiMuon,
                        TongNgayVeSom = tongNgayVeSom,
                        TongNgayDungGio = tongNgayDungGio
                    }
                };
            }
            catch (Exception ex)
            {
                return new MobileDto.ThongKeDiemDanhResultDto
                {
                    status = false,
                    errorCode = 500,
                    message = "Lỗi hệ thống: " + ex.Message,
                    DanhSach = null,
                    TongHop = null
                };
            }
        }
        [HttpPost]
        public async Task<MobileDto.CommonResponse> UpdateDeviceToken(MobileDto.UpdateDeviceTokenDto input)
        {
            var checkToken = CheckValidToken(input.token, (int)input.userid, input.username);
            if (!checkToken.status)
                return new MobileDto.CommonResponse { status = false, message = checkToken.message, errorCode = checkToken.errorCode };

            var user = await _nguoiBenhRepository.FirstOrDefaultAsync(input.userid);
            if (user == null)
                return new MobileDto.CommonResponse { status = false, message = "User not found", errorCode = 404 };

            user.DeviceToken = input.devicetoken;
            await _nguoiBenhRepository.UpdateAsync(user);

            return new MobileDto.CommonResponse { status = true, message = "Device token updated", errorCode = 0 };
        }
        
        [HttpPost]
        public async Task<MobileDto.CommonResponse> DeleteDeviceToken(MobileDto.GetMenuDto input)
        {
            var checkToken = CheckValidToken(input.token, (int)input.userid, input.username);
            if (!checkToken.status)
                return new MobileDto.CommonResponse { status = false, message = checkToken.message, errorCode = checkToken.errorCode };

            var user = await _nguoiBenhRepository.FirstOrDefaultAsync(input.userid);
            if (user == null)
                return new MobileDto.CommonResponse { status = false, message = "User not found", errorCode = 404 };

            user.DeviceToken = null;
            await _nguoiBenhRepository.UpdateAsync(user);

            return new MobileDto.CommonResponse { status = true, message = "Device token deleted", errorCode = 0 };
        }
        private async Task<string> ConvertImageUrlToBase64(string imageUrl)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
            {
                throw new ArgumentException("URL ảnh không được để trống");
            }

            try
            {
                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(30);
                    
                    // Kiểm tra URL hợp lệ
                    if (!Uri.TryCreate(imageUrl, UriKind.Absolute, out var uriResult))
                    {
                        throw new ArgumentException("URL ảnh không hợp lệ");
                    }

                    // Thêm User-Agent để tránh bị chặn
                    httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0");

                    // Tải dữ liệu với header để kiểm tra
                    using (var response = await httpClient.GetAsync(imageUrl, HttpCompletionOption.ResponseHeadersRead))
                    {
                        // Kiểm tra status code
                        if (!response.IsSuccessStatusCode)
                        {
                            var errorContent = await response.Content.ReadAsStringAsync();
                            throw new Exception($"Server trả về lỗi: {response.StatusCode}. Nội dung: {errorContent.Truncate(100)}");
                        }

                        // Kiểm tra Content-Type
                        var contentType = response.Content.Headers.ContentType?.MediaType?.ToLower();
                        if (contentType == "text/html")
                        {
                            // Đọc nội dung để xác định lỗi cụ thể
                            var content = await response.Content.ReadAsStringAsync();
                            if (content.Contains("404") || content.Contains("Not Found"))
                            {
                                throw new Exception("Ảnh không tồn tại (404)");
                            }
                            throw new Exception("URL không trỏ đến file ảnh trực tiếp");
                        }

                        if (!IsSupportedImageType(contentType))
                        {
                            throw new Exception($"Định dạng ảnh không được hỗ trợ: {contentType}");
                        }

                        // Đọc dữ liệu ảnh
                        var imageBytes = await response.Content.ReadAsByteArrayAsync();
                        
                        if (imageBytes == null || imageBytes.Length == 0)
                        {
                            throw new Exception("Dữ liệu ảnh trống");
                        }

                        // Kiểm tra signature của file ảnh
                        if (!IsValidImageSignature(imageBytes))
                        {
                            throw new Exception("File không phải là ảnh hợp lệ (kiểm tra signature thất bại)");
                        }

                        return Convert.ToBase64String(imageBytes);
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                throw new Exception($"Lỗi kết nối: {ex.Message}");
            }
            catch (TaskCanceledException)
            {
                throw new Exception("Timeout khi tải ảnh");
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi xử lý: {ex.Message}");
            }
        }
        private bool IsSupportedImageType(string contentType)
        {
            if (string.IsNullOrWhiteSpace(contentType)) return false;
            
            var supportedTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/bmp" };
            return supportedTypes.Contains(contentType.ToLower());
        }
        private bool IsValidImageSignature(byte[] bytes)
        {
            if (bytes == null || bytes.Length < 8) return false;

            // JPEG
            if (bytes[0] == 0xFF && bytes[1] == 0xD8 && bytes[2] == 0xFF)
                return true;
            
            // PNG
            if (bytes[0] == 0x89 && bytes[1] == 0x50 && bytes[2] == 0x4E && bytes[3] == 0x47)
                return true;
            
            // GIF
            if (bytes[0] == 0x47 && bytes[1] == 0x49 && bytes[2] == 0x46)
                return true;
            
            // BMP
            if (bytes[0] == 0x42 && bytes[1] == 0x4D)
                return true;

            return false;
        }
        private bool CompareImages(string base64Input, string base64Stored, out double similarity, double threshold = 0.4)
        {
            similarity = 0;

            try
            {
                // Kiểm tra đầu vào
                if (string.IsNullOrWhiteSpace(base64Input))
                    throw new ArgumentException("Input image is empty");
                if (string.IsNullOrWhiteSpace(base64Stored))
                    throw new ArgumentException("Stored image is empty");

                using var detector = Dlib.GetFrontalFaceDetector();

                var wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Common", "AI");
                var shapeBz2Path = Path.Combine(wwwRootPath, "shape_predictor_68_face_landmarks.dat.bz2");
                var faceBz2Path = Path.Combine(wwwRootPath, "dlib_face_recognition_resnet_model_v1.dat.bz2");
                var shapeDatPath = Path.Combine(wwwRootPath, "shape_predictor_68_face_landmarks.dat");
                var faceDatPath = Path.Combine(wwwRootPath, "dlib_face_recognition_resnet_model_v1.dat");

                if (!File.Exists(shapeDatPath))
                    DecompressBz2(shapeBz2Path, shapeDatPath);

                if (!File.Exists(faceDatPath))
                    DecompressBz2(faceBz2Path, faceDatPath);

                using var shapePredictor = ShapePredictor.Deserialize(shapeDatPath);
                using var faceRecognitionModel = LossMetric.Deserialize(faceDatPath);

                Matrix<RgbPixel> img1 = null, img2 = null;
                try
                {
                    img1 = LoadImageFromBase64(base64Input);
                    img2 = LoadImageFromBase64(base64Stored);
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to load images: " + ex.Message);
                }

                var faces1 = detector.Operator(img1);
                var faces2 = detector.Operator(img2);

                if (faces1.Length == 0 || faces2.Length == 0)
                    return false;

                var shape1 = shapePredictor.Detect(img1, faces1[0]);
                var shape2 = shapePredictor.Detect(img2, faces2[0]);

                var chip1 = Dlib.ExtractImageChip<RgbPixel>(img1, Dlib.GetFaceChipDetails(shape1, 150, 0.25));
                var chip2 = Dlib.ExtractImageChip<RgbPixel>(img2, Dlib.GetFaceChipDetails(shape2, 150, 0.25));

                var vec1 = faceRecognitionModel.Operator(new[] { chip1 })[0];
                var vec2 = faceRecognitionModel.Operator(new[] { chip2 })[0];

                similarity = ComputeEuclideanDistance(vec1, vec2);
                return similarity < threshold;
            }
            catch (Exception ex)
            {
                // Log the error if needed
                Console.WriteLine($"Error comparing images: {ex.Message}");
                return false;
            }
        }

        private void DecompressBz2(string sourcePath, string targetPath)
        {
            using var fileStream = File.OpenRead(sourcePath);
            using var bz2 = new ICSharpCode.SharpZipLib.BZip2.BZip2InputStream(fileStream);
            using var outFile = File.Create(targetPath);
            bz2.CopyTo(outFile);
        }
        private static double ComputeEuclideanDistance(Matrix<float> v1, Matrix<float> v2)
        {
            double sum = 0;
            for (int i = 0; i < v1.Size; i++)
            {
                var diff = v1[i] - v2[i];
                sum += diff * diff;
            }
            return Math.Sqrt(sum);
        }
        private double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
        {
            const double R = 6371; // bán kính Trái Đất (km)
            var dLat = ToRadians(lat2 - lat1);
            var dLon = ToRadians(lon2 - lon1);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(lat1)) * Math.Cos(ToRadians(lat2)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        private double ToRadians(double deg) => deg * (Math.PI / 180);
        
        private static Matrix<RgbPixel> LoadImageFromBase64(string base64)
        {
            if (string.IsNullOrWhiteSpace(base64))
                throw new ArgumentException("Base64 string cannot be null or empty");

            try
            {
                var imageBytes = Convert.FromBase64String(base64);
                if (imageBytes == null || imageBytes.Length == 0)
                    throw new ArgumentException("Invalid image data");

                using var ms = new MemoryStream(imageBytes);
                using var bmp = new Bitmap(ms);
        
                if (bmp.Width <= 0 || bmp.Height <= 0)
                    throw new ArgumentException("Invalid image dimensions");

                return bmp.ToMatrix<RgbPixel>();
            }
            catch (FormatException)
            {
                throw new ArgumentException("Invalid Base64 format");
            }
            catch (ArgumentException ex)
            {
                throw new ArgumentException("Invalid image data: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to load image: " + ex.Message);
            }
        }
        // public Task StartAsync(CancellationToken cancellationToken)
        // {
        //     // Timer sẽ gọi DoWork mỗi phút
        //     _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        //     return Task.CompletedTask;
        // }
        //
        // private void DoWork(object state)
        // {
        //     if (_isRunning) return;
        //     _isRunning = true;
        //     try
        //     {
        //         DoWorkAsync().GetAwaiter().GetResult();
        //     }
        //     catch (Exception ex)
        //     {
        //         // Log lỗi tại đây nếu muốn
        //         Console.WriteLine($"Error in DoWork: {ex.Message}");
        //     }
        //     finally
        //     {
        //         _isRunning = false;
        //     }
        // }
        //
        // private async Task DoWorkAsync()
        // {
        //     using (var scope = _scopeFactory.CreateScope())
        //     {
        //         var now = TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time"));
        //
        //         if (now.Hour == 15 && now.Minute == 37)
        //         {
        //             await SendAttendanceNotification(scope.ServiceProvider, "morning");
        //         }
        //         if (now.Hour == 17 && now.Minute == 20)
        //         {
        //             await SendAttendanceNotification(scope.ServiceProvider, "evening");
        //         }
        //     }
        // }
        //
        // private async Task SendAttendanceNotification(IServiceProvider serviceProvider, string type)
        // {
        //     var nguoiBenhRepository = serviceProvider.GetRequiredService<IRepository<NguoiBenh>>();
        //
        //     string title, body;
        //     if (type == "morning")
        //     {
        //         title = "Thông báo điểm danh";
        //         body = "Đã đến giờ điểm danh, vui lòng điểm danh vào giờ làm!";
        //     }
        //     else if (type == "evening")
        //     {
        //         title = "Thông báo điểm danh";
        //         body = "Vui lòng chấm công giờ về!";
        //     }
        //     else
        //     {
        //         return;
        //     }
        //
        //     var deviceTokens = await nguoiBenhRepository.GetAll()
        //         .Where(x => x.IsNhanVien && !string.IsNullOrEmpty(x.DeviceToken))
        //         .Select(x => x.DeviceToken)
        //         .ToListAsync();
        //
        //     int sendSuccess = 0, sendFail = 0;
        //
        //     foreach (var token in deviceTokens)
        //     {
        //         try
        //         {
        //             await FirebaseHelper.SendFCM(token, title, body);
        //             sendSuccess++;
        //         }
        //         catch
        //         {
        //             sendFail++;
        //         }
        //     }
        //     Console.WriteLine($"[{DateTime.Now}] {type}: Success {sendSuccess}, Fail {sendFail}");
        // }
        //
        // public Task StopAsync(CancellationToken cancellationToken)
        // {
        //     _timer?.Change(Timeout.Infinite, 0);
        //     return Task.CompletedTask;
        // }
        //
        // public void Dispose()
        // {
        //     _timer?.Dispose();
        // }
    }
}