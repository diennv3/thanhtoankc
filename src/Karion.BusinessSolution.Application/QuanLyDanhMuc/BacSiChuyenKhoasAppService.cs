using Karion.BusinessSolution.Authorization.Users;
using Karion.BusinessSolution.QuanLyDanhMuc;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
using System.Text;
using Abp.Runtime.Session;
using Abp.UI;
using Karion.BusinessSolution.Authorization.Users.Profile.Dto;
using Karion.BusinessSolution.Extension.Karion.BusinessSolution.Common;
using Karion.BusinessSolution.Storage;
using NPOI.OpenXmlFormats.Wordprocessing;

namespace Karion.BusinessSolution.QuanLyDanhMuc
{
	[AbpAuthorize(AppPermissions.Pages_BacSiChuyenKhoas)]
    public class BacSiChuyenKhoasAppService : BusinessSolutionAppServiceBase, IBacSiChuyenKhoasAppService
    {
	     private const int MaxProfilPictureBytes = 5242880; //5MB
	     private readonly IBinaryObjectManager _binaryObjectManager;
		 private readonly IRepository<BacSiChuyenKhoa> _bacSiChuyenKhoaRepository;
		 private readonly IBacSiChuyenKhoasExcelExporter _bacSiChuyenKhoasExcelExporter;
		 private readonly IRepository<User,long> _lookup_userRepository;
		 private readonly IRepository<ChuyenKhoa,int> _lookup_chuyenKhoaRepository;
		 private readonly IRepository<ThongTinBacSiMoRong> _thongTinBacSiMoRongRepository;
		 private readonly ITempFileCacheManager _tempFileCacheManager;

		  public BacSiChuyenKhoasAppService(IRepository<BacSiChuyenKhoa> bacSiChuyenKhoaRepository, 
			                                IBacSiChuyenKhoasExcelExporter bacSiChuyenKhoasExcelExporter,
			                                IRepository<User, long> lookup_userRepository,
			                                IRepository<ChuyenKhoa, int> lookup_chuyenKhoaRepository,
			                                IRepository<ThongTinBacSiMoRong> thongTinBacSiMoRongRepository,
			                                IBinaryObjectManager binaryObjectManager,
											ITempFileCacheManager tempFileCacheManager
			                                ) 
		  {
			  _bacSiChuyenKhoaRepository = bacSiChuyenKhoaRepository;
			  _bacSiChuyenKhoasExcelExporter = bacSiChuyenKhoasExcelExporter;
			  _lookup_userRepository = lookup_userRepository;
			  _lookup_chuyenKhoaRepository = lookup_chuyenKhoaRepository;
			  _thongTinBacSiMoRongRepository = thongTinBacSiMoRongRepository;
			  _tempFileCacheManager = tempFileCacheManager;
			  _binaryObjectManager = binaryObjectManager;
		  }

		 public async Task<PagedResultDto<GetBacSiChuyenKhoaForViewDto>> GetAll(GetAllBacSiChuyenKhoasInput input)
         {
			
			var filteredBacSiChuyenKhoas = _bacSiChuyenKhoaRepository.GetAll()
						.Include( e => e.UserFk)
						.Include( e => e.ChuyenKhoaFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ChuyenKhoaTenFilter), e => e.ChuyenKhoaFk != null && e.ChuyenKhoaFk.Ten == input.ChuyenKhoaTenFilter);

			var pagedAndFilteredBacSiChuyenKhoas = filteredBacSiChuyenKhoas
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var bacSiChuyenKhoas = from o in pagedAndFilteredBacSiChuyenKhoas
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_chuyenKhoaRepository.GetAll() on o.ChuyenKhoaId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3  in _thongTinBacSiMoRongRepository.GetAll() on o.UserId equals o3.UserId into  j3
                         from s3 in j3.DefaultIfEmpty() 
                         
                         select new GetBacSiChuyenKhoaForViewDto() {
							BacSiChuyenKhoa = new BacSiChuyenKhoaDto
							{
                                Id = o.Id,
                                UserId = o.UserId
							},
                         	UserName = s1 == null || s1.Name == null ? "" : ($"{s1.Surname} {s1.Name}"),
                         	ChuyenKhoaTen = s2 == null || s2.Ten == null ? "" : s2.Ten.ToString(),
                            ThongTinBacSiMoRong = new ThongTinBacSiMoRongDto
                            {
	                            Id = s3.Id,
	                            ChucDanh = s3.ChucDanh,
	                            TieuSu = s3.TieuSu,
	                            Image = s3.Image
                            }
						};

            var totalCount = await filteredBacSiChuyenKhoas.CountAsync();

            return new PagedResultDto<GetBacSiChuyenKhoaForViewDto>(
                totalCount,
                await bacSiChuyenKhoas.ToListAsync()
            );
         }
		 
		 public async Task<GetBacSiChuyenKhoaForViewDto> GetBacSiChuyenKhoaForView(int id)
         {
            var bacSiChuyenKhoa = await _bacSiChuyenKhoaRepository.GetAsync(id);

            var output = new GetBacSiChuyenKhoaForViewDto { BacSiChuyenKhoa = ObjectMapper.Map<BacSiChuyenKhoaDto>(bacSiChuyenKhoa) };

		    if (output.BacSiChuyenKhoa.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.BacSiChuyenKhoa.UserId);
                output.UserName = $"{_lookupUser?.Surname?.ToString()} {_lookupUser?.Name?.ToString()}";
            }

		    if (output.BacSiChuyenKhoa.ChuyenKhoaId != null)
            {
                var _lookupChuyenKhoa = await _lookup_chuyenKhoaRepository.FirstOrDefaultAsync((int)output.BacSiChuyenKhoa.ChuyenKhoaId);
                output.ChuyenKhoaTen = _lookupChuyenKhoa?.Ten?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_BacSiChuyenKhoas_Edit)]
		 public async Task<GetBacSiChuyenKhoaForEditOutput> GetBacSiChuyenKhoaForEdit(EntityDto input)
         {
            var bacSiChuyenKhoa = await _bacSiChuyenKhoaRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetBacSiChuyenKhoaForEditOutput {BacSiChuyenKhoa = ObjectMapper.Map<CreateOrEditBacSiChuyenKhoaDto>(bacSiChuyenKhoa)};

		    if (output.BacSiChuyenKhoa.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.BacSiChuyenKhoa.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

		    if (output.BacSiChuyenKhoa.ChuyenKhoaId != null)
            {
                var _lookupChuyenKhoa = await _lookup_chuyenKhoaRepository.FirstOrDefaultAsync((int)output.BacSiChuyenKhoa.ChuyenKhoaId);
                output.ChuyenKhoaTen = _lookupChuyenKhoa?.Ten?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditBacSiChuyenKhoaDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_BacSiChuyenKhoas_Create)]
		 protected virtual async Task Create(CreateOrEditBacSiChuyenKhoaDto input)
         {
            //var bacSiChuyenKhoa = ObjectMapper.Map<BacSiChuyenKhoa>(input);
            // if (AbpSession.TenantId != null)
            // {
	           //  bacSiChuyenKhoa.TenantId = (int?) AbpSession.TenantId;
            // }
            // await _bacSiChuyenKhoaRepository.InsertAsync(bacSiChuyenKhoa);
            _bacSiChuyenKhoaRepository.Insert(new BacSiChuyenKhoa()
            {
	            CreationTime = DateTime.Now,
	            IsDeleted = false,
	            UserId = input.UserId,
	            ChuyenKhoaId = input.ChuyenKhoaId,
	            TenantId = AbpSession.TenantId
            });
            _thongTinBacSiMoRongRepository.Insert(new ThongTinBacSiMoRong()
            {
	            CreationTime = DateTime.Now,
	            IsDeleted = false,
	            Image = null,
	            ChucDanh = input.ChucDanh,
	            TieuSu = input.TieuSu,
	            UserId = input.UserId,	
	            TenantId = AbpSession.TenantId
            });
         }

		 [AbpAuthorize(AppPermissions.Pages_BacSiChuyenKhoas_Edit)]
		 protected virtual async Task Update(CreateOrEditBacSiChuyenKhoaDto input)
         {
            var bacSiChuyenKhoa = await _bacSiChuyenKhoaRepository.FirstOrDefaultAsync((int)input.Id);
            var thongTinBacSiMoRong =
	            await _thongTinBacSiMoRongRepository.FirstOrDefaultAsync((int)input.ThongTinBacSiMoRongId);
            thongTinBacSiMoRong.TieuSu = input.TieuSu;
            thongTinBacSiMoRong.ChucDanh = input.ChucDanh;
            await _thongTinBacSiMoRongRepository.UpdateAsync(thongTinBacSiMoRong);
            ObjectMapper.Map(input, bacSiChuyenKhoa);
         }

		 [AbpAuthorize(AppPermissions.Pages_BacSiChuyenKhoas_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _bacSiChuyenKhoaRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetBacSiChuyenKhoasToExcel(GetAllBacSiChuyenKhoasForExcelInput input)
         {
			
			var filteredBacSiChuyenKhoas = _bacSiChuyenKhoaRepository.GetAll()
						.Include( e => e.UserFk)
						.Include( e => e.ChuyenKhoaFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.ChuyenKhoaTenFilter), e => e.ChuyenKhoaFk != null && e.ChuyenKhoaFk.Ten == input.ChuyenKhoaTenFilter);

			var query = (from o in filteredBacSiChuyenKhoas
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_chuyenKhoaRepository.GetAll() on o.ChuyenKhoaId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetBacSiChuyenKhoaForViewDto() { 
							BacSiChuyenKhoa = new BacSiChuyenKhoaDto
							{
                                Id = o.Id
							},
                         	UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                         	ChuyenKhoaTen = s2 == null || s2.Ten == null ? "" : s2.Ten.ToString()
						 });


            var bacSiChuyenKhoaListDtos = await query.ToListAsync();

            return _bacSiChuyenKhoasExcelExporter.ExportToFile(bacSiChuyenKhoaListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_BacSiChuyenKhoas)]
         public async Task<PagedResultDto<BacSiChuyenKhoaUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<BacSiChuyenKhoaUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new BacSiChuyenKhoaUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<BacSiChuyenKhoaUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_BacSiChuyenKhoas)]
         public async Task<PagedResultDto<BacSiChuyenKhoaChuyenKhoaLookupTableDto>> GetAllChuyenKhoaForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_chuyenKhoaRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Ten != null && e.Ten.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var chuyenKhoaList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<BacSiChuyenKhoaChuyenKhoaLookupTableDto>();
			foreach(var chuyenKhoa in chuyenKhoaList){
				lookupTableDtoList.Add(new BacSiChuyenKhoaChuyenKhoaLookupTableDto
				{
					Id = chuyenKhoa.Id,
					DisplayName = chuyenKhoa.Ten?.ToString()
				});
			}

            return new PagedResultDto<BacSiChuyenKhoaChuyenKhoaLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
         public async Task CapNhatAnhBacSi(CapNhatAnhBacSiInput input)
         {
	         byte[] byteArray;

	         var imageBytes = _tempFileCacheManager.GetFile(input.FileToken);

	         if (imageBytes == null)
	         {
		         throw new UserFriendlyException("There is no such image file with the token: " + input.FileToken);
	         }

	         using (var bmpImage = new Bitmap(new MemoryStream(imageBytes)))
	         {
		         var width = (input.Width == 0 || input.Width > bmpImage.Width) ? bmpImage.Width : input.Width;
		         var height = (input.Height == 0 || input.Height > bmpImage.Height) ? bmpImage.Height : input.Height;
		         var bmCrop = bmpImage.Clone(new Rectangle(input.X, input.Y, width, height), bmpImage.PixelFormat);

		         using (var stream = new MemoryStream())
		         {
			         bmCrop.Save(stream, bmpImage.RawFormat);
			         byteArray = stream.ToArray();
		         }
	         }

	         if (byteArray.Length > MaxProfilPictureBytes)
	         {
		         throw new UserFriendlyException(L("ResizedProfilePicture_Warn_SizeLimit", AppConsts.ResizedMaxProfilPictureBytesUserFriendlyValue));
	         }

	         // var user = await UserManager.GetUserByIdAsync(AbpSession.GetUserId());
	         //
	         // if (user.ProfilePictureId.HasValue)
	         // {
		        //  await _binaryObjectManager.DeleteAsync(user.ProfilePictureId.Value);
	         // }
	         //
	         // var storedFile = new BinaryObject(AbpSession.TenantId, byteArray);
	         // await _binaryObjectManager.SaveAsync(storedFile);
	         //
	         // user.ProfilePictureId = storedFile.Id;

	         var thongTinBacSiMoRong =
		         await _thongTinBacSiMoRongRepository.FirstOrDefaultAsync(p => p.Id == input.ThongTinBacSiMoRongId);
				
	         if (!thongTinBacSiMoRong.Image.isNull())
	         {
		         try
		         {
			         var guid = Guid.Parse(thongTinBacSiMoRong.Image);
			         await _binaryObjectManager.DeleteAsync(guid);
		         }
		         catch (Exception e)
		         {
			         Console.WriteLine(e);
			         throw new UserFriendlyException("Input ảnh không đúng định dạng");
		         }
	         }

	         var storedFile = new BinaryObject(AbpSession.TenantId, byteArray);
	         await _binaryObjectManager.SaveAsync(storedFile);

	         thongTinBacSiMoRong.Image = storedFile.Id.ToString();
         }
         
    }
}