using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;

using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Models.GiaDichVus
{
    public class CreateOrEditGiaDichVuModalViewModel
    {
       public CreateOrEditGiaDichVuDto GiaDichVu { get; set; }

	   		public string DichVuTen { get; set;}


       
	   public bool IsEditMode => GiaDichVu.Id.HasValue;
    }
}