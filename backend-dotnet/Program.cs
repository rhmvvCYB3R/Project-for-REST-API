using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using REST_project.Controllers.Services;
using REST_project.Controllers.Services.Interface;
using Scalar.AspNetCore;
using System.Text;

namespace REST_project
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            // ✅ Настраиваем CORS с конкретным Origin (лучше, чем AllowAnyOrigin)
           builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
            "http://localhost:5500",
            "https://exchanger-06k9.onrender.com"  // продакшен фронтенд
        )
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});


            builder.Services.AddOpenApi();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddHttpClient<IExchangeRateService, ExchangeRateService>();

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Example: Bearer {token}"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    string key = builder.Configuration.GetRequiredSection("JwtSettings")["SecretKey"]
                        ?? throw new ArgumentNullException("JwtSettings:SecretKey not found in configuration");

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true
                    };
                });

            builder.Services.AddSingleton<IUserMockService, UserMockService>();

            var app = builder.Build();

            // ✅ ВКЛЮЧАЕМ CORS СРАЗУ после Build и до любого middleware
            app.UseCors("AllowFrontend");

            // Swagger работает и в production
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}

