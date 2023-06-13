
using HotelLibrary.Extensions;
using HotelLibrary.Interfaces;
using HotelLibrary.Models;
using HotelLibrary.Repositories;
using HotelLibrary.Services;
using HotelService.Models;
using HotelService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace HotelService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(option =>
            {
                option.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

            });
            builder.Services.Configure<ElasticSettings>(builder.Configuration.GetSection("ElasticsearchSettings"));

            builder.Services.AddDbContext<HotelDbContext>(config =>
            {
                config.UseNpgsql(@"User ID=postgres;Password=qwe789asd;Server=localhost;Port=5432;Database=HotelDb;Integrated Security=true;Pooling=true;");
                config.EnableSensitiveDataLogging();
            });
            builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
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


            builder.Services.AddTransient<IHotelService, HotelServices>();
            builder.Services.AddScoped<HotelDbContext>();
            builder.Services.AddSingleton<ILogger,ElasticsearchLogger>();
            var app = builder.Build();

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