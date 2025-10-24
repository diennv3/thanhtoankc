using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;

using Abp.Extensions;

namespace Karion.BusinessSolution.Web.Areas.App.Models.NguoiBenhNotifications
{
    public class CreateOrEditNguoiBenhNotificationModalViewModel
    {
       public CreateOrEditNguoiBenhNotificationDto NguoiBenhNotification { get; set; }

	   		public string NguoiBenhUserName { get; set;}


       
	   public bool IsEditMode => NguoiBenhNotification.Id.HasValue;
    }
}