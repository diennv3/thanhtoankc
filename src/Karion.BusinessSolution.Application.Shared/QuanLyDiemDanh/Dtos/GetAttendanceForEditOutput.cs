using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.QuanLyDiemDanh.Dtos
{
    public class GetAttendanceForEditOutput
    {
		public CreateOrEditAttendanceDto Attendance { get; set; }

		public string NguoiBenhUserName { get; set;}


    }
}