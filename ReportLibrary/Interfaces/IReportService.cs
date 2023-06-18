using ReportLibrary.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportLibrary.Interfaces
{
    public interface IReportService
    {
        Task<ResponseData<List<Report>>> GetListReport();
        Task<ResponseData<bool>> CreatedReport(long HotelId);
        Task<ResponseData<ReportDetail>> GetReportDetail(long Id);

    }
}
