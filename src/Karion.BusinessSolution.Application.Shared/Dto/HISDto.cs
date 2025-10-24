using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace Karion.BusinessSolution.Dto
{
    public class HISDto
    {
        
    }
    public class HISNguoiBenhDto : EntityDto
    {
        public string HoVaTen { get; set; }
        public string CMND { get; set; }
        public string GioiTinh { get; set; }
        public string DiaChi { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public int NgaySinh { get; set; }
        public int ThangSinh { get; set; }
        public int NamSinh { get; set; }
        
    }
    public class HISNguoiThanDto : EntityDto
    {
        public string HoVaTen { get; set; }

        public int Tuoi { get; set; }

        public string GioiTinh { get; set; }

        public string DiaChi { get; set; }

        public string MoiQuanHe { get; set; }

        public string SoDienThoai { get; set; }
    }
    public class HISDtoGetAllRegistrationResult
    {
        public int RegistrationId { get; set; }
        public HISNguoiBenhDto NguoiDangKy { get; set; }
        public HISNguoiThanDto DangKyHoNguoiThan { get; set; }
        public string ChuyenKhoaName { get; set; }
        public int? DangKyHoNguoiThanId { get; set; }
        public DateTime NgayHenKham { get; set; }
        public string MoTaTrieuChung { get; set; }
    }
    public class HISDtoGetAllRegistrationInput
    {
        public string cmnd { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }

    public class ConfirmRegistrationSavedResponse
    {
        public bool status { get; set; }
        public string message { get; set; }
    }

    public class ConfirmRegistrationSavedInput
    {
        public List<int> RegistrationIds { get; set; }
    }

    public class UpdatePaymentAmountInputItem
    {
        public int RegistrationId { get; set; }
        public int Amount { get; set; }
    }
    public class UpdatePaymentAmountInput
    {
        public List<UpdatePaymentAmountInputItem> items { get; set; }
    }
    public class UpdatePaymentAmountResponseItem
    {
        public int RegistrationId { get; set; }
        public int Amount { get; set; }
        public bool Status { get; set; }
        public string Messenger { get; set; }
        public string QRHashCode { get; set; }
    }
    public class UpdatePaymentAmountResponse
    {
        public List<UpdatePaymentAmountResponseItem> items { get; set; }
    }
}