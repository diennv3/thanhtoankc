using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;

using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Models.ChuyenKhoas
{
    public class CreateOrEditChuyenKhoaModalViewModel
    {
       public CreateOrEditChuyenKhoaDto ChuyenKhoa { get; set; }

	   
       
	   public bool IsEditMode => ChuyenKhoa.Id.HasValue;
    }
}