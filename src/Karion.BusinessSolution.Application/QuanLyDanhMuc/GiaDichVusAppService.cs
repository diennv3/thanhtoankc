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
	[AbpAuthorize(AppPermissions.Pages_GiaDichVus)]
    public class GiaDichVusAppService : BusinessSolutionAppServiceBase, IGiaDichVusAppService
    {
		 private readonly IRepository<GiaDichVu> _giaDichVuRepository;
		 private readonly IGiaDichVusExcelExporter _giaDichVusExcelExporter;
		 private readonly IRepository<DichVu,int> _lookup_dichVuRepository;
		 

		  public GiaDichVusAppService(IRepository<GiaDichVu> giaDichVuRepository, IGiaDichVusExcelExporter giaDichVusExcelExporter , IRepository<DichVu, int> lookup_dichVuRepository) 
		  {
			_giaDichVuRepository = giaDichVuRepository;
			_giaDichVusExcelExporter = giaDichVusExcelExporter;
			_lookup_dichVuRepository = lookup_dichVuRepository;
		
		  }

		 public async Task<PagedResultDto<GetGiaDichVuForViewDto>> GetAll(GetAllGiaDichVusInput input)
         {
			
			var filteredGiaDichVus = _giaDichVuRepository.GetAll()
						.Include( e => e.DichVuFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.MucGia.Contains(input.Filter) || e.MoTa.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.MucGiaFilter),  e => e.MucGia == input.MucGiaFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.MoTaFilter),  e => e.MoTa == input.MoTaFilter)
						.WhereIf(input.MinGiaFilter != null, e => e.Gia >= input.MinGiaFilter)
						.WhereIf(input.MaxGiaFilter != null, e => e.Gia <= input.MaxGiaFilter)
						.WhereIf(input.MinNgayApDungFilter != null, e => e.NgayApDung >= input.MinNgayApDungFilter)
						.WhereIf(input.MaxNgayApDungFilter != null, e => e.NgayApDung <= input.MaxNgayApDungFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DichVuTenFilter), e => e.DichVuFk != null && e.DichVuFk.Ten == input.DichVuTenFilter);

			var pagedAndFilteredGiaDichVus = filteredGiaDichVus
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var giaDichVus = from o in pagedAndFilteredGiaDichVus
                         join o1 in _lookup_dichVuRepository.GetAll() on o.DichVuId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetGiaDichVuForViewDto() {
							GiaDichVu = new GiaDichVuDto
							{
                                MucGia = o.MucGia,
                                MoTa = o.MoTa,
                                Gia = o.Gia,
                                NgayApDung = o.NgayApDung,
                                Id = o.Id
							},
                         	DichVuTen = s1 == null || s1.Ten == null ? "" : s1.Ten.ToString()
						};

            var totalCount = await filteredGiaDichVus.CountAsync();

            return new PagedResultDto<GetGiaDichVuForViewDto>(
                totalCount,
                await giaDichVus.ToListAsync()
            );
         }
		 
		 public async Task<GetGiaDichVuForViewDto> GetGiaDichVuForView(int id)
         {
            var giaDichVu = await _giaDichVuRepository.GetAsync(id);

            var output = new GetGiaDichVuForViewDto { GiaDichVu = ObjectMapper.Map<GiaDichVuDto>(giaDichVu) };

		    if (output.GiaDichVu.DichVuId != null)
            {
                var _lookupDichVu = await _lookup_dichVuRepository.FirstOrDefaultAsync((int)output.GiaDichVu.DichVuId);
                output.DichVuTen = _lookupDichVu?.Ten?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_GiaDichVus_Edit)]
		 public async Task<GetGiaDichVuForEditOutput> GetGiaDichVuForEdit(EntityDto input)
         {
            var giaDichVu = await _giaDichVuRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetGiaDichVuForEditOutput {GiaDichVu = ObjectMapper.Map<CreateOrEditGiaDichVuDto>(giaDichVu)};

		    if (output.GiaDichVu.DichVuId != null)
            {
                var _lookupDichVu = await _lookup_dichVuRepository.FirstOrDefaultAsync((int)output.GiaDichVu.DichVuId);
                output.DichVuTen = _lookupDichVu?.Ten?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditGiaDichVuDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_GiaDichVus_Create)]
		 protected virtual async Task Create(CreateOrEditGiaDichVuDto input)
         {
            var giaDichVu = ObjectMapper.Map<GiaDichVu>(input);

			
			if (AbpSession.TenantId != null)
			{
				giaDichVu.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _giaDichVuRepository.InsertAsync(giaDichVu);
         }

		 [AbpAuthorize(AppPermissions.Pages_GiaDichVus_Edit)]
		 protected virtual async Task Update(CreateOrEditGiaDichVuDto input)
         {
            var giaDichVu = await _giaDichVuRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, giaDichVu);
         }

		 [AbpAuthorize(AppPermissions.Pages_GiaDichVus_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _giaDichVuRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetGiaDichVusToExcel(GetAllGiaDichVusForExcelInput input)
         {
			
			var filteredGiaDichVus = _giaDichVuRepository.GetAll()
						.Include( e => e.DichVuFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.MucGia.Contains(input.Filter) || e.MoTa.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.MucGiaFilter),  e => e.MucGia == input.MucGiaFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.MoTaFilter),  e => e.MoTa == input.MoTaFilter)
						.WhereIf(input.MinGiaFilter != null, e => e.Gia >= input.MinGiaFilter)
						.WhereIf(input.MaxGiaFilter != null, e => e.Gia <= input.MaxGiaFilter)
						.WhereIf(input.MinNgayApDungFilter != null, e => e.NgayApDung >= input.MinNgayApDungFilter)
						.WhereIf(input.MaxNgayApDungFilter != null, e => e.NgayApDung <= input.MaxNgayApDungFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DichVuTenFilter), e => e.DichVuFk != null && e.DichVuFk.Ten == input.DichVuTenFilter);

			var query = (from o in filteredGiaDichVus
                         join o1 in _lookup_dichVuRepository.GetAll() on o.DichVuId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetGiaDichVuForViewDto() { 
							GiaDichVu = new GiaDichVuDto
							{
                                MucGia = o.MucGia,
                                MoTa = o.MoTa,
                                Gia = o.Gia,
                                NgayApDung = o.NgayApDung,
                                Id = o.Id
							},
                         	DichVuTen = s1 == null || s1.Ten == null ? "" : s1.Ten.ToString()
						 });


            var giaDichVuListDtos = await query.ToListAsync();

            return _giaDichVusExcelExporter.ExportToFile(giaDichVuListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_GiaDichVus)]
         public async Task<PagedResultDto<GiaDichVuDichVuLookupTableDto>> GetAllDichVuForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_dichVuRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Ten != null && e.Ten.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var dichVuList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<GiaDichVuDichVuLookupTableDto>();
			foreach(var dichVu in dichVuList){
				lookupTableDtoList.Add(new GiaDichVuDichVuLookupTableDto
				{
					Id = dichVu.Id,
					DisplayName = dichVu.Ten?.ToString()
				});
			}

            return new PagedResultDto<GiaDichVuDichVuLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}