using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PulseLogicHMR
{
    /*
       Author: Krishna, 
       Creation: 19thMay'23
       Purpose: 1. Make Startup Clean and Readable
                2. To Achieve Separation of Concerns
                3. Startup class was getting bulky, which leads to less  maintainable code.
    */
    public static class ConfigureServices
    {
        public static IServiceCollection AddSwaggerAndJwtServices(this IServiceCollection services, IConfigurationRoot Configuration)
        {
            // Add Swagger
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "PulseLogicHMR APIs",
                    Version = "v1",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                    {
                        Name = "PulseLogicHMR",
                        Email = "info.pulselogic-emr.com",
                        Url = new Uri("https://pulselogichealth.com/")
                    },
                    Description = "We are testing Swagger in PulseLogicHMR",
                    TermsOfService = new Uri("https://pulselogichealth.com/")
                });
                config.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Please enter your JWT token here starting with Bearer followed by single white space",
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                config.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                //PulseLogicEmrAPI.xml file is created by the Project>build event
                //and it is read by swagger to show in the api documentation.
                //We can show: <summary>, <remarks>, <response>
                var filePath = Path.Combine(System.AppContext.BaseDirectory, "PulseLogicEmrAPI.xml");
                config.IncludeXmlComments(filePath);

            });


            //Add JWT
            // S1 SECURITY FIX: Read JWT secret from environment variable first; fall back to config only for dev.
            // In production, set environment variable: ASPNETCORE_JWT_KEY=<256-bit-random-key>
            string jwtKey = Environment.GetEnvironmentVariable("ASPNETCORE_JWT_KEY");
            bool usingEnvKey = !string.IsNullOrWhiteSpace(jwtKey);
            if (!usingEnvKey)
            {
                // Fallback to config — log a warning so this is never silently used in production.
                jwtKey = Configuration["JwtTokenConfig:JwtKey"];
                // We cannot use ILogger here (outside DI), so we write directly.
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[SECURITY WARNING] ASPNETCORE_JWT_KEY environment variable is not set. " +
                                  "Falling back to appsettings.json JwtKey. DO NOT use the appsettings key in production.");
                Console.ResetColor();
            }

            string jwtIssuer   = Configuration["JwtTokenConfig:JwtIssuer"];
            string jwtAudience = Configuration["JwtTokenConfig:JwtAudience"];

            // S4 FIX: Determine if we are in a development environment to relax some checks.
            bool isDev = string.Equals(Configuration["environment:isdevelopment"], "true",
                                       StringComparison.OrdinalIgnoreCase);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme             = JwtBearerDefaults.AuthenticationScheme;
            })
            // Adding Jwt Bearer
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                // S11 FIX: Require HTTPS metadata in production; allow HTTP only in dev.
                options.RequireHttpsMetadata = !isDev;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    // The signing key must match!
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                    // S4 FIX: Enable issuer and audience validation (were false — accepted ANY JWT).
                    ValidateIssuer   = true,
                    ValidateAudience = true,
                    ValidAudience    = jwtAudience,
                    ValidIssuer      = jwtIssuer,
                    // Ensure tokens are not accepted after expiry
                    ValidateLifetime       = true,
                    ClockSkew              = TimeSpan.FromSeconds(30)
                };
            });
            return services;
        }
    }
}
