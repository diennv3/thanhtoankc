using System;
using System.Collections.Generic;
using System.Text;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class ThongTinChuyenDto
    {
        public int Id { get; set; }

        public int Flag { get; set; }

        public decimal TongChiPhi { get; set; }

        public bool DaThanhToan { get; set; }
    }
}
