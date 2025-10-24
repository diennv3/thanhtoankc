using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;

using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Models.ChiTietThanhToans
{
    public class CreateOrEditChiTietThanhToanModalViewModel
    {
       public CreateOrEditChiTietThanhToanDto ChiTietThanhToan { get; set; }

	   		public string LichHenKhamMoTaTrieuChung { get; set;}

		public string NguoiBenhUserName { get; set;}


       
	   public bool IsEditMode => ChiTietThanhToan.Id.HasValue;
    }
}