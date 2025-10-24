using Karion.BusinessSolution.Authorization.Users;


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
	[AbpAuthorize(AppPermissions.Pages_ThongTinBacSiMoRongs)]
    public class ThongTinBacSiMoRongsAppService : BusinessSolutionAppServiceBase, IThongTinBacSiMoRongsAppService
    {
		 private readonly IRepository<ThongTinBacSiMoRong> _thongTinBacSiMoRongRepository;
		 private readonly IThongTinBacSiMoRongsExcelExporter _thongTinBacSiMoRongsExcelExporter;
		 private readonly IRepository<User,long> _lookup_userRepository;
		 

		  public ThongTinBacSiMoRongsAppService(IRepository<ThongTinBacSiMoRong> thongTinBacSiMoRongRepository, IThongTinBacSiMoRongsExcelExporter thongTinBacSiMoRongsExcelExporter , IRepository<User, long> lookup_userRepository) 
		  {
			_thongTinBacSiMoRongRepository = thongTinBacSiMoRongRepository;
			_thongTinBacSiMoRongsExcelExporter = thongTinBacSiMoRongsExcelExporter;
			_lookup_userRepository = lookup_userRepository;
		
		  }

		 public async Task<PagedResultDto<GetThongTinBacSiMoRongForViewDto>> GetAll(GetAllThongTinBacSiMoRongsInput input)
         {
			
			var filteredThongTinBacSiMoRongs = _thongTinBacSiMoRongRepository.GetAll()
						.Include( e => e.UserFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Image.Contains(input.Filter) || e.TieuSu.Contains(input.Filter) || e.ChucDanh.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.ImageFilter),  e => e.Image == input.ImageFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TieuSuFilter),  e => e.TieuSu == input.TieuSuFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ChucDanhFilter),  e => e.ChucDanh == input.ChucDanhFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

			var pagedAndFilteredThongTinBacSiMoRongs = filteredThongTinBacSiMoRongs
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var thongTinBacSiMoRongs = from o in pagedAndFilteredThongTinBacSiMoRongs
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetThongTinBacSiMoRongForViewDto() {
							ThongTinBacSiMoRong = new ThongTinBacSiMoRongDto
							{
                                Image = o.Image,
                                TieuSu = o.TieuSu,
                                ChucDanh = o.ChucDanh,
                                Id = o.Id
							},
							UserName = s1 == null || s1.Name == null ? "" : (s1.Surname + " " +s1.Name).ToString()
                            
						};

            var totalCount = await filteredThongTinBacSiMoRongs.CountAsync();

            return new PagedResultDto<GetThongTinBacSiMoRongForViewDto>(
                totalCount,
                await thongTinBacSiMoRongs.ToListAsync()
            );
         }
		 
		 public async Task<GetThongTinBacSiMoRongForViewDto> GetThongTinBacSiMoRongForView(int id)
         {
            var thongTinBacSiMoRong = await _thongTinBacSiMoRongRepository.GetAsync(id);

            var output = new GetThongTinBacSiMoRongForViewDto { ThongTinBacSiMoRong = ObjectMapper.Map<ThongTinBacSiMoRongDto>(thongTinBacSiMoRong) };

		    if (output.ThongTinBacSiMoRong.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.ThongTinBacSiMoRong.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_ThongTinBacSiMoRongs_Edit)]
		 public async Task<GetThongTinBacSiMoRongForEditOutput> GetThongTinBacSiMoRongForEdit(EntityDto input)
         {
            var thongTinBacSiMoRong = await _thongTinBacSiMoRongRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetThongTinBacSiMoRongForEditOutput {ThongTinBacSiMoRong = ObjectMapper.Map<CreateOrEditThongTinBacSiMoRongDto>(thongTinBacSiMoRong)};

		    if (output.ThongTinBacSiMoRong.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.ThongTinBacSiMoRong.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditThongTinBacSiMoRongDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_ThongTinBacSiMoRongs_Create)]
		 protected virtual async Task Create(CreateOrEditThongTinBacSiMoRongDto input)
         {
            var thongTinBacSiMoRong = ObjectMapper.Map<ThongTinBacSiMoRong>(input);

			
			if (AbpSession.TenantId != null)
			{
				thongTinBacSiMoRong.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _thongTinBacSiMoRongRepository.InsertAsync(thongTinBacSiMoRong);
         }

		 [AbpAuthorize(AppPermissions.Pages_ThongTinBacSiMoRongs_Edit)]
		 protected virtual async Task Update(CreateOrEditThongTinBacSiMoRongDto input)
         {
            var thongTinBacSiMoRong = await _thongTinBacSiMoRongRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, thongTinBacSiMoRong);
         }

		 [AbpAuthorize(AppPermissions.Pages_ThongTinBacSiMoRongs_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _thongTinBacSiMoRongRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetThongTinBacSiMoRongsToExcel(GetAllThongTinBacSiMoRongsForExcelInput input)
         {
			
			var filteredThongTinBacSiMoRongs = _thongTinBacSiMoRongRepository.GetAll()
						.Include( e => e.UserFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Image.Contains(input.Filter) || e.TieuSu.Contains(input.Filter) || e.ChucDanh.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.ImageFilter),  e => e.Image == input.ImageFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TieuSuFilter),  e => e.TieuSu == input.TieuSuFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ChucDanhFilter),  e => e.ChucDanh == input.ChucDanhFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

			var query = (from o in filteredThongTinBacSiMoRongs
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetThongTinBacSiMoRongForViewDto() { 
							ThongTinBacSiMoRong = new ThongTinBacSiMoRongDto
							{
                                Image = o.Image,
                                TieuSu = o.TieuSu,
                                ChucDanh = o.ChucDanh,
                                Id = o.Id
							},
                         	UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
						 });


            var thongTinBacSiMoRongListDtos = await query.ToListAsync();

            return _thongTinBacSiMoRongsExcelExporter.ExportToFile(thongTinBacSiMoRongListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_ThongTinBacSiMoRongs)]
         public async Task<PagedResultDto<ThongTinBacSiMoRongUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ThongTinBacSiMoRongUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new ThongTinBacSiMoRongUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<ThongTinBacSiMoRongUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}