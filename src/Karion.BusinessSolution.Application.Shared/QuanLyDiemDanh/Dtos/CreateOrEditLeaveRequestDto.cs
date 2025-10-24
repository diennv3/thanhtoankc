using Karion.BusinessSolution.QuanLyDiemDanh;

using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.QuanLyDiemDanh.Dtos
{
    public class CreateOrEditLeaveRequestDto : EntityDto<int?>
    {

		public DateTime StartDateTime { get; set; }
		
		
		public DateTime EndDateTime { get; set; }
		
		
		public string LeaveType { get; set; }
		
		
		public LeaveRequestStatus Status { get; set; }
		
		
		public string Reason { get; set; }
		
		
		public bool IsForEarlyLeave { get; set; }
		
		
		public bool IsForLateArrival { get; set; }
		
		
		 public int? NguoiBenhId { get; set; }
		 
		 		 public long UserId { get; set; }
		 
		 		 public int ShiftId { get; set; }
		 
		 
    }
}