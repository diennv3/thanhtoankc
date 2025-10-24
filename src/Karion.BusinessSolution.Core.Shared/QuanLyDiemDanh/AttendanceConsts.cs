namespace Karion.BusinessSolution.QuanLyDiemDanh
{
    public class AttendanceConsts
    {

						
						
    }

    public enum LOAI_BAO_CAO : int
    {
	    NGAY,
	    THANG,
	    QUY,
	    NAM
    }

    public enum WorkDayFlags
    {
        NONE = 0,
        SUNDAY = 1 << 0,   // 1
        MONDAY = 1 << 1,   // 2
        TUESDAY = 1 << 2,  // 4
        WEDNESDAY = 1 << 3,// 8
        THURSDAY = 1 << 4, // 16
        FRIDAY = 1 << 5,   // 32
        SATURDAY = 1 << 6  // 64
    }
}