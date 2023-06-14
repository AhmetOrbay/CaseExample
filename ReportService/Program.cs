
using Microsoft.AspNetCore.Authentication.JwtBearer;
using ReportLibrary.Interfaces;
using ReportLibrary.Models;
using ReportLibrary.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog.Events;
using Serilog;
using Serilog.AspNetCore;
using System.Text;
using Serilog.Sinks.Elasticsearch;
using ReportLibrary.Extensions;
using Elasticsearch.Net;
using Microsoft.AspNetCore.Connections;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using ReportLibrary.Model.RabbitMqModel;
using RabbitMQ.Client;
using Microsoft.Extensions.DependencyInjection;

namespace ReportService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var CONFIG = builder.Configuration;
            var elasticsearchUrl = CONFIG["ElasticsearchSettings:Url"];
            builder.Host.UseSerilog();//(context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            Log.Logger = new LoggerConfiguration()
                     .Enrich.FromLogContext()
                     .Enrich.WithMachineName()
                     .Enrich.WithEnvironmentUserName()
                    .WriteTo.File("Log.txt")
                     .WriteTo.Logger(lc => lc
                         .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
                         .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(CONFIG["ElasticsearchSettings:Url"]))
                         {
                             AutoRegisterTemplate = true,
                             IndexFormat = "error-{0:yyyy.MM.dd}",
                         })
                         .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
                         .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(CONFIG["ElasticsearchSettings:Url"]))
                         {
                             AutoRegisterTemplate = true,
                             IndexFormat = "Information-{0:yyyy.MM.dd}"
                         })
                     )
                     .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(CONFIG["ElasticsearchSettings:Url"]))
                     {
                         AutoRegisterTemplate = true,
                         IndexFormat = "log-{0:yyyy.MM.dd}"
                     })

                     .CreateLogger();


            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(option =>
            {
                option.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

            });
            builder.Services.Configure<ElasticSettings>(builder.Configuration.GetSection("ElasticsearchSettings"));

            builder.Services.AddDbContext<ReportDbContext>(config =>
            {
                config.UseNpgsql(@"User ID=postgres;Password=qwe789asd;Server=localhost;Port=5432;Database=ReportDb;Integrated Security=true;Pooling=true;");
                config.EnableSensitiveDataLogging();
            });
            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

            #region JWT

            #endregion

            #region RabbitMq
                var rabbitMQConfig = builder.Configuration.GetSection("RabbitMQSettings").Get<RabbitMQSettingsModel>();

                // RabbitMQ bağlantısını oluşturun
                var connectionFactory = new ConnectionFactory
                {
                    HostName = rabbitMQConfig.HostName,
                    Port = rabbitMQConfig.Port,
                    UserName = rabbitMQConfig.UserName,
                    Password = rabbitMQConfig.Password
                };
                var connection = connectionFactory.CreateConnection();

                // RabbitMQ kanalını oluşturun
                var channel = connection.CreateModel();

                // RabbitMQ servisini DI konteynerine ekleyin
                builder.Services.AddSingleton<IConnection>(connection);
            builder.Services.AddSingleton<IModel>(channel);
            #endregion
            builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JwtIssuer"],
                    ValidAudience = builder.Configuration["JwtAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSecurityKey2"]))
                };
            });
            builder.Services.AddSingleton<JwtHandler>(provider => new JwtHandler(builder.Configuration["Jwt:JwtSecurityKey2"]));

            builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));


            builder.Services.AddTransient<IReportService, ReportLibrary.Services.ReportService>();
            builder.Services.AddScoped<ReportDbContext>();



            var app = builder.Build();
            app.UseSerilogRequestLogging();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(x =>
                {
                    // x.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
                    //x.RoutePrefix = string.Empty;
                });
            }

            app.UseHttpsRedirection();


            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}