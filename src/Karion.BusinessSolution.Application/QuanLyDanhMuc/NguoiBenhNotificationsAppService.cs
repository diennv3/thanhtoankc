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
	[AbpAuthorize(AppPermissions.Pages_NguoiBenhNotifications)]
    public class NguoiBenhNotificationsAppService : BusinessSolutionAppServiceBase, INguoiBenhNotificationsAppService
    {
		 private readonly IRepository<NguoiBenhNotification, long> _nguoiBenhNotificationRepository;
		 private readonly INguoiBenhNotificationsExcelExporter _nguoiBenhNotificationsExcelExporter;
		 private readonly IRepository<NguoiBenh,int> _lookup_nguoiBenhRepository;
		 

		  public NguoiBenhNotificationsAppService(IRepository<NguoiBenhNotification, long> nguoiBenhNotificationRepository, INguoiBenhNotificationsExcelExporter nguoiBenhNotificationsExcelExporter , IRepository<NguoiBenh, int> lookup_nguoiBenhRepository) 
		  {
			_nguoiBenhNotificationRepository = nguoiBenhNotificationRepository;
			_nguoiBenhNotificationsExcelExporter = nguoiBenhNotificationsExcelExporter;
			_lookup_nguoiBenhRepository = lookup_nguoiBenhRepository;
		
		  }

		 public async Task<PagedResultDto<GetNguoiBenhNotificationForViewDto>> GetAll(GetAllNguoiBenhNotificationsInput input)
         {
			
			var filteredNguoiBenhNotifications = _nguoiBenhNotificationRepository.GetAll()
						.Include( e => e.NguoiBenhFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.NoiDungTinNhan.Contains(input.Filter) || e.TieuDe.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NoiDungTinNhanFilter),  e => e.NoiDungTinNhan == input.NoiDungTinNhanFilter)
						.WhereIf(input.MinTrangThaiFilter != null, e => e.TrangThai >= input.MinTrangThaiFilter)
						.WhereIf(input.MaxTrangThaiFilter != null, e => e.TrangThai <= input.MaxTrangThaiFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TieuDeFilter),  e => e.TieuDe == input.TieuDeFilter)
						.WhereIf(input.MinThoiGianGuiFilter != null, e => e.ThoiGianGui >= input.MinThoiGianGuiFilter)
						.WhereIf(input.MaxThoiGianGuiFilter != null, e => e.ThoiGianGui <= input.MaxThoiGianGuiFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NguoiBenhUserNameFilter), e => e.NguoiBenhFk != null && e.NguoiBenhFk.UserName == input.NguoiBenhUserNameFilter);

			var pagedAndFilteredNguoiBenhNotifications = filteredNguoiBenhNotifications
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var nguoiBenhNotifications = from o in pagedAndFilteredNguoiBenhNotifications
                         join o1 in _lookup_nguoiBenhRepository.GetAll() on o.NguoiBenhId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetNguoiBenhNotificationForViewDto() {
							NguoiBenhNotification = new NguoiBenhNotificationDto
							{
                                NoiDungTinNhan = o.NoiDungTinNhan,
                                TrangThai = o.TrangThai,
                                TieuDe = o.TieuDe,
                                ThoiGianGui = o.ThoiGianGui,
                                Id = o.Id
							},
                         	NguoiBenhUserName = s1 == null || s1.UserName == null ? "" : s1.UserName.ToString()
						};

            var totalCount = await filteredNguoiBenhNotifications.CountAsync();

            return new PagedResultDto<GetNguoiBenhNotificationForViewDto>(
                totalCount,
                await nguoiBenhNotifications.ToListAsync()
            );
         }
		 
		 public async Task<GetNguoiBenhNotificationForViewDto> GetNguoiBenhNotificationForView(long id)
         {
            var nguoiBenhNotification = await _nguoiBenhNotificationRepository.GetAsync(id);

            var output = new GetNguoiBenhNotificationForViewDto { NguoiBenhNotification = ObjectMapper.Map<NguoiBenhNotificationDto>(nguoiBenhNotification) };

		    if (output.NguoiBenhNotification.NguoiBenhId != null)
            {
                var _lookupNguoiBenh = await _lookup_nguoiBenhRepository.FirstOrDefaultAsync((int)output.NguoiBenhNotification.NguoiBenhId);
                output.NguoiBenhUserName = _lookupNguoiBenh?.UserName?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_NguoiBenhNotifications_Edit)]
		 public async Task<GetNguoiBenhNotificationForEditOutput> GetNguoiBenhNotificationForEdit(EntityDto<long> input)
         {
            var nguoiBenhNotification = await _nguoiBenhNotificationRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetNguoiBenhNotificationForEditOutput {NguoiBenhNotification = ObjectMapper.Map<CreateOrEditNguoiBenhNotificationDto>(nguoiBenhNotification)};

		    if (output.NguoiBenhNotification.NguoiBenhId != null)
            {
                var _lookupNguoiBenh = await _lookup_nguoiBenhRepository.FirstOrDefaultAsync((int)output.NguoiBenhNotification.NguoiBenhId);
                output.NguoiBenhUserName = _lookupNguoiBenh?.UserName?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditNguoiBenhNotificationDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_NguoiBenhNotifications_Create)]
		 protected virtual async Task Create(CreateOrEditNguoiBenhNotificationDto input)
         {
            var nguoiBenhNotification = ObjectMapper.Map<NguoiBenhNotification>(input);

			

            await _nguoiBenhNotificationRepository.InsertAsync(nguoiBenhNotification);
         }

		 [AbpAuthorize(AppPermissions.Pages_NguoiBenhNotifications_Edit)]
		 protected virtual async Task Update(CreateOrEditNguoiBenhNotificationDto input)
         {
            var nguoiBenhNotification = await _nguoiBenhNotificationRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, nguoiBenhNotification);
         }

		 [AbpAuthorize(AppPermissions.Pages_NguoiBenhNotifications_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _nguoiBenhNotificationRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetNguoiBenhNotificationsToExcel(GetAllNguoiBenhNotificationsForExcelInput input)
         {
			
			var filteredNguoiBenhNotifications = _nguoiBenhNotificationRepository.GetAll()
						.Include( e => e.NguoiBenhFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.NoiDungTinNhan.Contains(input.Filter) || e.TieuDe.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NoiDungTinNhanFilter),  e => e.NoiDungTinNhan == input.NoiDungTinNhanFilter)
						.WhereIf(input.MinTrangThaiFilter != null, e => e.TrangThai >= input.MinTrangThaiFilter)
						.WhereIf(input.MaxTrangThaiFilter != null, e => e.TrangThai <= input.MaxTrangThaiFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TieuDeFilter),  e => e.TieuDe == input.TieuDeFilter)
						.WhereIf(input.MinThoiGianGuiFilter != null, e => e.ThoiGianGui >= input.MinThoiGianGuiFilter)
						.WhereIf(input.MaxThoiGianGuiFilter != null, e => e.ThoiGianGui <= input.MaxThoiGianGuiFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NguoiBenhUserNameFilter), e => e.NguoiBenhFk != null && e.NguoiBenhFk.UserName == input.NguoiBenhUserNameFilter);

			var query = (from o in filteredNguoiBenhNotifications
                         join o1 in _lookup_nguoiBenhRepository.GetAll() on o.NguoiBenhId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetNguoiBenhNotificationForViewDto() { 
							NguoiBenhNotification = new NguoiBenhNotificationDto
							{
                                NoiDungTinNhan = o.NoiDungTinNhan,
                                TrangThai = o.TrangThai,
                                TieuDe = o.TieuDe,
                                ThoiGianGui = o.ThoiGianGui,
                                Id = o.Id
							},
                         	NguoiBenhUserName = s1 == null || s1.UserName == null ? "" : s1.UserName.ToString()
						 });


            var nguoiBenhNotificationListDtos = await query.ToListAsync();

            return _nguoiBenhNotificationsExcelExporter.ExportToFile(nguoiBenhNotificationListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_NguoiBenhNotifications)]
         public async Task<PagedResultDto<NguoiBenhNotificationNguoiBenhLookupTableDto>> GetAllNguoiBenhForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_nguoiBenhRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.UserName != null && e.UserName.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var nguoiBenhList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<NguoiBenhNotificationNguoiBenhLookupTableDto>();
			foreach(var nguoiBenh in nguoiBenhList){
				lookupTableDtoList.Add(new NguoiBenhNotificationNguoiBenhLookupTableDto
				{
					Id = nguoiBenh.Id,
					DisplayName = nguoiBenh.UserName?.ToString()
				});
			}

            return new PagedResultDto<NguoiBenhNotificationNguoiBenhLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}