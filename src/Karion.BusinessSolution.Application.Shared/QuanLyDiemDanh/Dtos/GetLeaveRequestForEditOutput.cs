using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.QuanLyDiemDanh.Dtos
{
    public class GetLeaveRequestForEditOutput
    {
		public CreateOrEditLeaveRequestDto LeaveRequest { get; set; }

		public string NguoiBenhUserName { get; set;}

		public string UserName { get; set;}

		public string ShiftName { get; set;}


    }
}