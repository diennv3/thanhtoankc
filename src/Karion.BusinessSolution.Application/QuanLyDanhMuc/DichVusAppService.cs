using Karion.BusinessSolution.QuanLyDanhMuc;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Karion.BusinessSolution.QuanLyDanhMuc.Exporting;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.Dto;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Karion.BusinessSolution.QuanLyDanhMuc
{
    [AbpAuthorize(AppPermissions.Pages_DichVus)]
    public class DichVusAppService : BusinessSolutionAppServiceBase, IDichVusAppService
    {
        private readonly IRepository<DichVu> _dichVuRepository;
        private readonly IDichVusExcelExporter _dichVusExcelExporter;
        private readonly IRepository<ChuyenKhoa, int> _lookup_chuyenKhoaRepository;
        private readonly IRepository<GiaDichVu, int> _lookup_giaDichVuRepository;


        public DichVusAppService(IRepository<DichVu> dichVuRepository,
                                 IDichVusExcelExporter dichVusExcelExporter,
                                 IRepository<ChuyenKhoa, int> lookup_chuyenKhoaRepository,
                                 IRepository<GiaDichVu, int> lookup_giaDichVuRepository
        )
        {
            _dichVuRepository = dichVuRepository;
            _dichVusExcelExporter = dichVusExcelExporter;
            _lookup_chuyenKhoaRepository = lookup_chuyenKhoaRepository;
            _lookup_giaDichVuRepository = lookup_giaDichVuRepository;
        }

        public async Task<PagedResultDto<GetDichVuForViewDto>> GetAll(GetAllDichVusInput input)
        {
            var filteredDichVus = _dichVuRepository.GetAll()
                .Include(e => e.ChuyenKhoaFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.Ten.Contains(input.Filter) || e.MoTa.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.TenFilter), e => e.Ten == input.TenFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.MoTaFilter), e => e.MoTa == input.MoTaFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.ChuyenKhoaTenFilter),
                    e => e.ChuyenKhoaFk != null && e.ChuyenKhoaFk.Ten == input.ChuyenKhoaTenFilter);

            var pagedAndFilteredDichVus = filteredDichVus
                .OrderBy(input.Sorting ?? "chuyenKhoaId asc").ThenBy("ten asc")
                .PageBy(input);

            var dichVus = from o in pagedAndFilteredDichVus
                join o1 in _lookup_chuyenKhoaRepository.GetAll() on o.ChuyenKhoaId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()
                join o2 in _lookup_giaDichVuRepository.GetAll() on o.Id equals o2.DichVuId into j2
                from s2 in j2.DefaultIfEmpty() 
                select new GetDichVuForViewDto()
                {
                    DichVu = new DichVuDto
                    {
                        Ten = o.Ten,
                        MoTa = o.MoTa,
                        Id = o.Id
                    },
                    ChuyenKhoaTen = s1 == null || s1.Ten == null ? "" : s1.Ten.ToString(),
                    GiaDichVu = new GiaDichVuDto
                    {
                        MucGia  = s2.MucGia,
                        MoTa = s2.MoTa,
                        Gia = s2.Gia,
                        NgayApDung = s2.NgayApDung,
                        Id = s2.Id
                    }
                };

            var totalCount = await filteredDichVus.CountAsync();

            return new PagedResultDto<GetDichVuForViewDto>(
                totalCount,
                await dichVus.ToListAsync()
            );
        }

        public async Task<GetDichVuForViewDto> GetDichVuForView(int id)
        {
            var dichVu = await _dichVuRepository.GetAsync(id);

            var output = new GetDichVuForViewDto {DichVu = ObjectMapper.Map<DichVuDto>(dichVu)};

            if (output.DichVu.ChuyenKhoaId != null)
            {
                var _lookupChuyenKhoa =
                    await _lookup_chuyenKhoaRepository.FirstOrDefaultAsync((int) output.DichVu.ChuyenKhoaId);
                output.ChuyenKhoaTen = _lookupChuyenKhoa?.Ten?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_DichVus_Edit)]
        public async Task<GetDichVuForEditOutput> GetDichVuForEdit(EntityDto input)
        {
            var dichVu = await _dichVuRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDichVuForEditOutput {DichVu = ObjectMapper.Map<CreateOrEditDichVuDto>(dichVu)};

            if (output.DichVu.ChuyenKhoaId != null)
            {
                var _lookupChuyenKhoa =
                    await _lookup_chuyenKhoaRepository.FirstOrDefaultAsync((int) output.DichVu.ChuyenKhoaId);
                output.ChuyenKhoaTen = _lookupChuyenKhoa?.Ten?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDichVuDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_DichVus_Create)]
        protected virtual async Task Create(CreateOrEditDichVuDto input)
        {
            var dichVu = ObjectMapper.Map<DichVu>(input);


            if (AbpSession.TenantId != null)
            {
                dichVu.TenantId = (int?) AbpSession.TenantId;
            }


            await _dichVuRepository.InsertAsync(dichVu);
        }

        [AbpAuthorize(AppPermissions.Pages_DichVus_Edit)]
        protected virtual async Task Update(CreateOrEditDichVuDto input)
        {
            var dichVu = await _dichVuRepository.FirstOrDefaultAsync((int) input.Id);
            ObjectMapper.Map(input, dichVu);
        }

        [AbpAuthorize(AppPermissions.Pages_DichVus_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _dichVuRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetDichVusToExcel(GetAllDichVusForExcelInput input)
        {
            var filteredDichVus = _dichVuRepository.GetAll()
                .Include(e => e.ChuyenKhoaFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.Ten.Contains(input.Filter) || e.MoTa.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.TenFilter), e => e.Ten == input.TenFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.MoTaFilter), e => e.MoTa == input.MoTaFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.ChuyenKhoaTenFilter),
                    e => e.ChuyenKhoaFk != null && e.ChuyenKhoaFk.Ten == input.ChuyenKhoaTenFilter);

            var query = (from o in filteredDichVus
                join o1 in _lookup_chuyenKhoaRepository.GetAll() on o.ChuyenKhoaId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()
                select new GetDichVuForViewDto()
                {
                    DichVu = new DichVuDto
                    {
                        Ten = o.Ten,
                        MoTa = o.MoTa,
                        Id = o.Id
                    },
                    ChuyenKhoaTen = s1 == null || s1.Ten == null ? "" : s1.Ten.ToString()
                });


            var dichVuListDtos = await query.ToListAsync();

            return _dichVusExcelExporter.ExportToFile(dichVuListDtos);
        }


        [AbpAuthorize(AppPermissions.Pages_DichVus)]
        public async Task<PagedResultDto<DichVuChuyenKhoaLookupTableDto>> GetAllChuyenKhoaForLookupTable(
            GetAllForLookupTableInput input)
        {
            var query = _lookup_chuyenKhoaRepository.GetAll().WhereIf(
                !string.IsNullOrWhiteSpace(input.Filter),
                e => e.Ten != null && e.Ten.Contains(input.Filter)
            );

            var totalCount = await query.CountAsync();

            var chuyenKhoaList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<DichVuChuyenKhoaLookupTableDto>();
            foreach (var chuyenKhoa in chuyenKhoaList)
            {
                lookupTableDtoList.Add(new DichVuChuyenKhoaLookupTableDto
                {
                    Id = chuyenKhoa.Id,
                    DisplayName = chuyenKhoa.Ten?.ToString()
                });
            }

            return new PagedResultDto<DichVuChuyenKhoaLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}