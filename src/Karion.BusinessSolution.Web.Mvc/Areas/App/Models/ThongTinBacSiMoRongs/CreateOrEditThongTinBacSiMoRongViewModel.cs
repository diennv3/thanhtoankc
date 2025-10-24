using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;

using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Models.ThongTinBacSiMoRongs
{
    public class CreateOrEditThongTinBacSiMoRongModalViewModel
    {
       public CreateOrEditThongTinBacSiMoRongDto ThongTinBacSiMoRong { get; set; }

	   		public string UserName { get; set;}


       
	   public bool IsEditMode => ThongTinBacSiMoRong.Id.HasValue;
    }
}