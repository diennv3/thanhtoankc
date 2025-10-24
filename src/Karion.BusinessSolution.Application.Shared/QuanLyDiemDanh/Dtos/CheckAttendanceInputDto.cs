namespace Karion.BusinessSolution.QuanLyDiemDanh.Dtos
{
    public class CheckAttendanceInputDto
    {
        public int UserId { get; set; }
        public byte[] CapturedImage { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}