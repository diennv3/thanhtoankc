using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Karion.BusinessSolution.Web.Areas.App.Models.LichHenKhams;
using Karion.BusinessSolution.Web.Controllers;
using Karion.BusinessSolution.Authorization;
using Karion.BusinessSolution.QuanLyDanhMuc;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Karion.BusinessSolution.Dto;
using Karion.BusinessSolution.Extension.Karion.BusinessSolution.Common;
using Microsoft.EntityFrameworkCore;

namespace Karion.BusinessSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_LichHenKhams)]
    public class LichHenKhamsController : BusinessSolutionControllerBase
    {
        private readonly ILichHenKhamsAppService _lichHenKhamsAppService;
        private readonly IRepository<LichHenKham> _LHKrepository;
        private readonly IRepository<ThongTinDonVi> _thongTinDonVirepository;
        private readonly IRepository<NguoiBenh> _nguoiBenhRepository;
        private readonly IRepository<LichHenKham> _lichHenKhamRepository;
        private readonly IRepository<ChiTietThanhToan> _chiTietThanhToanRepository;

        public LichHenKhamsController(ILichHenKhamsAppService lichHenKhamsAppService, 
            IRepository<LichHenKham> LHKrepository,
            IRepository<ThongTinDonVi> thongTinDonVirepository,
            IRepository<NguoiBenh> nguoiBenhRepository,
            IRepository<LichHenKham> lichHenKhamRepository,
            IRepository<ChiTietThanhToan> chiTietThanhToanRepository
            )
        {
            _lichHenKhamsAppService = lichHenKhamsAppService;
            _LHKrepository = LHKrepository;
            _thongTinDonVirepository = thongTinDonVirepository;
            _nguoiBenhRepository = nguoiBenhRepository;
            _lichHenKhamRepository = lichHenKhamRepository;
            _chiTietThanhToanRepository = chiTietThanhToanRepository;
        }

        //public ActionResult Index()
        //{
        //    var model = new LichHenKhamsViewModel
        //    {
        //        FilterText = ""
        //    };

        //    return View(model);
        //}

        [AbpMvcAuthorize(AppPermissions.Pages_TiepNhanBenhNhan_LeTan)]
        public ActionResult LeTan()
        {
            var model = new LichHenKhamsViewModel
            {
                FilterText = ""
            };
            return View(model);
        }

        public ActionResult BacSiChiDinh()
        {
            var model = new LichHenKhamsViewModel
            {
                FilterText = ""
            };
            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_TiepNhanBenhNhan_KhuVucThanhToan)]
        public ActionResult ThuNgan()
        {
            var model = new LichHenKhamsViewModel
            {
                FilterText = ""
            };
            return View(model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_LichHenKhams_Create, AppPermissions.Pages_LichHenKhams_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetLichHenKhamForEditOutput getLichHenKhamForEditOutput;
            if (id.HasValue)
            {
                getLichHenKhamForEditOutput =
                    await _lichHenKhamsAppService.GetLichHenKhamForEdit(new EntityDto {Id = (int) id});
            }
            else
            {
                getLichHenKhamForEditOutput = new GetLichHenKhamForEditOutput
                {
                    LichHenKham = new CreateOrEditLichHenKhamDto()
                };
                getLichHenKhamForEditOutput.LichHenKham.NgayHenKham = DateTime.Now;
                getLichHenKhamForEditOutput.LichHenKham.BHYTValidDate = DateTime.Now;
                getLichHenKhamForEditOutput.LichHenKham.TimeHoanThanhKham = DateTime.Now;
                getLichHenKhamForEditOutput.LichHenKham.TimeHoanThanhThanhToan = DateTime.Now;
            }

            var viewModel = new CreateOrEditLichHenKhamModalViewModel()
            {
                LichHenKham = getLichHenKhamForEditOutput.LichHenKham,
                UserName = getLichHenKhamForEditOutput.UserName,
                UserName2 = getLichHenKhamForEditOutput.UserName2,
                NguoiBenhUserName = getLichHenKhamForEditOutput.NguoiBenhUserName,
                NguoiThanHoVaTen = getLichHenKhamForEditOutput.NguoiThanHoVaTen,
                ChuyenKhoaTen = getLichHenKhamForEditOutput.ChuyenKhoaTen,
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }

        public async Task<PartialViewResult> FacesDetect()
        {
            var viewModel = await _lichHenKhamsAppService.GetDetectedFacesHenKham();
            
            return PartialView("_FaceDetectsModal", viewModel);
        }

        public async Task<PartialViewResult> ViewLichHenKhamModal(int id)
        {
            // var getLichHenKhamForViewDto = await _lichHenKhamsAppService.GetLichHenKhamForView(id);
            var lichHenKham =  await _lichHenKhamRepository.FirstOrDefaultAsync(id);

            var detail = await (from o in _lichHenKhamRepository.GetAll().WhereIf(true, p => p.Id == id)
                join o1 in _nguoiBenhRepository.GetAll() on o.NguoiBenhId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()
                select new LichHenKhamForViewDto()
                {
                    LichHenKham = new LichHenKhamDto()
                    {
                      TongChiPhi  = o.TongChiPhi,
                      IsTamUng = o.IsTamUng,
                      TongTienThanhToan = o.TongTienDaThanhToan,
                      Id = o.Id
                    },
                    NguoiBenh = new NguoiBenhDto()
                    {
                        HoVaTen = s1.HoVaTen,
                        DiaChi = s1.DiaChi,
                        GioiTinh = s1.GioiTinh,
                        NamSinh = s1.NamSinh,
                        ThangSinh = s1.ThangSinh,
                        NgaySinh = s1.NgaySinh,
                        UserName = s1.UserName,
                        PhoneNumber = s1.PhoneNumber,
                        ProfilePicture = s1.ProfilePicture
                    }
                    
                }).ToListAsync(); 
            
            var chiTietThanhToan = await _chiTietThanhToanRepository.GetAll()
                .WhereIf(!lichHenKham.Id.isNull(), p => p.LichHenKhamId == lichHenKham.Id)
                .ToListAsync();

            var model = new LichHenKhamViewModel()
            {
                Detail = ObjectMapper.Map<List<LichHenKhamForViewDto>>(detail),
                ChiTietThanhToan = ObjectMapper.Map<List<ChiTietThanhToanDto>>(chiTietThanhToan)
            };

            return PartialView("_ViewLichHenKhamModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_LichHenKhams_Create, AppPermissions.Pages_LichHenKhams_Edit)]
        public PartialViewResult UserLookupTableModal(long? id, string displayName)
        {
            var viewModel = new LichHenKhamUserLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_LichHenKhamUserLookupTableModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_LichHenKhams_Create, AppPermissions.Pages_LichHenKhams_Edit)]
        public PartialViewResult NguoiBenhLookupTableModal(int? id, string displayName)
        {
            var viewModel = new LichHenKhamNguoiBenhLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_LichHenKhamNguoiBenhLookupTableModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_LichHenKhams_Create, AppPermissions.Pages_LichHenKhams_Edit)]
        public PartialViewResult NguoiThanLookupTableModal(int? id, string displayName)
        {
            var viewModel = new LichHenKhamNguoiThanLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_LichHenKhamNguoiThanLookupTableModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_LichHenKhams_Create, AppPermissions.Pages_LichHenKhams_Edit)]
        public PartialViewResult ChuyenKhoaLookupTableModal(int? id, string displayName)
        {
            var viewModel = new LichHenKhamChuyenKhoaLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_LichHenKhamChuyenKhoaLookupTableModal", viewModel);
        }

        public PartialViewResult DanhSachDichVuModal(int id, int chuyenKhoaId, string serializedDichVu, int flag)
        {
            LichHenKham lichHenKham = _LHKrepository.FirstOrDefault(id);
            if(!lichHenKham.isNull())
            {
                var viewModel = new DanhSachDichVuHenKhamModel()
                {
                    Id = id,
                    ChuyenKhoaId = chuyenKhoaId,
                    SerializedDichVu = serializedDichVu,
                    Flag = flag,
                    QRString = lichHenKham.QRString
                };

                return PartialView("_DanhSachDichVuModal", viewModel);
            }
            else
            {
                var viewModel = new DanhSachDichVuHenKhamModel();

                return PartialView("_DanhSachDichVuModal", viewModel);
            }
        }
    }
}