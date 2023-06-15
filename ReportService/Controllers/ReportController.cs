using Microsoft.AspNetCore.Mvc;
using ReportLibrary.Interfaces;
using ReportLibrary.Services.RabbitMq;

namespace ReportService.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("CreatedReport/{OtelId}")]
        public IActionResult CreatedReport(long OtelId)
        {
           var result =  _reportService.CreatedReport(OtelId);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetListReport()
        {
            var result = await _reportService.GetListReport();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> GetReportDetail([FromBody] long Id)
        {
            var result =await  _reportService.GetReportDetail(Id);
            return Ok(result);
        }
    }
}
