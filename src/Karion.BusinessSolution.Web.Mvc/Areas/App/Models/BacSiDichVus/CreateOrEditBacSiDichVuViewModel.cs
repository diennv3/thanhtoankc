using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;

using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Models.BacSiDichVus
{
    public class CreateOrEditBacSiDichVuModalViewModel
    {
       public CreateOrEditBacSiDichVuDto BacSiDichVu { get; set; }

	   		public string UserName { get; set;}

		public string DichVuTen { get; set;}


       
	   public bool IsEditMode => BacSiDichVu.Id.HasValue;
    }
}