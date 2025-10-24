using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class GetNguoiBenhNotificationForEditOutput
    {
		public CreateOrEditNguoiBenhNotificationDto NguoiBenhNotification { get; set; }

		public string NguoiBenhUserName { get; set;}


    }
}