using System;
using Karion.BusinessSolution.Authorization.Users;

using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.Authorization;
using Abp.Authorization;
using Karion.BusinessSolution.HanetTenant;
using Microsoft.EntityFrameworkCore;
using Karion.BusinessSolution.MobileAppServices;
using Newtonsoft.Json;
using System.Net;
using Abp;
using Abp.Collections.Extensions;
using Abp.RealTime;
using Karion.BusinessSolution.Chat;
using Karion.BusinessSolution.Dto;
using Karion.BusinessSolution.Extension.Karion.BusinessSolution.Common;
using Karion.BusinessSolution.TBHostConfigure.Dtos;
using Karion.BusinessSolution.ThanhToanKhongChamSocket;
using Microsoft.AspNetCore.Mvc;
using GetAllForLookupTableInput = Karion.BusinessSolution.QuanLyDanhMuc.Dtos.GetAllForLookupTableInput;

namespace Karion.BusinessSolution.QuanLyDanhMuc
{
	[AbpAuthorize(AppPermissions.Pages_LichHenKhams)]
    public class LichHenKhamsAppService : BusinessSolutionAppServiceBase, ILichHenKhamsAppService
    {
		private readonly IRepository<LichHenKham> _lichHenKhamRepository;
		private readonly IRepository<HanetFaceDetected,long> _hanetFacesRepository;
		private readonly IRepository<HanetTenantPlaceDatas> _hanetPlacesRepository;
		private readonly IRepository<User,long> _lookup_userRepository;
		private readonly IRepository<NguoiBenh,int> _lookup_nguoiBenhRepository;
		private readonly IRepository<NguoiThan,int> _lookup_nguoiThanRepository;
		private readonly IRepository<ChuyenKhoa,int> _lookup_chuyenKhoaRepository;
		private readonly IRepository<DichVu, int> _dichVuRepository;
        private readonly IRepository<GiaDichVu, int> _giaDichVuRepository;
        private readonly IMobileAppServices _mobileAppServices;
        private readonly IRepository<ThongTinDonVi> _thongTinDonViRepository;
        private readonly IRepository<ChiTietThanhToan> _chiTietThanhToanRepository;
        private readonly IThanhToanKhongChamCommunicator _thanhToanKhongChamCommunicator;

        public LichHenKhamsAppService(IRepository<HanetTenantPlaceDatas> hanetPlacesRepository,
            IRepository<HanetFaceDetected,long> hanetFacesRepository,
            IRepository<LichHenKham> lichHenKhamRepository, 
            IRepository<User, long> lookup_userRepository, 
            IRepository<NguoiBenh, int> lookup_nguoiBenhRepository, 
            IRepository<NguoiThan, int> lookup_nguoiThanRepository, 
            IRepository<ChuyenKhoa, int> lookup_chuyenKhoaRepository,
            IRepository<DichVu, int> dichVuRepository,
            IRepository<GiaDichVu, int> giaDichVuRepository,
            IMobileAppServices mobileAppServices,
            IRepository<ThongTinDonVi> thongTinDonViRepository,
            IRepository<ChiTietThanhToan> chiTietThanhToanRepository,
            IThanhToanKhongChamCommunicator thanhToanKhongChamCommunicator
        )
        {
            _lichHenKhamRepository = lichHenKhamRepository;
            _lookup_userRepository = lookup_userRepository;
            _lookup_nguoiBenhRepository = lookup_nguoiBenhRepository;
            _lookup_nguoiThanRepository = lookup_nguoiThanRepository;
            _lookup_chuyenKhoaRepository = lookup_chuyenKhoaRepository;
            _hanetFacesRepository=hanetFacesRepository;
            _hanetPlacesRepository = hanetPlacesRepository;
            _dichVuRepository = dichVuRepository;
            _giaDichVuRepository = giaDichVuRepository;
            _mobileAppServices = mobileAppServices;
            _thongTinDonViRepository = thongTinDonViRepository;
            _chiTietThanhToanRepository = chiTietThanhToanRepository;
            _thanhToanKhongChamCommunicator = thanhToanKhongChamCommunicator;
        }

		 public async Task<PagedResultDto<GetLichHenKhamForViewDto>> GetAll(GetAllLichHenKhamsInput input)    
         {


			List<LichHenKhamDto> detects = await GetDetectedFacesHenKham();
			var selectedIdDetect = detects.Select(p => p.Id).ToList();
            var filteredLichHenKhams = _lichHenKhamRepository.GetAll()
                        .Include(e => e.BacSiFk)
                        .Include(e => e.ThuNganFk)
                        .Include(e => e.NguoiBenhFk)
                        .Include(e => e.NguoiThanFk)
                        .Include(e => e.ChuyenKhoaFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.MoTaTrieuChung.Contains(input.Filter) || e.SoTheBHYT.Contains(input.Filter) || e.NoiDangKyKCBDauTien.Contains(input.Filter) || e.ChiDinhDichVuSerialize.Contains(input.Filter))
                        .WhereIf(input.MinNgayHenKhamFilter != null, e => e.NgayHenKham >= input.MinNgayHenKhamFilter)
                        .WhereIf(input.MaxNgayHenKhamFilter != null, e => e.NgayHenKham <= input.MaxNgayHenKhamFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MoTaTrieuChungFilter), e => e.MoTaTrieuChung == input.MoTaTrieuChungFilter)
                        .WhereIf(input.IsCoBHYTFilter > -1, e => (input.IsCoBHYTFilter == 1 && e.IsCoBHYT) || (input.IsCoBHYTFilter == 0 && !e.IsCoBHYT))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SoTheBHYTFilter), e => e.SoTheBHYT == input.SoTheBHYTFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NoiDangKyKCBDauTienFilter), e => e.NoiDangKyKCBDauTien == input.NoiDangKyKCBDauTienFilter)
                        .WhereIf(input.MinBHYTValidDateFilter != null, e => e.BHYTValidDate >= input.MinBHYTValidDateFilter)
                        .WhereIf(input.MaxBHYTValidDateFilter != null, e => e.BHYTValidDate <= input.MaxBHYTValidDateFilter)
                        .WhereIf(input.MinPhuongThucThanhToanFilter != null, e => e.PhuongThucThanhToan >= input.MinPhuongThucThanhToanFilter)
                        .WhereIf(input.MaxPhuongThucThanhToanFilter != null, e => e.PhuongThucThanhToan <= input.MaxPhuongThucThanhToanFilter)
                        .WhereIf(input.IsDaKhamFilter > -1, e => (input.IsDaKhamFilter == 1 && e.IsDaKham) || (input.IsDaKhamFilter == 0 && !e.IsDaKham))
                        .WhereIf(input.IsDaThanhToanFilter > -1, e => (input.IsDaThanhToanFilter == 1 && e.IsDaThanhToan) || (input.IsDaThanhToanFilter == 0 && !e.IsDaThanhToan))
                        .WhereIf(input.MinTimeHoanThanhKhamFilter != null, e => e.TimeHoanThanhKham >= input.MinTimeHoanThanhKhamFilter)
                        .WhereIf(input.MaxTimeHoanThanhKhamFilter != null, e => e.TimeHoanThanhKham <= input.MaxTimeHoanThanhKhamFilter)
                        .WhereIf(input.MinTimeHoanThanhThanhToanFilter != null, e => e.TimeHoanThanhThanhToan >= input.MinTimeHoanThanhThanhToanFilter)
                        .WhereIf(input.MaxTimeHoanThanhThanhToanFilter != null, e => e.TimeHoanThanhThanhToan <= input.MaxTimeHoanThanhThanhToanFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ChiDinhDichVuSerializeFilter), e => e.ChiDinhDichVuSerialize == input.ChiDinhDichVuSerializeFilter)
                        .WhereIf(input.MinFlagFilter != null, e => e.Flag >= input.MinFlagFilter)
                        .WhereIf(input.MaxFlagFilter != null, e => e.Flag <= input.MaxFlagFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.BacSiFk != null && e.BacSiFk.Name == input.UserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserName2Filter), e => e.ThuNganFk != null && e.ThuNganFk.Name == input.UserName2Filter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NguoiBenhUserNameFilter), e => e.NguoiBenhFk != null && e.NguoiBenhFk.UserName == input.NguoiBenhUserNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NguoiThanHoVaTenFilter), e => e.NguoiThanFk != null && e.NguoiThanFk.HoVaTen == input.NguoiThanHoVaTenFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ChuyenKhoaTenFilter), e => e.ChuyenKhoaFk != null && e.ChuyenKhoaFk.Ten == input.ChuyenKhoaTenFilter)
                        .WhereIf(input.FlagFilter != null, e => e.Flag == input.FlagFilter);

			var pagedAndFilteredLichHenKhams = filteredLichHenKhams
                .OrderBy(input.Sorting ?? "ngayHenKham desc")
                .PageBy(input);

			var lichHenKhams = from o in pagedAndFilteredLichHenKhams
                         join o1 in _lookup_userRepository.GetAll() on o.BacSiId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_userRepository.GetAll() on o.ThuNganId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         join o3 in _lookup_nguoiBenhRepository.GetAll() on o.NguoiBenhId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()
                         
                         join o4 in _lookup_nguoiThanRepository.GetAll() on o.NguoiThanId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()
                         
                         join o5 in _lookup_chuyenKhoaRepository.GetAll() on o.ChuyenKhoaId equals o5.Id into j5
                         from s5 in j5.DefaultIfEmpty()

                         select new GetLichHenKhamForViewDto() {
							LichHenKham = new LichHenKhamDto
							{
                                NgayHenKham = o.NgayHenKham,
                                MoTaTrieuChung = o.MoTaTrieuChung,
                                IsCoBHYT = o.IsCoBHYT,
                                SoTheBHYT = o.SoTheBHYT,
                                NoiDangKyKCBDauTien = o.NoiDangKyKCBDauTien,
                                BHYTValidDate = o.BHYTValidDate,
                                PhuongThucThanhToan = o.PhuongThucThanhToan,
                                IsDaKham = o.IsDaKham,
                                IsDaThanhToan = o.IsDaThanhToan,
                                TimeHoanThanhKham = o.TimeHoanThanhKham,
                                TimeHoanThanhThanhToan = o.TimeHoanThanhThanhToan,
                                ChiDinhDichVuSerialize = o.ChiDinhDichVuSerialize,
                                TongTienThanhToan = o.TongTienDaThanhToan,
                                TienThua = o.TienThua,
                                Flag = o.Flag,
                                Id = o.Id,
                                ChuyenKhoaId = s5 != null ? s5.Id : -1,
                                TongChiPhi = o.TongChiPhi,
                                IsTamUng = o.IsTamUng
							},
                         	UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                         	UserName2 = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                         	NguoiBenhUserName = s3 == null || s3.UserName == null ? "" : s3.UserName.ToString(),
                         	NguoiThanHoVaTen = s4 == null || s4.HoVaTen == null ? "" : s4.HoVaTen.ToString(),
                         	ChuyenKhoaTen = s5 == null || s5.Ten == null ? "" : s5.Ten.ToString(),
                         	isNhanDien = selectedIdDetect.Contains(o.Id)
						};

            var totalCount = await filteredLichHenKhams.CountAsync();

            return new PagedResultDto<GetLichHenKhamForViewDto>(
                totalCount,
                // await lichHenKhams.OrderByDescending(p=>p.isNhanDien).ToListAsync()
                await lichHenKhams.ToListAsync()
            );
         }
		 
		 public async Task<GetLichHenKhamForViewDto> GetLichHenKhamForView(int id)
         {
            var lichHenKham = await _lichHenKhamRepository.GetAsync(id);

            var output = new GetLichHenKhamForViewDto { LichHenKham = ObjectMapper.Map<LichHenKhamDto>(lichHenKham) };

		    if (output.LichHenKham.BacSiId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.LichHenKham.BacSiId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

		    if (output.LichHenKham.ThuNganId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.LichHenKham.ThuNganId);
                output.UserName2 = _lookupUser?.Name?.ToString();
            }

		    if (output.LichHenKham.NguoiBenhId != null)
            {
                var _lookupNguoiBenh = await _lookup_nguoiBenhRepository.FirstOrDefaultAsync((int)output.LichHenKham.NguoiBenhId);
                output.NguoiBenhUserName = _lookupNguoiBenh?.UserName?.ToString();
            }

		    if (output.LichHenKham.NguoiThanId != null)
            {
                var _lookupNguoiThan = await _lookup_nguoiThanRepository.FirstOrDefaultAsync((int)output.LichHenKham.NguoiThanId);
                output.NguoiThanHoVaTen = _lookupNguoiThan?.HoVaTen?.ToString();
            }

		    if (output.LichHenKham.ChuyenKhoaId != null)
            {
                var _lookupChuyenKhoa = await _lookup_chuyenKhoaRepository.FirstOrDefaultAsync((int)output.LichHenKham.ChuyenKhoaId);
                output.ChuyenKhoaTen = _lookupChuyenKhoa?.Ten?.ToString();
            }
			
            return output;
         }

        public async Task<int> ChuyenBenhNhan(ThongTinChuyenDto input)
        {
            try
            {
                var lichHenKham = await _lichHenKhamRepository.FirstOrDefaultAsync(input.Id);
                if(lichHenKham != null)
                {
                    if (input.Flag == LichHenKhamAppServiceConst.LE_TAN_FLAG)
                    {
                        lichHenKham.Flag = LichHenKhamAppServiceConst.THU_NGAN_FLAG;
                    }
                    else if(input.Flag == LichHenKhamAppServiceConst.THU_NGAN_FLAG)
                    {
                        lichHenKham.TongChiPhi = input.TongChiPhi;
                    }
                }
                else
                {
                    return (int)HttpStatusCode.InternalServerError;
                }

                await _lichHenKhamRepository.UpdateAsync(lichHenKham);
                return (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                return (int)HttpStatusCode.InternalServerError;
            }
        }

        // code cũ
        //public async Task<int> ChuyenBenhNhan(ThongTinChuyenDto input)
        //{
        //    var lichHenKham = await _lichHenKhamRepository.FirstOrDefaultAsync(input.Id);

        //    if(lichHenKham != null)
        //    {
        //        if(input.Flag == 1 || input.Flag == 2)
        //        {
        //            lichHenKham.Flag = input.Flag + 1;
        //        }else if(input.Flag == 3)
        //        {
        //            if (!lichHenKham.IsDaThanhToan)
        //            {
        //                lichHenKham.IsDaThanhToan = true;
        //                var listDichVu = JsonConvert.DeserializeObject<List<DichVuSerializeDto>>(lichHenKham.ChiDinhDichVuSerialize);
        //                decimal sum = 0;
        //                sum = listDichVu.Select(x => x.Gia).Aggregate((a, b) =>
        //                {
        //                    return a + b;
        //                });
        //                lichHenKham.TongTienDaThanhToan = sum;
        //            }
        //            else
        //            {
        //                lichHenKham.IsDaKham = true;
        //            }
                    
        //        }

        //        await _lichHenKhamRepository.UpdateAsync(lichHenKham);
        //        return (int)HttpStatusCode.OK;
        //    }

        //    return (int)HttpStatusCode.InternalServerError;
        //}

        [AbpAuthorize(AppPermissions.Pages_LichHenKhams_Edit)]
		 public async Task<GetLichHenKhamForEditOutput> GetLichHenKhamForEdit(EntityDto input)
         {
            var lichHenKham = await _lichHenKhamRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetLichHenKhamForEditOutput {LichHenKham = ObjectMapper.Map<CreateOrEditLichHenKhamDto>(lichHenKham)};

		    if (output.LichHenKham.BacSiId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.LichHenKham.BacSiId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

		    if (output.LichHenKham.ThuNganId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.LichHenKham.ThuNganId);
                output.UserName2 = _lookupUser?.Name?.ToString();
            }

		    if (output.LichHenKham.NguoiBenhId != null)
            {
                var _lookupNguoiBenh = await _lookup_nguoiBenhRepository.FirstOrDefaultAsync((int)output.LichHenKham.NguoiBenhId);
                output.NguoiBenhUserName = _lookupNguoiBenh?.UserName?.ToString();
            }

		    if (output.LichHenKham.NguoiThanId != null)
            {
                var _lookupNguoiThan = await _lookup_nguoiThanRepository.FirstOrDefaultAsync((int)output.LichHenKham.NguoiThanId);
                output.NguoiThanHoVaTen = _lookupNguoiThan?.HoVaTen?.ToString();
            }

		    if (output.LichHenKham.ChuyenKhoaId != null)
            {
                var _lookupChuyenKhoa = await _lookup_chuyenKhoaRepository.FirstOrDefaultAsync((int)output.LichHenKham.ChuyenKhoaId);
                output.ChuyenKhoaTen = _lookupChuyenKhoa?.Ten?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditLichHenKhamDto input)
         {
             if (input.IsCoBHYT == false)
             {
                 input.SoTheBHYT = "";
                 input.NoiDangKyKCBDauTien = "";
                 input.BHYTValidDate = null;
             }
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_LichHenKhams_Create)]
		 protected virtual async Task Create(CreateOrEditLichHenKhamDto input)
         {
             input.Flag = 1;
            var lichHenKham = ObjectMapper.Map<LichHenKham>(input);

			
			if (AbpSession.TenantId != null)
			{
				lichHenKham.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _lichHenKhamRepository.InsertAsync(lichHenKham);
         }

		 [AbpAuthorize(AppPermissions.Pages_LichHenKhams_Edit)]
		 protected virtual async Task Update(CreateOrEditLichHenKhamDto input)
         {
            LichHenKham lichHenKham = await _lichHenKhamRepository.FirstOrDefaultAsync((int)input.Id);
            input.Flag = lichHenKham.Flag;
            ObjectMapper.Map(input, lichHenKham);
         }

		 [AbpAuthorize(AppPermissions.Pages_LichHenKhams_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _lichHenKhamRepository.DeleteAsync(input.Id);
         } 

		[AbpAuthorize(AppPermissions.Pages_LichHenKhams)]
         public async Task<PagedResultDto<LichHenKhamUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<LichHenKhamUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new LichHenKhamUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<LichHenKhamUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_LichHenKhams)]
         public async Task<PagedResultDto<LichHenKhamNguoiBenhLookupTableDto>> GetAllNguoiBenhForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_nguoiBenhRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.UserName != null && e.UserName.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var nguoiBenhList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<LichHenKhamNguoiBenhLookupTableDto>();
			foreach(var nguoiBenh in nguoiBenhList){
				lookupTableDtoList.Add(new LichHenKhamNguoiBenhLookupTableDto
				{
					Id = nguoiBenh.Id,
					DisplayName = nguoiBenh.UserName?.ToString()
				});
			}

            return new PagedResultDto<LichHenKhamNguoiBenhLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_LichHenKhams)]
         public async Task<PagedResultDto<LichHenKhamNguoiThanLookupTableDto>> GetAllNguoiThanForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_nguoiThanRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.HoVaTen != null && e.HoVaTen.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var nguoiThanList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<LichHenKhamNguoiThanLookupTableDto>();
			foreach(var nguoiThan in nguoiThanList){
				lookupTableDtoList.Add(new LichHenKhamNguoiThanLookupTableDto
				{
					Id = nguoiThan.Id,
					DisplayName = nguoiThan.HoVaTen?.ToString()
				});
			}

            return new PagedResultDto<LichHenKhamNguoiThanLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_LichHenKhams)]
         public async Task<PagedResultDto<LichHenKhamChuyenKhoaLookupTableDto>> GetAllChuyenKhoaForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_chuyenKhoaRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Ten != null && e.Ten.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var chuyenKhoaList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<LichHenKhamChuyenKhoaLookupTableDto>();
			foreach(var chuyenKhoa in chuyenKhoaList){
				lookupTableDtoList.Add(new LichHenKhamChuyenKhoaLookupTableDto
				{
					Id = chuyenKhoa.Id,
					DisplayName = chuyenKhoa.Ten?.ToString()
				});
			}

            return new PagedResultDto<LichHenKhamChuyenKhoaLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

         public async Task<List<LichHenKhamDto>> GetDetectedFacesHenKham()
         {
	         //get list place của tenants
	         var placeids = await _hanetPlacesRepository.GetAll().WhereIf(true,(p => p.tenantId == CurrentUnitOfWork.GetTenantId())).Select(p=>p.placeId).ToListAsync();

	         //get các facesDetect tương ứng với các placesId thuộc list place (dưới 20 phút)
	         var faceDetects = await _hanetFacesRepository.GetAll().WhereIf(true, p => placeids.Contains(p.placeId) && p.CreationTime>=DateTime.Now.AddMinutes(-20)).Select(p=>p.userDetectedId).ToListAsync();

	         //find lịch hẹn khám trong hôm nay của người bệnh theo list faceDetect
	         var lichHenKhams =
		         await _lichHenKhamRepository.GetAllListAsync(p => faceDetects.Contains(p.NguoiBenhId.ToString()) && p.NgayHenKham.Date==DateTime.Now.Date);
	         return ObjectMapper.Map<List<LichHenKhamDto>>(lichHenKhams);
         }

        public async Task<PagedResultDto<DichVuChuyenKhoaVaGiaDichVuDto>> GetDichVuChuyenKhoaVaGiaDichVu(GetDichVuChuyenKhoaVaGiaDichVuInput input)
        {
            var query = from d in _dichVuRepository.GetAll().Where(e => e.ChuyenKhoaId == input.ChuyenKhoaId)
                        join g in _giaDichVuRepository.GetAll() on d.Id equals g.DichVuId into j1
                        from s1 in j1.DefaultIfEmpty()
                        select new DichVuChuyenKhoaVaGiaDichVuDto
                        {
                            Id = d.Id,
                            // TODO: gia default hien tai de mac dinh 500k, sau nay du lieu se khong de null nen can sua lai code sau
                            Gia = s1 == null ? 500000 : s1.Gia,
                            MoTa = d.MoTa,
                            TenDichVu = d.Ten
                        };

            var totalCount = await query.CountAsync();

            var result = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<DichVuChuyenKhoaVaGiaDichVuDto>();
            foreach (var item in result)
            {
                lookupTableDtoList.Add(new DichVuChuyenKhoaVaGiaDichVuDto
                {
                    Id = item.Id,
                    Gia = item.Gia,
                    MoTa = item.MoTa,
                    TenDichVu = item.TenDichVu
                });
            }

            return new PagedResultDto<DichVuChuyenKhoaVaGiaDichVuDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        //TODO: uncomment phan quyen sau khi thiet lap hoan thien
        //[AbpAuthorize(AppPermissions.Pages_LichHenKhams_CapNhatDichVu)]
        public async Task<int> CapNhatDichVuLichHenKham(ThongTinCapNhatDichVuLichHenKhamDto input)
        {
            try
            {
                var lichHenKham = await _lichHenKhamRepository.GetAsync(input.Id);
                lichHenKham.ChiDinhDichVuSerialize = input.DichVuSerialized;
                await _lichHenKhamRepository.UpdateAsync(lichHenKham);
            }catch(Exception ex)
            {
                return (int)HttpStatusCode.InternalServerError;
            }

            return (int)HttpStatusCode.OK;
        }

        [HttpPost]
        //public string Generator(GeneratorQRDto input)
        public string Generator(int lichHenKhamId, int chiTietThanhToanId)
        {
            try
            {
                // LichHenKham lichHenKham = _lichHenKhamRepository.Get(id);
                var query = _lichHenKhamRepository.GetAll().Include(e => e.NguoiBenhFk);
                //LichHenKham lichHenKham = query.FirstOrDefault(p => p.Id == input.LichHenKhamId);
                LichHenKham lichHenKham = query.FirstOrDefault(p => p.Id == lichHenKhamId);
                ChiTietThanhToan chiTietThanhToan =
                   //_chiTietThanhToanRepository.FirstOrDefault(p => p.Id == input.ChiTietThanhToanId);
                    _chiTietThanhToanRepository.FirstOrDefault(p => p.Id == chiTietThanhToanId);

                //var result = JsonConvert.DeserializeObject<List<DichVuSerializeDto>>(lichHenKham.ChiDinhDichVuSerialize);
                //int toTal = result.Sum(item => item.Gia);

                //TODO tam thoi hardcode
                const string BANKCODE = "BankCode";
                const string BANKACCOUNT = "BankAccount";
                var bankCode = _thongTinDonViRepository.FirstOrDefault(e => e.Key == BANKCODE).Value;
                var bankAccount = _thongTinDonViRepository.FirstOrDefault(e => e.Key == BANKACCOUNT).Value;

                if (lichHenKham.TongTienDaThanhToan > 0)
                {
                    var qRstring = _mobileAppServices.PaymentQRGenerator(new Dto.MobileDto.QRInputDto()
                    {
                        amount = (int)chiTietThanhToan.SoTienThanhToan,//sotien,
                        bankAccount = bankAccount,//so tai khoan cua don vi (trong configure)
                        bankCode = bankCode,//trong danh muc,
                        noiDung = $"{lichHenKham.NguoiBenhFk.UserName}-TT-CHI-PHI-KHAM-BENH",
                        transactionUid = $"{lichHenKhamId}-{chiTietThanhToanId}"
                    });

                    if (!string.IsNullOrWhiteSpace(bankCode) && !string.IsNullOrWhiteSpace(bankAccount))
                    {
                        chiTietThanhToan.QRString = qRstring;
                        _chiTietThanhToanRepository.Update(chiTietThanhToan);
                        return qRstring;
                    }

                    return null;
                }
                else
                {
                    return null;
                }
            } catch(Exception ex)
            {
                //TODO chua quy dinh common Response cho frontend nen tam thoi return null
                return null;
            }
        }

        public async Task<int> HoanTatThanhToan(int id)
        {
            try
            {
                var lichHenKham = await _lichHenKhamRepository.GetAsync(id);
                if(lichHenKham != null)
                {
                    lichHenKham.IsDaThanhToan = true;
                    lichHenKham.IsDaKham = true;
                }
                else
                {
                    return (int)HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                return (int)HttpStatusCode.InternalServerError;
            }


            return (int)HttpStatusCode.OK;
        }
        public async Task<int> ThanhToanVienPhi(ThanhToanVienPhiDto input)
        {
            try
            {
                LichHenKham lichHenKham = await _lichHenKhamRepository.FirstOrDefaultAsync(input.Id);
                var soTienDaThanhToanUpdate = lichHenKham.TongTienDaThanhToan + input.SoTienThanhToan;
                var trangThai = (input.SoTienThanhToan < lichHenKham.TongChiPhi) ? TechberConsts.TAM_UNG : TechberConsts.KHONG_TAM_UNG;
                
                if(!lichHenKham.isNull())
                {
                    _chiTietThanhToanRepository.Insert(new ChiTietThanhToan()
                    {
                        NgayThanhToan = DateTime.Now,
                        LoaiThanhToan = 0,
                        NguoiBenhId = lichHenKham.NguoiBenhId,
                        LichHenKhamId = lichHenKham.Id,
                        SoTienThanhToan = input.SoTienThanhToan,
                        TenantId = AbpSession.TenantId
                    });
                    lichHenKham.TongTienDaThanhToan = soTienDaThanhToanUpdate;
                    lichHenKham.IsTamUng = trangThai;
                    _lichHenKhamRepository.Update(lichHenKham);
                }
                else
                {
                    return (int)HttpStatusCode.InternalServerError;
                }
            }
            catch (Exception ex)
            {
                return (int)HttpStatusCode.InternalServerError;
            }


            return (int)HttpStatusCode.OK;
        }

        //public async Task<int> CapNhatTienThua(ThongTinCapNhatTienThuaLichHenKhamDto input)
        //{
        //    try
        //    {
        //        var lichHenKham = await _lichHenKhamRepository.GetAsync(input.Id);
        //        lichHenKham.TienThua = input.TienThua;
        //        lichHenKham.IsDaKham = true;
        //        await _lichHenKhamRepository.UpdateAsync(lichHenKham);

        //    }catch(Exception ex)
        //    {
        //        return (int)HttpStatusCode.InternalServerError;
        //    }
        //    return (int)HttpStatusCode.OK;
        //}

        public async Task<GetBaoCaoOutput> GetBaoCao(GetBaoCaoInput input)
        {
            var dulieu = await _lichHenKhamRepository.GetAllListAsync(p=>p.NgayHenKham.Month==input.thang && p.NgayHenKham.Year==input.nam);
            GetBaoCaoOutput result = new GetBaoCaoOutput()
            {
                nam = input.nam,
                thang = input.thang,
                duLieuTho = new List<BaoCaoDataGroupedByDay>()
            };

            foreach (var VARIABLE in dulieu)
            {
                var t = result.duLieuTho.FirstOrDefault(p => p.date.Date == VARIABLE.NgayHenKham.Date);
                if (t.isNull())
                {
                    result.duLieuTho.Add(new BaoCaoDataGroupedByDay()
                    {
                        date = VARIABLE.NgayHenKham.Date,
                        doanhthu = VARIABLE.IsDaKham ? (int)VARIABLE.TongTienDaThanhToan : 0,
                        soNguoiChuaKham = (VARIABLE.IsDaKham?0:1),
                        soNguoiDaKham = (VARIABLE.IsDaKham?1:0),
                    });
                }
                else
                {
                    t.doanhthu += VARIABLE.IsDaKham ? (int) VARIABLE.TongTienDaThanhToan : 0;
                    t.soNguoiChuaKham += (VARIABLE.IsDaKham?0:1);
                    t.soNguoiDaKham += (VARIABLE.IsDaKham?1:0);
                    
                }
            }

            return result;
        }

        public async Task<GetKhungKhamResultDto> GetKhungKham(GetKhungKhamDto input)
        {
            var ngayHenKham = input.NgayHenKham.Day;
            var thangHenKham = input.NgayHenKham.Month;
            var namHenKham = input.NgayHenKham.Year;
            try
            {
                var query = _lichHenKhamRepository.GetAll()
                        .WhereIf(true, p => p.ChuyenKhoaId == input.ChuyenKhoaId)
                        .WhereIf(true, p => p.NgayHenKham.Day == ngayHenKham && p.NgayHenKham.Month == thangHenKham && p.NgayHenKham.Year == namHenKham)
                        ;
                var khamSession = await _thongTinDonViRepository.FirstOrDefaultAsync(p => p.Key.Equals("KhamSession"));
                int khamSessionValue = Int32.Parse(khamSession.Value);
                var gioBatDauLamViecSang = await _thongTinDonViRepository.FirstOrDefaultAsync(p => p.Key.Equals("GioBatDauLamViecSang"));
                var gioBatDauLamViecSangValue = gioBatDauLamViecSang.Value?? "";
                var gioKetThucLamViecSang = await _thongTinDonViRepository.FirstOrDefaultAsync(p => p.Key.Equals("GioKetThucLamViecSang"));
                var gioKetThucLamViecSangValue = gioKetThucLamViecSang.Value?? "";
                var gioBatDauLamViecChieu = await _thongTinDonViRepository.FirstOrDefaultAsync(p => p.Key.Equals("GioBatDauLamViecChieu"));
                var gioBatDauLamViecChieuValue = gioBatDauLamViecChieu.Value?? "";
                var gioKetThucLamViecChieu = await _thongTinDonViRepository.FirstOrDefaultAsync(p => p.Key.Equals("GioKetThucLamViecChieu"));
                var gioKetThucLamViecChieuValue = gioKetThucLamViecChieu.Value?? "";
                var lichHenKham = (from o in query
                    select new LichHenKhamDto()
                    {
                        NgayHenKham = o.NgayHenKham,
                        MoTaTrieuChung = o.MoTaTrieuChung,
                        IsCoBHYT = o.IsCoBHYT,
                        SoTheBHYT = o.SoTheBHYT,
                        NoiDangKyKCBDauTien = o.NoiDangKyKCBDauTien,
                        BHYTValidDate = o.BHYTValidDate,
                        PhuongThucThanhToan = o.PhuongThucThanhToan,
                        IsDaKham = o.IsDaKham,
                        IsDaThanhToan = o.IsDaThanhToan,
                        TimeHoanThanhKham = o.TimeHoanThanhKham,
                        TimeHoanThanhThanhToan = o.TimeHoanThanhThanhToan,
                        ChiDinhDichVuSerialize = o.ChiDinhDichVuSerialize,
                        Flag = o.Flag,
                        QRString = o.QRString,
                        BacSiId = o.BacSiId,
                        TienThua = o.TienThua,
                        ThuNganId = o.ThuNganId,
                        NguoiBenhId = o.NguoiBenhId,
                        NguoiThanId = o.NguoiThanId,
                        ChuyenKhoaId = o.ChuyenKhoaId,
                        KhungKham = (int)o.KhungKham
                    }).ToList();
                return new GetKhungKhamResultDto()
                {
                    LichHenKham = lichHenKham,
                    KhamSession = khamSessionValue,
                    GioBatDauLamViecSang = gioBatDauLamViecSangValue,
                    GioKetThucLamViecSang = gioKetThucLamViecSangValue,
                    GioBatDauLamViecChieu = gioBatDauLamViecChieuValue,
                    GioKetThucLamViecChieu = gioKetThucLamViecChieuValue
                };

            }    
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

       
    }
}