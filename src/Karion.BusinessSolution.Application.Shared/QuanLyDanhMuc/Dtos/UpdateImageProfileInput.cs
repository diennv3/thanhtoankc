using System;
using System.Collections.Generic;
using System.Text;

namespace Karion.BusinessSolution.QuanLyDanhMuc.Dtos
{
    public class UpdateImageProfileInput
    {
        public int NguoiBenhId { get; set; }
        public byte[] Data { get; set; }
        public string JpegFileName { get; set; }
    }
}
