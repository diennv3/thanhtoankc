using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;

using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Models.DichVus
{
    public class CreateOrEditDichVuModalViewModel
    {
       public CreateOrEditDichVuDto DichVu { get; set; }

	   		public string ChuyenKhoaTen { get; set;}


       
	   public bool IsEditMode => DichVu.Id.HasValue;
    }
}