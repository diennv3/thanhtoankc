namespace Karion.BusinessSolution.Web.Areas.App.Models.LichHenKhams
{
	public class DanhSachDichVuHenKhamModel
	{
		public int Id { get; set; }
		public int ChuyenKhoaId { get; set; }
		public string SerializedDichVu { get; set; }
		public int Flag { get; set; }
		public string QRString { get; set; }
	}
}