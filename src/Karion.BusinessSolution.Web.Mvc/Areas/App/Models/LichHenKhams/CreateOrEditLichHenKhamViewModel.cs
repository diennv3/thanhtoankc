using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;

using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Models.LichHenKhams
{
    public class CreateOrEditLichHenKhamModalViewModel
    {
       public CreateOrEditLichHenKhamDto LichHenKham { get; set; }

	    public string UserName { get; set;}
	    public string UserName2 { get; set;}
	    public string NguoiBenhUserName { get; set;}
		public string NguoiThanHoVaTen { get; set;}

		public string DichVuTen { get; set;}
		public string ChuyenKhoaTen { get; set;}
		
		public bool IsEditMode => LichHenKham.Id.HasValue;
    }
}