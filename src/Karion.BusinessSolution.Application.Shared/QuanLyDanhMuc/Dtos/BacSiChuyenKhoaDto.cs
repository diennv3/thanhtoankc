using System;
   using Abp.Application.Services.Dto;
   
   namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
   {
       public class BacSiChuyenKhoaDto : EntityDto
       {
           public long UserId { get; set; }
   
           public int ChuyenKhoaId { get; set; }
           public int Id { get; set; }
       }
   }