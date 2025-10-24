using System.Collections.Generic;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;
using Abp.Extensions;
using Karion.BusinessSolution.Dto;
using Karion.BusinessSolution.Extension.Karion.BusinessSolution.Common;

namespace Karion.BusinessSolution.Web.Areas.App.Models.BacSiChuyenKhoas
{
    public class CreateOrEditBacSiChuyenKhoaModalViewModel
    {
        public CreateOrEditBacSiChuyenKhoaDto BacSiChuyenKhoa { get; set; }
        public string UserName { get; set; }
        
        public string ChuyenKhoaTen { get; set; }
        public ThongTinBacSiMoRongDto ThongTinBacSiMoRong { get; set; }
        
        public bool IsEditMode => BacSiChuyenKhoa.Id.HasValue;
    }
}