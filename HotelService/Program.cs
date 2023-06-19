
using HotelLibrary.Extensions;
using HotelLibrary.Interfaces;
using HotelLibrary.Models;
using HotelLibrary.Repositories;
using HotelLibrary.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog.Events;
using Serilog;
using System.Text;
using Serilog.Sinks.Elasticsearch;
using RabbitMQ.Client;
using HotelLibrary.Model.RabbitMqModel;
using HotelLibrary.Services.RabbitMq;
using ReportLibrary.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json.Serialization;

namespace HotelService
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

            builder.Services.AddControllers().AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
            builder.Services.AddSingleton(Log.Logger);


            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(option =>
            {
                option.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

            });
            builder.Services.Configure<ElasticSettings>(builder.Configuration.GetSection("ElasticsearchSettings"));

            builder.Services.AddDbContextPool<HotelDbContext>(config =>
            {
                config.UseNpgsql(@"User ID=postgres;Password=qwe789asd;Server=localhost;Port=5432;Database=HotelDb;Integrated Security=true;Pooling=true;");


                config.EnableSensitiveDataLogging();
                
            });
            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));

            //#region JWT

            //    builder.Services.AddAuthentication(opt =>
            //    {
            //        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //    })
            //    .AddJwtBearer(options =>
            //    {
            //        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            //        {
            //            ValidateIssuer = true,
            //            ValidateAudience = true,
            //            ValidateLifetime = true,
            //            ValidateIssuerSigningKey = true,
            //            ValidIssuer = builder.Configuration["JwtIssuer"],
            //            ValidAudience = builder.Configuration["JwtAudience"],
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSecurityKey2"]))
            //        };
            //    });
            //    builder.Services.AddSingleton<JwtHandler>(provider => new JwtHandler(builder.Configuration["Jwt:JwtSecurityKey2"]));

            //#endregion


            #region RABBITMQ
                var rabbitMQConfig = builder.Configuration.GetSection("RabbitMQSettings").Get<RabbitMQSettingsModel>();

                builder.Services.AddSingleton<ConnectionFactory>(sp =>
                {
                    var factory = new ConnectionFactory()
                    {
                        HostName = rabbitMQConfig.HostName,
                        UserName = rabbitMQConfig.UserName,
                        Password = rabbitMQConfig.Password,
                        Port = rabbitMQConfig.Port
                    };
                    return factory;
                });

            builder.Services.AddSingleton<HotelDbContext>();
            builder.Services.AddSingleton<IHotelService, HotelServices>();
            builder.Services.AddHostedService<RabbitMQBackgroundService>();
            builder.Services.AddSingleton<PublishRabbitMQService>();
            //builder.Services.AddHostedService<RabbitMQBackgroundService>();


            #endregion
            builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));





            var app = builder.Build();

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentUserName()
                .WriteTo.Console()
                .WriteTo.File("error.txt", LogEventLevel.Error)
                .WriteTo.File("information.txt", LogEventLevel.Information)
                .MinimumLevel.Error()
                .MinimumLevel.Information()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticsearchUrl))
                {
                    AutoRegisterTemplate = true,
                    IndexFormat = "Error--Hotel-{0:yyyy.MM.dd}",
                    MinimumLogEventLevel = LogEventLevel.Error
                })
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticsearchUrl))
                {
                    AutoRegisterTemplate = true,
                    IndexFormat = "info-Hotel-{0:yyyy.MM.dd}",
                    MinimumLogEventLevel = LogEventLevel.Information
                })
                .CreateLogger();

            app.UseSerilogRequestLogging();

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
            //app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}