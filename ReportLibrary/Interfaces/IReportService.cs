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
        Task<List<Report>> GetListReport();
        Task<bool> CreatedReport(long HotelId);
        Task<ReportDetail> GetReportDetail(long Id);
        Task AddReportDetail(ReportDetail modelDetail);

    }
}
