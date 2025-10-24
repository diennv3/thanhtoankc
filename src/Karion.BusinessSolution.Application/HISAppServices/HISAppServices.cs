using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Karion.BusinessSolution.Authorization;
using Karion.BusinessSolution.Dto;
using Karion.BusinessSolution.Extension.Karion.BusinessSolution.Common;
using Karion.BusinessSolution.HISConnect;
using Karion.BusinessSolution.QuanLyDanhMuc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Karion.BusinessSolution.HISAppServices.HISAppServices
{
    [AbpAuthorize(AppPermissions.HISApiAccess)]
    public class HISAppServices : BusinessSolutionAppServiceBase, IHISAppServices
    {
        private readonly IRepository<LichHenKham> _lichHenKhamRepository;
        private readonly IRepository<NguoiBenh> _nguoiBenhRepository;
        private readonly IRepository<NguoiThan> _nguoiThanRepository;
        private readonly IRepository<ChuyenKhoa> _chuyenKhoaRepository;
        

        public HISAppServices(
            IRepository<LichHenKham> lichHenKhamRepository,
            IRepository<NguoiBenh> nguoiBenhRepository,
            IRepository<NguoiThan> nguoiThanRepository,
            IRepository<ChuyenKhoa> chuyenKhoaRepository
        )
        {
            _lichHenKhamRepository = lichHenKhamRepository;
            _nguoiBenhRepository = nguoiBenhRepository;
            _nguoiThanRepository = nguoiThanRepository;
            _chuyenKhoaRepository = chuyenKhoaRepository;
        }

        [HttpPost]
        public async Task<List<HISDtoGetAllRegistrationResult>> GetAllRegistrationList(
            HISDtoGetAllRegistrationInput input)
        {
            List<HISDtoGetAllRegistrationResult> results = new List<HISDtoGetAllRegistrationResult>();


            NguoiBenh nguoiBenh = null;
            if (!input.cmnd.IsNullOrEmpty())
            {
                nguoiBenh = await _nguoiBenhRepository.FirstOrDefaultAsync(p => p.UserName.Equals(input.cmnd));
                if (nguoiBenh.IsNullOrDeleted())
                {
                    throw new UserFriendlyException("Người đăng ký có CMND " + input.cmnd +
                                                    " không tồn tại trên hệ thống");
                }
            }
            
            var filtered = _lichHenKhamRepository.GetAll()
                .WhereIf(true, p => !p.IsHISGet)
                .WhereIf(input.FromDate.HasValue, p => p.NgayHenKham.Date >= input.FromDate.Value.Date)
                .WhereIf(input.ToDate.HasValue, p => p.NgayHenKham.Date <= input.ToDate.Value.Date)
                .WhereIf(!nguoiBenh.IsNullOrDeleted(), p => p.NguoiBenhId == nguoiBenh.Id);

           

            var containers = from o in filtered
                join o1 in _nguoiBenhRepository.GetAll() on o.NguoiBenhId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()
                join o2 in _chuyenKhoaRepository.GetAll() on o.ChuyenKhoaId equals o2.Id into j2
                from s2 in j2.DefaultIfEmpty()
                select new HISDtoGetAllRegistrationResult()
                {
                    NguoiDangKy = new HISNguoiBenhDto()
                    {
                        Id = s1.Id,
                        DiaChi = s1.DiaChi,
                        EmailAddress = s1.EmailAddress,
                        GioiTinh = s1.GioiTinh,
                        NamSinh = s1.NgaySinh,
                        ThangSinh = s1.ThangSinh,
                        NgaySinh = s1.NgaySinh,
                        PhoneNumber = s1.PhoneNumber,
                        HoVaTen = s1.HoVaTen,
                        CMND = s1.UserName
                    },
                    DangKyHoNguoiThanId = o.NguoiThanId,
                    RegistrationId = o.Id,
                    NgayHenKham = o.NgayHenKham,
                    ChuyenKhoaName = s2.Ten,
                    MoTaTrieuChung = o.MoTaTrieuChung
                };

            results = await containers.ToListAsync();
            foreach (var VARIABLE in results)
            {
                if (VARIABLE.DangKyHoNguoiThanId.HasValue)
                {
                    NguoiThan nguoiThan =
                        await _nguoiThanRepository.FirstOrDefaultAsync(p => p.Id == VARIABLE.DangKyHoNguoiThanId);
                    VARIABLE.DangKyHoNguoiThan = new HISNguoiThanDto()
                    {
                        Tuoi = nguoiThan.Tuoi,
                        DiaChi = nguoiThan.DiaChi,
                        GioiTinh = nguoiThan.GioiTinh,
                        HoVaTen = nguoiThan.HoVaTen,
                        MoiQuanHe = nguoiThan.MoiQuanHe,
                        SoDienThoai = nguoiThan.SoDienThoai
                    };
                }
                else
                {
                    VARIABLE.DangKyHoNguoiThan = null;
                }
            }
            return results;
        }

        [HttpPost]
        public async Task<ConfirmRegistrationSavedResponse> ConfirmRegistrationSaved(ConfirmRegistrationSavedInput input)
        {
            if (input.RegistrationIds.isEmpty())
            {
                return new ConfirmRegistrationSavedResponse()
                {
                    status = false,
                    message = "Không có bản ghi nào được truyền - No record in request"
                };
            }

            List<LichHenKham> lichHenKhams = await _lichHenKhamRepository.GetAll()
                .WhereIf(true, p => input.RegistrationIds.Contains(p.Id))
                .ToListAsync();
            foreach (var VARIABLE in lichHenKhams)
            {
                VARIABLE.IsHISGet = true;
                await _lichHenKhamRepository.UpdateAsync(VARIABLE);
            }
            return new ConfirmRegistrationSavedResponse()
            {
                message = "OK",
                status = true
            };
        }

        [HttpPost]
        public async Task<UpdatePaymentAmountResponse> UpdatePaymentAmount(
            UpdatePaymentAmountInput input)
        {
            UpdatePaymentAmountResponse result = new UpdatePaymentAmountResponse()
            {
                items = new List<UpdatePaymentAmountResponseItem>()
            };
            foreach (var VARIABLE in input.items)
            {
                LichHenKham lichHenKham = await _lichHenKhamRepository.FirstOrDefaultAsync(VARIABLE.RegistrationId);
                if (lichHenKham.IsNullOrDeleted())
                {
                    result.items.Add(new UpdatePaymentAmountResponseItem()
                    {
                        Amount = VARIABLE.Amount,
                        Messenger = "Không tìm thấy bản ghi đăng ký khám có mã "+VARIABLE.RegistrationId+" - registration ID "+VARIABLE.RegistrationId+" not found",
                        Status = false,
                        RegistrationId = VARIABLE.RegistrationId,
                        QRHashCode = ""
                    });
                }
                else
                {
                    lichHenKham.TongChiPhi = VARIABLE.Amount;
                    await _lichHenKhamRepository.UpdateAsync(lichHenKham);
                    result.items.Add(new UpdatePaymentAmountResponseItem()
                    {
                        Amount = VARIABLE.Amount,
                        Messenger = "OK",
                        Status = true,
                        RegistrationId = VARIABLE.RegistrationId,
                        QRHashCode = "00020101021238580010A000000727012800069704220114888886636888880208QRIBFTTA530370454062000005802VN62180814TBR_TTKC 27 2763041375"
                    });
                    //TODO: thêm generate QR vào đây xong cho vào  qrhashcode
                }
            }

            return result;
        }
    }
}