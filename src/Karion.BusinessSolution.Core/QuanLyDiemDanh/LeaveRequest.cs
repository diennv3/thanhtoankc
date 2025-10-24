using System;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Karion.BusinessSolution.QuanLyDanhMuc;
using Karion.BusinessSolution.Authorization.Users;

namespace Karion.BusinessSolution.QuanLyDiemDanh
{
    [Table("LeaveRequests")]
    public class LeaveRequest : FullAuditedEntity, IMayHaveTenant
    {
        
        public int? TenantId { get; set; }

        [Column(TypeName = "datetime(6)")]
        public virtual DateTime StartDateTime { get; set; }

        [Column(TypeName = "datetime(6)")]
        public virtual DateTime EndDateTime { get; set; }

        public virtual LeaveRequestType Type { get; set; } = LeaveRequestType.FullDay;

        public virtual HalfDayPart? HalfDayPart { get; set; }

        public virtual double? DurationHours { get; set; }

        public virtual LeaveRequestStatus Status { get; set; } = LeaveRequestStatus.Pending;

        public virtual string Reason { get; set; }

        public virtual long? ApprovedByUserId { get; set; }

        [Column(TypeName = "datetime(6)")]
        public virtual DateTime? ApprovedAt { get; set; }

        public virtual bool IsForEarlyLeave { get; set; }
        public virtual bool IsForLateArrival { get; set; }

        public virtual int? NguoiBenhId { get; set; }

        [ForeignKey("NguoiBenhId")]
        public virtual NguoiBenh NguoiBenhFk { get; set; }

        public virtual long? UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User UserFk { get; set; }

        public virtual int? ShiftId { get; set; }

        [ForeignKey("ShiftId")]
        public virtual Shift ShiftFk { get; set; }

        public LeaveRequest()
        {
            Status = LeaveRequestStatus.Pending;
            IsForEarlyLeave = false;
            IsForLateArrival = false;
            DurationHours = null;
        }
    }
}