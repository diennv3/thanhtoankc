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
	[AbpAuthorize(AppPermissions.Pages_NguoiThans)]
    public class NguoiThansAppService : BusinessSolutionAppServiceBase, INguoiThansAppService
    {
		 private readonly IRepository<NguoiThan> _nguoiThanRepository;
		 private readonly INguoiThansExcelExporter _nguoiThansExcelExporter;
		 private readonly IRepository<NguoiBenh,int> _lookup_nguoiBenhRepository;
		 

		  public NguoiThansAppService(IRepository<NguoiThan> nguoiThanRepository, INguoiThansExcelExporter nguoiThansExcelExporter , IRepository<NguoiBenh, int> lookup_nguoiBenhRepository) 
		  {
			_nguoiThanRepository = nguoiThanRepository;
			_nguoiThansExcelExporter = nguoiThansExcelExporter;
			_lookup_nguoiBenhRepository = lookup_nguoiBenhRepository;
		
		  }

		 public async Task<PagedResultDto<GetNguoiThanForViewDto>> GetAll(GetAllNguoiThansInput input)
         {
			
			var filteredNguoiThans = _nguoiThanRepository.GetAll()
						.Include( e => e.NguoiBenhFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.HoVaTen.Contains(input.Filter) || e.GioiTinh.Contains(input.Filter) || e.DiaChi.Contains(input.Filter) || e.MoiQuanHe.Contains(input.Filter) || e.SoDienThoai.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.HoVaTenFilter),  e => e.HoVaTen == input.HoVaTenFilter)
						.WhereIf(input.MinTuoiFilter != null, e => e.Tuoi >= input.MinTuoiFilter)
						.WhereIf(input.MaxTuoiFilter != null, e => e.Tuoi <= input.MaxTuoiFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.GioiTinhFilter),  e => e.GioiTinh == input.GioiTinhFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DiaChiFilter),  e => e.DiaChi == input.DiaChiFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.MoiQuanHeFilter),  e => e.MoiQuanHe == input.MoiQuanHeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.SoDienThoaiFilter),  e => e.SoDienThoai == input.SoDienThoaiFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NguoiBenhHoVaTenFilter), e => e.NguoiBenhFk != null && e.NguoiBenhFk.HoVaTen == input.NguoiBenhHoVaTenFilter)
						.WhereIf(input.NguoiBenhId.HasValue, e => e.NguoiBenhId.Equals(input.NguoiBenhId));

			var pagedAndFilteredNguoiThans = filteredNguoiThans
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var nguoiThans = from o in pagedAndFilteredNguoiThans
                         join o1 in _lookup_nguoiBenhRepository.GetAll() on o.NguoiBenhId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetNguoiThanForViewDto() {
							NguoiThan = new NguoiThanDto
							{
                                HoVaTen = o.HoVaTen,
                                Tuoi = o.Tuoi,
                                GioiTinh = o.GioiTinh,
                                DiaChi = o.DiaChi,
                                MoiQuanHe = o.MoiQuanHe,
                                SoDienThoai = o.SoDienThoai,
                                Id = o.Id
							},
                         	NguoiBenhHoVaTen = s1 == null || s1.HoVaTen == null ? "" : s1.HoVaTen.ToString()
						};

            var totalCount = await filteredNguoiThans.CountAsync();

            return new PagedResultDto<GetNguoiThanForViewDto>(
                totalCount,
                await nguoiThans.ToListAsync()
            );
         }
		 
		 public async Task<GetNguoiThanForViewDto> GetNguoiThanForView(int id)
         {
            var nguoiThan = await _nguoiThanRepository.GetAsync(id);

            var output = new GetNguoiThanForViewDto { NguoiThan = ObjectMapper.Map<NguoiThanDto>(nguoiThan) };

		    if (output.NguoiThan.NguoiBenhId != null)
            {
                var _lookupNguoiBenh = await _lookup_nguoiBenhRepository.FirstOrDefaultAsync((int)output.NguoiThan.NguoiBenhId);
                output.NguoiBenhHoVaTen = _lookupNguoiBenh?.HoVaTen?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_NguoiThans_Edit)]
		 public async Task<GetNguoiThanForEditOutput> GetNguoiThanForEdit(EntityDto input)
         {
            var nguoiThan = await _nguoiThanRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetNguoiThanForEditOutput {NguoiThan = ObjectMapper.Map<CreateOrEditNguoiThanDto>(nguoiThan)};

		    if (output.NguoiThan.NguoiBenhId != null)
            {
                var _lookupNguoiBenh = await _lookup_nguoiBenhRepository.FirstOrDefaultAsync((int)output.NguoiThan.NguoiBenhId);
                output.NguoiBenhHoVaTen = _lookupNguoiBenh?.HoVaTen?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditNguoiThanDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_NguoiThans_Create)]
		 protected virtual async Task Create(CreateOrEditNguoiThanDto input)
         {
            var nguoiThan = ObjectMapper.Map<NguoiThan>(input);

			

            await _nguoiThanRepository.InsertAsync(nguoiThan);
         }

		 [AbpAuthorize(AppPermissions.Pages_NguoiThans_Edit)]
		 protected virtual async Task Update(CreateOrEditNguoiThanDto input)
         {
            var nguoiThan = await _nguoiThanRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, nguoiThan);
         }

		 [AbpAuthorize(AppPermissions.Pages_NguoiThans_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _nguoiThanRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetNguoiThansToExcel(GetAllNguoiThansForExcelInput input)
         {
			
			var filteredNguoiThans = _nguoiThanRepository.GetAll()
						.Include( e => e.NguoiBenhFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.HoVaTen.Contains(input.Filter) || e.GioiTinh.Contains(input.Filter) || e.DiaChi.Contains(input.Filter) || e.MoiQuanHe.Contains(input.Filter) || e.SoDienThoai.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.HoVaTenFilter),  e => e.HoVaTen == input.HoVaTenFilter)
						.WhereIf(input.MinTuoiFilter != null, e => e.Tuoi >= input.MinTuoiFilter)
						.WhereIf(input.MaxTuoiFilter != null, e => e.Tuoi <= input.MaxTuoiFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.GioiTinhFilter),  e => e.GioiTinh == input.GioiTinhFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DiaChiFilter),  e => e.DiaChi == input.DiaChiFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.MoiQuanHeFilter),  e => e.MoiQuanHe == input.MoiQuanHeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.SoDienThoaiFilter),  e => e.SoDienThoai == input.SoDienThoaiFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NguoiBenhHoVaTenFilter), e => e.NguoiBenhFk != null && e.NguoiBenhFk.HoVaTen == input.NguoiBenhHoVaTenFilter);

			var query = (from o in filteredNguoiThans
                         join o1 in _lookup_nguoiBenhRepository.GetAll() on o.NguoiBenhId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetNguoiThanForViewDto() { 
							NguoiThan = new NguoiThanDto
							{
                                HoVaTen = o.HoVaTen,
                                Tuoi = o.Tuoi,
                                GioiTinh = o.GioiTinh,
                                DiaChi = o.DiaChi,
                                MoiQuanHe = o.MoiQuanHe,
                                SoDienThoai = o.SoDienThoai,
                                Id = o.Id
							},
                         	NguoiBenhHoVaTen = s1 == null || s1.HoVaTen == null ? "" : s1.HoVaTen.ToString()
						 });


            var nguoiThanListDtos = await query.ToListAsync();

            return _nguoiThansExcelExporter.ExportToFile(nguoiThanListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_NguoiThans)]
         public async Task<PagedResultDto<NguoiThanNguoiBenhLookupTableDto>> GetAllNguoiBenhForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_nguoiBenhRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.HoVaTen != null && e.HoVaTen.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var nguoiBenhList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<NguoiThanNguoiBenhLookupTableDto>();
			foreach(var nguoiBenh in nguoiBenhList){
				lookupTableDtoList.Add(new NguoiThanNguoiBenhLookupTableDto
				{
					Id = nguoiBenh.Id,
					DisplayName = nguoiBenh.HoVaTen?.ToString()
				});
			}

            return new PagedResultDto<NguoiThanNguoiBenhLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}