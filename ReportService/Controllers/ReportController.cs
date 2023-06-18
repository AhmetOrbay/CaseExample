using Microsoft.AspNetCore.Mvc;
using ReportLibrary.Interfaces;
using ReportLibrary.Model;
using ReportLibrary.Services.RabbitMq;
using System.Net;

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

        /// <summary>
        /// Created Report Request
        /// </summary>
        /// <param name="HotelId"></param>
        /// <returns></returns>

        [HttpGet("CreatedReport/{HotelId}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ResponseData<bool>), (int)HttpStatusCode.OK)]
        public async Task<ResponseData<bool>> CreatedReport(long HotelId)
        {
           var result = await  _reportService.CreatedReport(HotelId);
            return result;
        }

        /// <summary>
        /// Full Report List
        /// </summary>
        /// <returns></returns>
        [HttpPost("ReportList")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ResponseData<List<Report>>), (int)HttpStatusCode.OK)]
        public async Task<ResponseData<List<Report>>> GetListReport()
        {
            var result = await _reportService.GetListReport();
            return result;
        }

        /// <summary>
        /// Report by ID
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost("ReportDetail/{Id}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ResponseData<ReportDetail>), (int)HttpStatusCode.OK)]
        public async Task<ResponseData<ReportDetail>> GetReportDetail([FromBody] long Id)
        {
            var result =await  _reportService.GetReportDetail(Id);
            return result;
        }
    }
}
