using Karion.BusinessSolution.QuanLyDanhMuc;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using Abp.Auditing;

namespace Karion.BusinessSolution.QuanLyDanhMuc
{
	[Table("NguoiThans")]
    [Audited]
    public class NguoiThan : FullAuditedEntity 
    {

		[Required]
		[StringLength(NguoiThanConsts.MaxHoVaTenLength, MinimumLength = NguoiThanConsts.MinHoVaTenLength)]
		public virtual string HoVaTen { get; set; }
		
		public virtual int Tuoi { get; set; }
		
		public virtual string GioiTinh { get; set; }
		
		public virtual string DiaChi { get; set; }
		
		[StringLength(NguoiThanConsts.MaxMoiQuanHeLength, MinimumLength = NguoiThanConsts.MinMoiQuanHeLength)]
		public virtual string MoiQuanHe { get; set; }
		
		[StringLength(NguoiThanConsts.MaxSoDienThoaiLength, MinimumLength = NguoiThanConsts.MinSoDienThoaiLength)]
		public virtual string SoDienThoai { get; set; }
		

		public virtual int? NguoiBenhId { get; set; }
		
        [ForeignKey("NguoiBenhId")]
		public NguoiBenh NguoiBenhFk { get; set; }
		
    }
}