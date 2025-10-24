using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class CreateOrEditBacSiChuyenKhoaDto : EntityDto<int?>
    {
        public long UserId { get; set; }

        public int ChuyenKhoaId { get; set; }
        public string ChucDanh { get; set; }
        public string TieuSu { get; set; }
        public long ThongTinBacSiMoRongId  { get; set; }
    }
}