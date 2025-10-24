using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Karion.BusinessSolution.QuanLyDanhMuc.Dtos;

namespace Karion.BusinessSolution.QuanLyDanhMuc
{
    public interface IThongKeBaoCaosAppService : IApplicationService  
    {
        Task<List<AttendanceDailyStatusDto>> GetDailyAttendanceStatus(DateTime date);
    }
}