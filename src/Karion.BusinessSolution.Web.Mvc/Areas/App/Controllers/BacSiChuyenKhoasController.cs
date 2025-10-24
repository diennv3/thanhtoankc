using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Karion.BusinessSolution.Web.Areas.App.Models.BacSiChuyenKhoas;
using Karion.BusinessSolution.Web.Controllers;
using Karion.BusinessSolution.Authorization;
using Karion.BusinessSolution.QuanLyDanhMuc;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using System.Linq;
using Abp.AspNetZeroCore.Net;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Karion.BusinessSolution.Dto;
using Karion.BusinessSolution.Extension.Karion.BusinessSolution.Common;
using Karion.BusinessSolution.Storage;

namespace Karion.BusinessSolution.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_BacSiChuyenKhoas)]
    public class BacSiChuyenKhoasController : BusinessSolutionControllerBase
    {
        private readonly IBacSiChuyenKhoasAppService _bacSiChuyenKhoasAppService;
        private readonly IRepository<ThongTinBacSiMoRong> _thongTinBacSiMoRongRepository;
        private readonly IRepository<BacSiChuyenKhoa> _bacSiChuyenKhoaRepository;
        private readonly IBinaryObjectManager _binaryObjectManager;

        public BacSiChuyenKhoasController(IBacSiChuyenKhoasAppService bacSiChuyenKhoasAppService,
                                          IRepository<ThongTinBacSiMoRong> thongTinBacSiMoRongRepository,
                                          IBinaryObjectManager binaryObjectManager,
                                          IRepository<BacSiChuyenKhoa> bacSiChuyenKhoaRepository
            )
        {
            _bacSiChuyenKhoasAppService = bacSiChuyenKhoasAppService;
            _thongTinBacSiMoRongRepository = thongTinBacSiMoRongRepository;
            _bacSiChuyenKhoaRepository = bacSiChuyenKhoaRepository;
            _binaryObjectManager = binaryObjectManager;
        }

        public ActionResult Index()
        {
            var model = new BacSiChuyenKhoasViewModel
            {
                FilterText = ""
            };

            return View(model);
        }


        [AbpMvcAuthorize(AppPermissions.Pages_BacSiChuyenKhoas_Create, AppPermissions.Pages_BacSiChuyenKhoas_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetBacSiChuyenKhoaForEditOutput getBacSiChuyenKhoaForEditOutput;
            ThongTinBacSiMoRongDto thongTinBacSiMoRongDto = new ThongTinBacSiMoRongDto();
            if (id.HasValue)
            {
                getBacSiChuyenKhoaForEditOutput =
                    await _bacSiChuyenKhoasAppService.GetBacSiChuyenKhoaForEdit(new EntityDto {Id = (int) id});
                var thongTinBacSiMoRong = await _thongTinBacSiMoRongRepository
                    .FirstOrDefaultAsync(p => p.UserId == getBacSiChuyenKhoaForEditOutput.BacSiChuyenKhoa.UserId);
                if (!thongTinBacSiMoRong.isNull())
                {
                    thongTinBacSiMoRongDto = ObjectMapper.Map<ThongTinBacSiMoRongDto>(thongTinBacSiMoRong);
                }
            }
            else
            {
                getBacSiChuyenKhoaForEditOutput = new GetBacSiChuyenKhoaForEditOutput
                {
                    BacSiChuyenKhoa = new CreateOrEditBacSiChuyenKhoaDto()
                };
            }
            var viewModel = new CreateOrEditBacSiChuyenKhoaModalViewModel()
            {
                BacSiChuyenKhoa = getBacSiChuyenKhoaForEditOutput.BacSiChuyenKhoa,
                UserName = getBacSiChuyenKhoaForEditOutput.UserName,
                ChuyenKhoaTen = getBacSiChuyenKhoaForEditOutput.ChuyenKhoaTen,
                ThongTinBacSiMoRong = thongTinBacSiMoRongDto
            };
            
            return PartialView("_CreateOrEditModal", viewModel);
        }


        public async Task<PartialViewResult> ViewBacSiChuyenKhoaModal(int id)
        {
            var getBacSiChuyenKhoaForViewDto = await _bacSiChuyenKhoasAppService.GetBacSiChuyenKhoaForView(id);

            var model = new BacSiChuyenKhoaViewModel()
            {
                BacSiChuyenKhoa = getBacSiChuyenKhoaForViewDto.BacSiChuyenKhoa,
                UserName = getBacSiChuyenKhoaForViewDto.UserName,
                ChuyenKhoaTen = getBacSiChuyenKhoaForViewDto.ChuyenKhoaTen
            };

            return PartialView("_ViewBacSiChuyenKhoaModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_BacSiChuyenKhoas_Create, AppPermissions.Pages_BacSiChuyenKhoas_Edit)]
        public PartialViewResult UserLookupTableModal(long? id, string displayName)
        {
            var viewModel = new BacSiChuyenKhoaUserLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_BacSiChuyenKhoaUserLookupTableModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_BacSiChuyenKhoas_Create, AppPermissions.Pages_BacSiChuyenKhoas_Edit)]
        public PartialViewResult ChuyenKhoaLookupTableModal(int? id, string displayName)
        {
            var viewModel = new BacSiChuyenKhoaChuyenKhoaLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_BacSiChuyenKhoaChuyenKhoaLookupTableModal", viewModel);
        }
        
        public PartialViewResult CapNhatAnhBacSiModal(int thongTinBacSiMoRongId)
        {
            var viewModel = new CapNhatAnhBacSiViewModel()
            {
                ThongTinBacSiMoRongId = thongTinBacSiMoRongId
            };
            return PartialView("_CapNhatAnhBacSiModal", viewModel);
        }
        
        public async Task<FileResult> GetProfilePictureById(string id = "")
        {
            if (id.IsNullOrEmpty())
            {
                return GetDefaultProfilePictureInternal();
            }

            return await GetProfilePictureById(Guid.Parse(id));
        }
        private async Task<FileResult> GetProfilePictureById(Guid profilePictureId)
        {
            var file = await _binaryObjectManager.GetOrNullAsync(profilePictureId);
            if (file == null)
            {
                return GetDefaultProfilePictureInternal();
            }

            return File(file.Bytes, MimeTypeNames.ImageJpeg);
        }
        protected FileResult GetDefaultProfilePictureInternal()
        {
            return File(
                @"Common\Images\default-profile-picture.png",
                MimeTypeNames.ImagePng
            );
        }
    }
}