using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using data.collection.api.Middlewares;
using data.collection.util.ApiClient;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;

namespace data.collection.api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "数据采集平台", Version = "v1"});
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "在下框中输入请求头中需要添加Jwt授权Token：Bearer Token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new string[] { }
                    }
                });
                // 为 Swagger JSON and UI设置xml文档注释路径
                var basePath =
                    Path.GetDirectoryName(typeof(Program).Assembly.Location); //获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                var xmlPath = Path.Combine(basePath, "data.collection.api.xml");
                c.IncludeXmlComments(xmlPath);
            });

            services.AddAuthentication("Bearer").AddJwtBearer("Bearer", option =>
            {
                // option.Authority = "http://dc.auth.crservice.cn";
                option.Authority = Configuration["AuthServer"];
                option.RequireHttpsMetadata = false;
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false
                };

                IdentityModelEventSource.ShowPII = true;
            });
            services.AddSingleton(typeof(ApiClient));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //跨域
            app.UseCors(x =>
                {
                    x.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .DisallowCredentials();
                })
                .UseMiddleware<RequestBodyMiddleware>()
                .UseMiddleware<RequestLogMiddleware>();

            // if (env.IsDevelopment())
            // {
            //     app.UseDeveloperExceptionPage();
            //     app.UseSwagger();
            //     app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "data.collection.api v1"));
            // }    

            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "data.collection.api v1"));

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}