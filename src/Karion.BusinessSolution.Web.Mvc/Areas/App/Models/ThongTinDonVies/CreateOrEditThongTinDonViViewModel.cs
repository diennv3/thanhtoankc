using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;

using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Models.ThongTinDonVies
{
    public class CreateOrEditThongTinDonViModalViewModel
    {
       public CreateOrEditThongTinDonViDto ThongTinDonVi { get; set; }

	   
       
	   public bool IsEditMode => ThongTinDonVi.Id.HasValue;
    }
}