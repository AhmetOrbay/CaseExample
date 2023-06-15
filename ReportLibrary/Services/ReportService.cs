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

        public Task<bool> CreatedReport(long HotelId)
        {
            try
            {
                if (HotelId > 0)
                {
                    var entity = _reportDbContext.Reports.Add(new Report
                    {
                        CreatedDateTime = DateTime.Now,
                        Status = ReportStatus.Waiting
                    });
                    if (entity.Entity.Id > 0)
                    {
                        var publishModel = new PublishModel()
                        {
                            HotelId = HotelId,
                            ReportId = entity.Entity.Id
                        };
                        var resultPublish = _publishService.PublishMessage(
                            JsonSerializer.Serialize(publishModel));
                        return Task.FromResult(true);
                    }
                }
                return Task.FromResult(false);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Publish Error => {ex.Message}");
                return Task.FromResult(false);
            }
        }

        public async Task<List<Report>> GetListReport()
        {
            try
            {
                return await _reportDbContext.Reports.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"error => {ex.Message}");
                return new List<Report>();
            }
        }

        public async Task<ReportDetail> GetReportDetail(long Id)
        {
            try
            {
                var Result = await _reportDbContext.ReportDetails
                                        .FirstOrDefaultAsync(x => x.Id == Id);
                return Result;
            }
            catch (Exception ex)
            {

                _logger.LogError($"error => {ex.Message}");
                return new ReportDetail();
            }
        }

        public async Task AddReportDetail(ReportDetail modelDetail)
        {
            try
            {
                var entity = _reportDbContext.ReportDetails.Add(modelDetail);
                if (entity.Entity.Id > 0)
                {
                    var report = _reportDbContext.Reports
                                    .FirstOrDefault(x => x.Id == modelDetail.ReportId);
                    if (report is not null)
                    {
                        report.Status = ReportStatus.Completed;
                        _reportDbContext.Entry(report).Property(x => x.Status).IsModified = true;
                    }
                    else
                    {

                        _logger.LogError($"not found ReportsId => {report.Id}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"error => {ex.Message}");
            }
        }
    }
}
