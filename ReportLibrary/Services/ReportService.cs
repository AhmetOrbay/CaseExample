using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ReportLibrary.Interfaces;
using ReportLibrary.Model;
using ReportLibrary.Model.RabbitMqModel;
using ReportLibrary.Repositories;
using ReportLibrary.Services.RabbitMq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ReportLibrary.Services
{
    public class ReportService : IReportService
    {
        private readonly PublishRabbitMQService _publishService;
        private readonly ILogger<ReportService> _logger;
        private readonly ReportDbContext _reportDbContext;

        public ReportService(PublishRabbitMQService publishService
                    , ILogger<ReportService> logger
                    , ReportDbContext reportDbContext)
        {
            _publishService = publishService;
            _logger = logger;
            _reportDbContext = reportDbContext;
        }

        /// <summary>
        /// Report created and publish rabbitMq
        /// </summary>
        /// <param name="HotelId"></param>
        /// <returns></returns>
        public async Task<ResponseData<bool>> CreatedReport(long HotelId)
        {
            ResponseData<bool> model = new ();
            try
            {
                if (HotelId > 0)
                {
                    var entity = _reportDbContext.Reports.Add(new Report
                    {
                        CreatedDateTime = DateTime.Now.ToUniversalTime(),
                        Status = ReportStatus.Waiting
                    });
                    await _reportDbContext.SaveChangesAsync();
                    if (entity.Entity.Id > 0)
                    {
                        var publishModel = new PublishModel()
                        {
                            HotelId = HotelId,
                            ReportId = entity.Entity.Id
                        };
                        var resultPublish = _publishService.PublishMessage(
                            JsonSerializer.Serialize(publishModel), "ReportRequest");
                        model.Data = true;
                    }
                }
                else
                {
                    model.ErrorMessage = $"The sent value is incorrect.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Publish Error => {ex.Message}");
                model.ErrorMessage = $"The sent value is incorrect.";
            }
            return await Task.FromResult(model);
        }

        public async Task<ResponseData<List<Report>>> GetListReport()
        {
            ResponseData<List<Report>> model = new();

            try
            {
                var dataa =  await _reportDbContext.Reports.ToListAsync();
                model.Data = dataa;

            }
            catch (Exception ex)
            {
                _logger.LogError($"error => {ex.Message}");
                model.ErrorMessage = $"{ex.Message}";
            }
            return model;
                 
        }

        public async Task<ResponseData<ReportDetail>> GetReportDetail(long Id)
        {
            ResponseData<ReportDetail> model = new();

            try
            {
                var Result = await _reportDbContext.ReportDetails
                                        .FirstOrDefaultAsync(x => x.Id == Id);
                model.Data = Result;
            }
            catch (Exception ex)
            {

                _logger.LogError($"error => {ex.Message}");
                model.ErrorMessage = $"{ex.Message}";
            }
            return model;
        }

    }
}
