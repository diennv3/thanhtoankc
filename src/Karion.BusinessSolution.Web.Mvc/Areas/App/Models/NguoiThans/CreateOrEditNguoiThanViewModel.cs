using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;

using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Models.NguoiThans
{
    public class CreateOrEditNguoiThanModalViewModel
    {
       public CreateOrEditNguoiThanDto NguoiThan { get; set; }

	   		public string NguoiBenhHoVaTen { get; set;}


       
	   public bool IsEditMode => NguoiThan.Id.HasValue;
    }
}