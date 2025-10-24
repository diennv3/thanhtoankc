using Karion.BusinessSolution.QuanLyDanhMuc;
using Karion.BusinessSolution.QuanLyDanhMuc;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Repositories;
using Karion.BusinessSolution.QuanLyDanhMuc.Exporting;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Karion.BusinessSolution.Dto;
using Abp.Application.Services.Dto;
using Karion.BusinessSolution.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using AutoMapper.QueryableExtensions;
using Karion.BusinessSolution.APINganHang;
using Karion.BusinessSolution.Configuration;
using Karion.BusinessSolution.Extension.Karion.BusinessSolution.Common;
using Karion.BusinessSolution.ThanhToanKhongChamSocket;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace Karion.BusinessSolution.QuanLyDanhMuc
{
	[AbpAuthorize(AppPermissions.Pages_ChiTietThanhToans)]
    public class ChiTietThanhToansAppService : BusinessSolutionAppServiceBase, IChiTietThanhToansAppService
    {
		 private readonly IRepository<ChiTietThanhToan> _chiTietThanhToanRepository;
		 private readonly IChiTietThanhToansExcelExporter _chiTietThanhToansExcelExporter;
		 private readonly IRepository<LichHenKham,int> _lookup_lichHenKhamRepository;
		 private readonly IRepository<NguoiBenh,int> _lookup_nguoiBenhRepository;
		 private readonly IConfigurationRoot _appConfiguration;
		 private readonly IWebHostEnvironment _hostingEnvironment;
		 private readonly IThanhToanKhongChamCommunicator _thanhToanKhongChamCommunicator;
		 private readonly IRepository<ThongTinDonVi> _thongTinDonViRepository;

		  public ChiTietThanhToansAppService(IRepository<ChiTietThanhToan> chiTietThanhToanRepository, 
			                                 IChiTietThanhToansExcelExporter chiTietThanhToansExcelExporter ,
											 IRepository<LichHenKham, int> lookup_lichHenKhamRepository, 
											 IRepository<NguoiBenh, int> lookup_nguoiBenhRepository,
											 IWebHostEnvironment env,
			                                 IThanhToanKhongChamCommunicator thanhToanKhongChamCommunicator,
			                                 IRepository<ThongTinDonVi> thongTinDonViRepository) 
		  {
			_chiTietThanhToanRepository = chiTietThanhToanRepository;
			_chiTietThanhToansExcelExporter = chiTietThanhToansExcelExporter;
			_lookup_lichHenKhamRepository = lookup_lichHenKhamRepository;
			_lookup_nguoiBenhRepository = lookup_nguoiBenhRepository;
			_appConfiguration = env.GetAppConfiguration();
			_hostingEnvironment = env;
			_thanhToanKhongChamCommunicator = thanhToanKhongChamCommunicator;
			_thongTinDonViRepository = thongTinDonViRepository;
		  }

		 public async Task<PagedResultDto<GetChiTietThanhToanForViewDto>> GetAll(GetAllChiTietThanhToansInput input)
         {
			
			var filteredChiTietThanhToans = _chiTietThanhToanRepository.GetAll()
						.Include( e => e.LichHenKhamFk)
						.Include( e => e.NguoiBenhFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(input.MinSoTienThanhToanFilter != null, e => e.SoTienThanhToan >= input.MinSoTienThanhToanFilter)
						.WhereIf(input.MaxSoTienThanhToanFilter != null, e => e.SoTienThanhToan <= input.MaxSoTienThanhToanFilter)
						.WhereIf(input.MinLoaiThanhToanFilter != null, e => e.LoaiThanhToan >= input.MinLoaiThanhToanFilter)
						.WhereIf(input.MaxLoaiThanhToanFilter != null, e => e.LoaiThanhToan <= input.MaxLoaiThanhToanFilter)
						.WhereIf(input.MinNgayThanhToanFilter != null, e => e.NgayThanhToan >= input.MinNgayThanhToanFilter)
						.WhereIf(input.MaxNgayThanhToanFilter != null, e => e.NgayThanhToan <= input.MaxNgayThanhToanFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LichHenKhamMoTaTrieuChungFilter), e => e.LichHenKhamFk != null && e.LichHenKhamFk.MoTaTrieuChung == input.LichHenKhamMoTaTrieuChungFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NguoiBenhUserNameFilter), e => e.NguoiBenhFk != null && e.NguoiBenhFk.UserName == input.NguoiBenhUserNameFilter);

			var pagedAndFilteredChiTietThanhToans = filteredChiTietThanhToans
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var chiTietThanhToans = from o in pagedAndFilteredChiTietThanhToans
                         join o1 in _lookup_lichHenKhamRepository.GetAll() on o.LichHenKhamId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_nguoiBenhRepository.GetAll() on o.NguoiBenhId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetChiTietThanhToanForViewDto() {
							ChiTietThanhToan = new ChiTietThanhToanDto
							{
                                SoTienThanhToan = o.SoTienThanhToan,
                                LoaiThanhToan = o.LoaiThanhToan,
                                NgayThanhToan = o.NgayThanhToan,
                                Id = o.Id
							},
                         	LichHenKhamMoTaTrieuChung = s1 == null || s1.MoTaTrieuChung == null ? "" : s1.MoTaTrieuChung.ToString(),
                         	NguoiBenhUserName = s2 == null || s2.UserName == null ? "" : s2.UserName.ToString()
						};

            var totalCount = await filteredChiTietThanhToans.CountAsync();

            return new PagedResultDto<GetChiTietThanhToanForViewDto>(
                totalCount,
                await chiTietThanhToans.ToListAsync()
            );
         }
		 
		 public async Task<GetChiTietThanhToanForViewDto> GetChiTietThanhToanForView(int id)
         {
            var chiTietThanhToan = await _chiTietThanhToanRepository.GetAsync(id);

            var output = new GetChiTietThanhToanForViewDto { ChiTietThanhToan = ObjectMapper.Map<ChiTietThanhToanDto>(chiTietThanhToan) };

		    if (output.ChiTietThanhToan.LichHenKhamId != null)
            {
                var _lookupLichHenKham = await _lookup_lichHenKhamRepository.FirstOrDefaultAsync((int)output.ChiTietThanhToan.LichHenKhamId);
                output.LichHenKhamMoTaTrieuChung = _lookupLichHenKham?.MoTaTrieuChung?.ToString();
            }

		    if (output.ChiTietThanhToan.NguoiBenhId != null)
            {
                var _lookupNguoiBenh = await _lookup_nguoiBenhRepository.FirstOrDefaultAsync((int)output.ChiTietThanhToan.NguoiBenhId);
                output.NguoiBenhUserName = _lookupNguoiBenh?.UserName?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_ChiTietThanhToans_Edit)]
		 public async Task<GetChiTietThanhToanForEditOutput> GetChiTietThanhToanForEdit(EntityDto input)
         {
            var chiTietThanhToan = await _chiTietThanhToanRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetChiTietThanhToanForEditOutput {ChiTietThanhToan = ObjectMapper.Map<CreateOrEditChiTietThanhToanDto>(chiTietThanhToan)};

		    if (output.ChiTietThanhToan.LichHenKhamId != null)
            {
                var _lookupLichHenKham = await _lookup_lichHenKhamRepository.FirstOrDefaultAsync((int)output.ChiTietThanhToan.LichHenKhamId);
                output.LichHenKhamMoTaTrieuChung = _lookupLichHenKham?.MoTaTrieuChung?.ToString();
            }

		    if (output.ChiTietThanhToan.NguoiBenhId != null)
            {
                var _lookupNguoiBenh = await _lookup_nguoiBenhRepository.FirstOrDefaultAsync((int)output.ChiTietThanhToan.NguoiBenhId);
                output.NguoiBenhUserName = _lookupNguoiBenh?.UserName?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditChiTietThanhToanDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_ChiTietThanhToans_Create)]
		 protected virtual async Task Create(CreateOrEditChiTietThanhToanDto input)
         {
            var chiTietThanhToan = ObjectMapper.Map<ChiTietThanhToan>(input);

			
			if (AbpSession.TenantId != null)
			{
				chiTietThanhToan.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _chiTietThanhToanRepository.InsertAsync(chiTietThanhToan);
         }

		 [AbpAuthorize(AppPermissions.Pages_ChiTietThanhToans_Edit)]
		 protected virtual async Task Update(CreateOrEditChiTietThanhToanDto input)
         {
            var chiTietThanhToan = await _chiTietThanhToanRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, chiTietThanhToan);
         }

		 [AbpAuthorize(AppPermissions.Pages_ChiTietThanhToans_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _chiTietThanhToanRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetChiTietThanhToansToExcel(GetAllChiTietThanhToansForExcelInput input)
         {
			
			var filteredChiTietThanhToans = _chiTietThanhToanRepository.GetAll()
						.Include( e => e.LichHenKhamFk)
						.Include( e => e.NguoiBenhFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(input.MinSoTienThanhToanFilter != null, e => e.SoTienThanhToan >= input.MinSoTienThanhToanFilter)
						.WhereIf(input.MaxSoTienThanhToanFilter != null, e => e.SoTienThanhToan <= input.MaxSoTienThanhToanFilter)
						.WhereIf(input.MinLoaiThanhToanFilter != null, e => e.LoaiThanhToan >= input.MinLoaiThanhToanFilter)
						.WhereIf(input.MaxLoaiThanhToanFilter != null, e => e.LoaiThanhToan <= input.MaxLoaiThanhToanFilter)
						.WhereIf(input.MinNgayThanhToanFilter != null, e => e.NgayThanhToan >= input.MinNgayThanhToanFilter)
						.WhereIf(input.MaxNgayThanhToanFilter != null, e => e.NgayThanhToan <= input.MaxNgayThanhToanFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LichHenKhamMoTaTrieuChungFilter), e => e.LichHenKhamFk != null && e.LichHenKhamFk.MoTaTrieuChung == input.LichHenKhamMoTaTrieuChungFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NguoiBenhUserNameFilter), e => e.NguoiBenhFk != null && e.NguoiBenhFk.UserName == input.NguoiBenhUserNameFilter);

			var query = (from o in filteredChiTietThanhToans
                         join o1 in _lookup_lichHenKhamRepository.GetAll() on o.LichHenKhamId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_nguoiBenhRepository.GetAll() on o.NguoiBenhId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetChiTietThanhToanForViewDto() { 
							ChiTietThanhToan = new ChiTietThanhToanDto
							{
                                SoTienThanhToan = o.SoTienThanhToan,
                                LoaiThanhToan = o.LoaiThanhToan,
                                NgayThanhToan = o.NgayThanhToan,
                                Id = o.Id
							},
                         	LichHenKhamMoTaTrieuChung = s1 == null || s1.MoTaTrieuChung == null ? "" : s1.MoTaTrieuChung.ToString(),
                         	NguoiBenhUserName = s2 == null || s2.UserName == null ? "" : s2.UserName.ToString()
						 });


            var chiTietThanhToanListDtos = await query.ToListAsync();

            return _chiTietThanhToansExcelExporter.ExportToFile(chiTietThanhToanListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_ChiTietThanhToans)]
         public async Task<PagedResultDto<ChiTietThanhToanLichHenKhamLookupTableDto>> GetAllLichHenKhamForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_lichHenKhamRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.MoTaTrieuChung != null && e.MoTaTrieuChung.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var lichHenKhamList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ChiTietThanhToanLichHenKhamLookupTableDto>();
			foreach(var lichHenKham in lichHenKhamList){
				lookupTableDtoList.Add(new ChiTietThanhToanLichHenKhamLookupTableDto
				{
					Id = lichHenKham.Id,
					DisplayName = lichHenKham.MoTaTrieuChung?.ToString()
				});
			}

            return new PagedResultDto<ChiTietThanhToanLichHenKhamLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_ChiTietThanhToans)]
         public async Task<PagedResultDto<ChiTietThanhToanNguoiBenhLookupTableDto>> GetAllNguoiBenhForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_nguoiBenhRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.UserName != null && e.UserName.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var nguoiBenhList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ChiTietThanhToanNguoiBenhLookupTableDto>();
			foreach(var nguoiBenh in nguoiBenhList){
				lookupTableDtoList.Add(new ChiTietThanhToanNguoiBenhLookupTableDto
				{
					Id = nguoiBenh.Id,
					DisplayName = nguoiBenh.UserName?.ToString()
				});
			}

            return new PagedResultDto<ChiTietThanhToanNguoiBenhLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

         public async Task<MBBankJsonResponDto> MBBankGetTransactionHistory(MBBankGetTransactionHistoryDto input)
         {
	         try
	         {
		         var currentTenantId = AbpSession.TenantId;
		         var currnttUserId = AbpSession.UserId;
		         IUserIdentifier receiver = new UserIdentifier(currentTenantId, (long)currnttUserId);
		         ResponseKiemTraThanhToanMBBank responseKiemTraThanhToanMBBank = new ResponseKiemTraThanhToanMBBank(); 
		         await _thanhToanKhongChamCommunicator.TestNotificationTTKC(receiver);
		         
		         //Lấy token
		         var userPass = _appConfiguration["MBBankAccount:MBB_USERNAME"] + ":" +
		                        _appConfiguration["MBBankAccount:MBB_PASSWORD"];
		         var tokenAuthString = CreateRequest.Base64Encode(userPass);
		         var tokenURL = _appConfiguration["MBBankAccount:MBB_TOKEN_URL"];
		         var tokenRequest = "grant_type=client_credentials";
		         var tokenContentType = "application/x-www-form-urlencoded";
		         string requestToken =
			         CreateRequest.webRequest(tokenURL, tokenRequest, tokenAuthString, "POST", tokenContentType);
		         var resultToken = JObject.Parse(requestToken);
		         //result Token temp
		         // { TH Thành công
		         //  "access_token": "c1aP1SqcBCJ9Y5KVQHWA55oQkEzU",
		         //  "expires_in": 597,
		         //  "scope": "",
		         //  "issued_at": "1654133101221",
		         //  "token_type": "Bearer"
		         // }
		         // { TH thất bại
		         //  "error": "invalid_request",
		         //  "error_description": "Required param : grant_type"
		         // }
		         var accessToken = "";
		         accessToken = resultToken["access_token"].ToString();
		         
		         //lấy transaction history
		         var UUId = System.Guid.NewGuid().ToString();
		         var bankAccount = await _thongTinDonViRepository.FirstOrDefaultAsync(p => p.Key.Equals("BankAccount"));
		         var accountNumber = bankAccount.Value;
		         var accountType = "ACCOUNT";
		         var fromDate = "2022-08-02T00:00:00";
		         var toDate = "2022-08-02T14:30:00"; 
		         // var fromDate = DateTime.Now.ToString("yyyy-MM-dd") + "T00:00:00";
		         // var toDate = DateTime.Now.AddSeconds(-30).ToString("yyyy-MM-dd") + "T" + DateTime.Now.AddSeconds(-30).ToString("HH:mm:ss");
		         
		         var getHistoryURL = _appConfiguration["MBBankAccount:MBB_GET_TRANSACTION_HISTORY_URL"];
		         var getHistotyContentType = "application/json";
		         getHistoryURL += "?accountNumber=" + accountNumber + "&" +
		                          "accountType=" + accountType + "&" +
		                          "fromDate=" + fromDate + "&" +
		                          "toDate=" + toDate;
		         string requestGetHistory =
			         CreateRequest.mBBankGetHistory(getHistoryURL,"", accessToken, UUId, "GET", getHistotyContentType);
		         var resultGetHistory = JObject.Parse(requestGetHistory);

		         //Xử lí 
		         var data = resultGetHistory["data"];
		         if (data.Count() == 0)
		         {
			         //không có dữ liệu trả về từ ngân hàng
			         responseKiemTraThanhToanMBBank.message = "Ngân hàng chưa nhận được giao dịch!";
			         responseKiemTraThanhToanMBBank.chiTietThanhToanId =  0;
			         responseKiemTraThanhToanMBBank.isUpdateTrangThaiThanhToan =  true;
			         await _thanhToanKhongChamCommunicator.ThongBaoThanhToanNganHangSignalR(receiver, responseKiemTraThanhToanMBBank);
		         }
		         else
		         {
			         var queryChiTietThanhToans =  
				          _chiTietThanhToanRepository.GetAll().WhereIf(true,
					         p => p.LichHenKhamId == input.lichHenKhamId && p.TrangThaiThanhToan == TechberConsts.CHUA_THANH_TOAN);
			          var chiTietThanhToans = (from o in queryChiTietThanhToans
				          select new ChiTietThanhToanDto()
				          {
					          Id = o.Id,
					          SoTienThanhToan = o.SoTienThanhToan,
					          LoaiThanhToan = o.LoaiThanhToan,
					          NgayThanhToan = o.NgayThanhToan,
					          LichHenKhamId = o.LichHenKhamId,
					          NguoiBenhId = o.NguoiBenhId,
					          TrangThaiThanhToan = o.TrangThaiThanhToan,
					          QRString = o.QRString
				          }).ToList();
			         
			         if (chiTietThanhToans.Count() == 0)
			         {
				         responseKiemTraThanhToanMBBank.message =  "Khách hàng không có dữ liệu cần kiểm tra thanh toán";
				         responseKiemTraThanhToanMBBank.chiTietThanhToanId =  0;
				         responseKiemTraThanhToanMBBank.isUpdateTrangThaiThanhToan =  false;
				         await _thanhToanKhongChamCommunicator.ThongBaoThanhToanNganHangSignalR(receiver, responseKiemTraThanhToanMBBank);
			         }
			         else
			         {
				         foreach (var chiTietThanhToanDetail in chiTietThanhToans)
				         {
					         foreach (var dataDetail in data)
					         {
						         var pattern = @"\s+";
						         string[] paymentDetail = Regex.Split(dataDetail["paymentDetail"].ToString().Trim(), pattern);
						         string amount = dataDetail["amount"].ToString().Trim();
						         //Lúc này chia mảng paymentDetail thành 3 phần tử
						         //paymentDetail[0] chứa tiền tố: TBR-TTKC
						         //paymentDetail[1] chứa Id của LichHenKham va ChiTietThanhToan: <LichHenKhamId>-<ChiTietThanhToanId>
						         //paymentDetail[2] chứa CCCD và hậu tố TT-CHI-PHI-KHAM-BENH: <CCCD>-TT-CHI-TIET-KHAM-BENH
						         if(paymentDetail[0].Equals("TBR-TTKC"))
						         {
							         var pattern1 =  @"-";
							         string[] duLieuIdLichHenVaChiTiet = Regex.Split(paymentDetail[1].Trim(), pattern1);
							         if (chiTietThanhToanDetail.LichHenKhamId.ToString().Equals(duLieuIdLichHenVaChiTiet[0]) 
							             && chiTietThanhToanDetail.Id.ToString().Equals(duLieuIdLichHenVaChiTiet[1])
							         )
							         {
								         if (chiTietThanhToanDetail.SoTienThanhToan == Decimal.Parse(amount))
								         {
									         ChiTietThanhToan chiTietThanhToanNguoiBenh = _chiTietThanhToanRepository
										         .FirstOrDefault(p =>
											         p.LichHenKhamId.ToString().Equals(duLieuIdLichHenVaChiTiet[0])
											         && p.Id.ToString().Equals(duLieuIdLichHenVaChiTiet[1])
										         );
									         chiTietThanhToanNguoiBenh.TrangThaiThanhToan = TechberConsts.DA_THANH_TOAN;
									         _chiTietThanhToanRepository.Update(chiTietThanhToanNguoiBenh);
									         responseKiemTraThanhToanMBBank.message =  "Đã cập nhật trạng thái thanh toán";
									         responseKiemTraThanhToanMBBank.chiTietThanhToanId =  Int32.Parse(duLieuIdLichHenVaChiTiet[1]);
									         responseKiemTraThanhToanMBBank.isUpdateTrangThaiThanhToan =  true;
									         await _thanhToanKhongChamCommunicator.ThongBaoThanhToanNganHangSignalR(receiver, responseKiemTraThanhToanMBBank);
								         }
								         else
								         {
									         responseKiemTraThanhToanMBBank.message =  "Số tiền cần thanh toán và số tiền thanh toán không khớp";
									         responseKiemTraThanhToanMBBank.chiTietThanhToanId =  0;
									         responseKiemTraThanhToanMBBank.isUpdateTrangThaiThanhToan =  false;
									         await _thanhToanKhongChamCommunicator.ThongBaoThanhToanNganHangSignalR(receiver, responseKiemTraThanhToanMBBank);
								         }
							         }
						         }
						         else
						         {
							         responseKiemTraThanhToanMBBank.message =  "Giao dịch chưa được thực hiện hoặc cú pháp không đúng";
							         responseKiemTraThanhToanMBBank.chiTietThanhToanId =  0;
							         responseKiemTraThanhToanMBBank.isUpdateTrangThaiThanhToan =  false;
							         await _thanhToanKhongChamCommunicator.ThongBaoThanhToanNganHangSignalR(receiver, responseKiemTraThanhToanMBBank);
						         }
					         }
				         }
			         }
		         }
		         
		         return new MBBankJsonResponDto()
		         {
			         status = true,
			         message = "Thành công"
		         };
		        
	         }
	         catch (Exception e)
	         {
		         return new MBBankJsonResponDto()
		         {
			         status = false,
			         message = "Có lỗi xảy ra"
		         };
	         }
         }
         
         public async Task KiemTraThanhToanNganHang(KiemTraThanhToanNganHangDto input)
         {

	         try
	         {
		         var bankCode = 
			         await _thongTinDonViRepository.FirstOrDefaultAsync(p => p.Key.Equals("BankCode"));
		         if (bankCode.Value.Equals("970422"))
		         {
			         var dataInput = new MBBankGetTransactionHistoryDto();
			         dataInput.lichHenKhamId = input.lichHenKhamId;
			         await MBBankGetTransactionHistory(dataInput);
		         }
	         }
	         catch (Exception e)
	         {
		         Console.WriteLine(e);
		         throw;
	         }
	         // var currentTenantId = AbpSession.TenantId;
	         // var currnttUserId = AbpSession.UserId;
	         // IUserIdentifier receiver = new UserIdentifier(currentTenantId, (long)currnttUserId);
	         // await _thanhToanKhongChamCommunicator.TestNotificationTTKC(receiver);
         }
    }
}