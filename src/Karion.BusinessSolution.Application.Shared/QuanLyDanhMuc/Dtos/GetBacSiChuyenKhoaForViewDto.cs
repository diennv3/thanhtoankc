namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class GetBacSiChuyenKhoaForViewDto
    {
		public BacSiChuyenKhoaDto BacSiChuyenKhoa { get; set; }

		public string UserName { get; set;}

		public string ChuyenKhoaTen { get; set;}

		public ThongTinBacSiMoRongDto ThongTinBacSiMoRong { get; set; }
    }
}