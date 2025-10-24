using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Json;
using Abp.Net.Mail;
using Karion.BusinessSolution.Authorization.Users;
using Karion.BusinessSolution.Configuration;
using Karion.BusinessSolution.Dto;
using Karion.BusinessSolution.Extension.Karion.BusinessSolution.Common;
using Karion.BusinessSolution.MobileAppServices;
using Karion.BusinessSolution.MultiTenancy;
using Karion.BusinessSolution.QuanLyDanhMuc;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.VersionControl;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Karion.BusinessSolution.MobileAppServices.MobileAppServices.JwtBearerMobile;

namespace Karion.BusinessSolution.PortalAppServices.PortalAppServices
{
    
    public class PortalAppServices : BusinessSolutionAppServiceBase, IPortalAppServices
    {
        private readonly IRepository<NguoiBenh> _nguoiBenhRepository;
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
        private readonly IRepository<PublicToken,long> _publicTokenRepository;
        private readonly IHanetAppservices _hanetAppservices;

        public PortalAppServices(IRepository<NguoiBenh> nguoiBenhRepository,
            IRepository<NguoiThan> nguoiThanRepository,
            IRepository<ThongTinDonVi> thongTinDonViRepository,
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
            IRepository<PublicToken,long> publicTokenRepository,
            IHanetAppservices hanetAppservices
        )
        {
            _nguoiBenhRepository = nguoiBenhRepository;
            _nguoiThanRepository = nguoiThanRepository;
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
            _publicTokenRepository = publicTokenRepository;
        }

        private MobileDto.CheckTokenDto CheckValidToken(string token)
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                PublicToken tokencheck = _publicTokenRepository.FirstOrDefault(p => p.Token == token && !p.IsTokenLocked);
                if (tokencheck.isNull())
                {
                    return new MobileDto.CheckTokenDto()
                    {
                        status = false,
                        message = "Invalid Token!"
                    };
                }

                return new MobileDto.CheckTokenDto()
                {
                    status = true,
                    message = tokencheck.TenantId.ToString()
                };
            }
          
        }

      
        

        [HttpPost]
        //[MobileAuthorization]
        public async Task<PortalDto.PortalGetListChuyenKhoaResult> PortalGetListChuyenKhoa(PortalDto.PortalGetListChuyenKhoaInput model)
        {
            MobileDto.CheckTokenDto checkTokenDto = CheckValidToken(model.token);
            if (!checkTokenDto.status)
            {
                return new PortalDto.PortalGetListChuyenKhoaResult();
            }

            _unitOfWorkManager.Current.SetTenantId(Int32.Parse(checkTokenDto.message));
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

            return new PortalDto.PortalGetListChuyenKhoaResult()
            {
                ChuyenKhoaDtos = await chuyenKhoas.ToListAsync()
            };
        }

        [HttpPost]
        public async Task<MobileDto.CommonResponse> PortalRegister(PortalDto.PortalRegisterInput input)
        {
            MobileDto.CheckTokenDto checkTokenDto = CheckValidToken(input.token);
            if (!checkTokenDto.status)
            {
                return new MobileDto.CommonResponse()
                {
                    status = false,
                    message = "Lỗi thông tin xác thực không chính xác!"
                };
            }
         
            NguoiBenh nguoiBenh = _nguoiBenhRepository.FirstOrDefault(p => p.UserName.Equals(input.cccd));
            if (nguoiBenh.isNull())
            {
                var confirmationCode = (input.cccd.clearSpace().ToLower() + input.sdt.ToMd5()).ToMd5();
                var newNguoiBenh = new NguoiBenh() {
                    Password = input.sdt.ToMd5(),
                    NgaySinh = 1,
                    ThangSinh = 1,
                    NamSinh = 1990,
                    DiaChi = "Chưa cập nhật",
                    GioiTinh = "Nam",
                    CreationTime = DateTime.Now,
                    IsActive = true,
                    IsDeleted = false,
                    IsPhoneNumberConfirmed = true,
                    EmailAddress = "Chưa cập nhật",
                    PhoneNumber = input.sdt,
                    TokenExpire = DateTime.Now,
                    UserName = input.cccd,
                    AccessFailedCount = 0,
                    HoVaTen = input.hovaten,
                    IsEmailConfirmed = false,
                    EmailConfirmationCode = confirmationCode,
                    SoTheBHYT = "Chưa cập nhật",
                    NoiDkBanDau = "Chưa cập nhật",
                    GiaTriSuDungTuNgay = DateTime.Now
                };
                await _nguoiBenhRepository.InsertAsync(newNguoiBenh);
                await _unitOfWorkManager.Current.SaveChangesAsync();
                _unitOfWorkManager.Current.SetTenantId(Int32.Parse(checkTokenDto.message));
                await _lichHenKhamRepository.InsertAsync(new LichHenKham()
                {
                    CreationTime = DateTime.Now,
                    IsDeleted = false,
                    TenantId = _unitOfWorkManager.Current.GetTenantId(),
                    BacSiId = null,
                    ChuyenKhoaId = input.chuyenkhoa,
                    IsDaKham = false,
                    NgayHenKham = input.ngaygiohenkham,
                    NguoiThanId = null,
                    MoTaTrieuChung = input.motatrieuchung,
                    IsDaThanhToan = false,
                    IsCoBHYT = false,
                    PhuongThucThanhToan = 1,
                    BHYTValidDate = DateTime.Now,
                    NoiDangKyKCBDauTien = "Không",
                    SoTheBHYT = "Không",
                    NguoiBenhId = newNguoiBenh.Id,
                    Flag = 1
                });
                return new MobileDto.CommonResponse()
                {
                    status = true,
                    message = "Đăng ký thành công, bạn đã được tạo tài khoản đăng nhập hệ thống với tài khoản là CCCD/CMND của bạn, mật khẩu là số điện thoại của bạn, vui lòng tải ứng dụng mobile nếu có nhu cầu đăng nhập kiểm tra!"
                };
            }
            _unitOfWorkManager.Current.SetTenantId(Int32.Parse(checkTokenDto.message));
            await _lichHenKhamRepository.InsertAsync(new LichHenKham()
            {
                CreationTime = DateTime.Now,
                IsDeleted = false,
                TenantId = _unitOfWorkManager.Current.GetTenantId(),
                BacSiId = null,
                ChuyenKhoaId = input.chuyenkhoa,
                IsDaKham = false,
                NgayHenKham = input.ngaygiohenkham,
                NguoiThanId = null,
                MoTaTrieuChung = input.motatrieuchung,
                IsDaThanhToan = false,
                IsCoBHYT = false,
                PhuongThucThanhToan = 1,
                BHYTValidDate = DateTime.Now,
                NoiDangKyKCBDauTien = "Không",
                SoTheBHYT = "Không",
                NguoiBenhId = nguoiBenh.Id,
                Flag = 1
            });
            return new MobileDto.CommonResponse()
            {
                status = true,
                message = "Đăng ký thành công vui lòng kiểm tra và theo dõi trên ứng dụng mobile!"
            };
        }
        
        
    }
}