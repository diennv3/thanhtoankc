using Karion.BusinessSolution.QuanLyDanhMuc;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;

namespace Karion.BusinessSolution.QuanLyDiemDanh
{
    [Table("Attendances")]
    public class Attendance : AuditedEntity, IMayHaveTenant
    {
        // Tenant
        public int? TenantId { get; set; }

        // Tọa độ check-in
        public virtual double CheckInLatitude { get; set; }
        public virtual double CheckInLongitude { get; set; }

        // Kết quả so khớp khuôn mặt
        public virtual bool IsCheckInFaceMatched { get; set; }
        public virtual double CheckInFaceMatchPercentage { get; set; }

        // Có trong phạm vi vị trí hay không
        public virtual bool IsWithinLocation { get; set; }

        // Thông tin thiết bị (vẫn là string, có thể lưu thêm metadata như Shift:{name};Device:{...})
        public virtual string CheckInDeviceInfo { get; set; }

        // Ảnh (có thể là base64 hoặc đường dẫn)
        public virtual string PhotoPath { get; set; }

        // Thời gian check-in (MySQL: datetime(6) nếu muốn fractional seconds)
        [Column(TypeName = "datetime(6)")]
        public virtual DateTime CheckIn { get; set; }

        // Thời gian check-out (nullable)
        [Column(TypeName = "datetime(6)")]
        public virtual DateTime? CheckOut { get; set; }

        // Cờ xác định bản ghi này đã được check-out/đóng chưa
        public virtual bool IsCheckOut { get; set; }

        public virtual int? ShiftId { get; set; }

        [ForeignKey("ShiftId")]
        public virtual Karion.BusinessSolution.QuanLyDanhMuc.Shift ShiftFk { get; set; }

        [StringLength(128)]
        public virtual string ShiftName { get; set; }
        // Muộn khi check-in
        public virtual bool IsLateCheckIn { get; set; }

        // Cờ tăng ca + thời gian tăng ca (giữ tương thích với model gốc)
        public virtual bool IsOvertime { get; set; }

        [Column(TypeName = "datetime(6)")]
        public virtual DateTime? OvertimeStart { get; set; }

        [Column(TypeName = "datetime(6)")]
        public virtual DateTime? OvertimeEnd { get; set; }

        // Đánh dấu nửa ngày (tự động hoặc do workflow nghỉ)
        public virtual bool IsHalfDay { get; set; }

        // Quan hệ tới NguoiBenh
        public virtual int? NguoiBenhId { get; set; }

        [ForeignKey("NguoiBenhId")]
        public virtual NguoiBenh NguoiBenhFk { get; set; }
    }
}