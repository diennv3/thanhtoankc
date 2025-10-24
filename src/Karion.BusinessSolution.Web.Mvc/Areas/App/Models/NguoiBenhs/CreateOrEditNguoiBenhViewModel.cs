using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;

using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Models.NguoiBenhs
{
    public class CreateOrEditNguoiBenhModalViewModel
    {
       public CreateOrEditNguoiBenhDto NguoiBenh { get; set; }

	   
       
	   public bool IsEditMode => NguoiBenh.Id.HasValue;
    }
}