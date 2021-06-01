using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using TongXinBack.Service;
using TongXinBack.Config;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using System;
using System.Collections.Generic;
using IdentityCode;
using RedisTemplet;
namespace TongXinBack
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


            //redis缓存
            var section = Configuration.GetSection("Redis:Default");
            //连接字符串
            string _connectionString = section.GetSection("Connection").Value;
            //实例名称
            string _instanceName = section.GetSection("InstanceName").Value;
       
            //默认数据库 
            int _defaultDB = int.Parse(section.GetSection("DefaultDB").Value ?? "0");
            services.AddSingleton(new RedisHelper(_connectionString, _instanceName, _defaultDB));

            // requires using Microsoft.Extensions.Options
            services.Configure<UserAdminDatabaseSettingsImpl>(
                Configuration.GetSection(nameof(UserAdminDatabaseSettings)));
            //添加jwt验证：
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,//是否验证Issuer
                        ValidateAudience = true,//是否验证Audience
                        ValidateLifetime = true,//是否验证失效时间
                        ClockSkew = TimeSpan.FromSeconds(30),
                        ValidateIssuerSigningKey = true,//是否验证SecurityKey
                        ValidAudience = JWTSettings.Domain,//Audience
                        ValidIssuer = JWTSettings.Domain,//Issuer，这两项和前面签发jwt的设置一致
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTSettings.SecurityKey))//拿到SecurityKey
                    };
                });


            services.AddSingleton<UserAdminDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<UserAdminDatabaseSettingsImpl>>().Value);

            services.AddSingleton<BookService>();
            services.AddSingleton<FollowedServiceImpl>();
            services.AddSingleton<FollowingServiceImpl>();
            services.AddSingleton<PostServiceImpl>();
            services.AddSingleton<FollowServiceImpl>();
            services.AddSingleton<CommentServiceImpl>();
            services.AddSingleton<SendMailServiceImpl>();
            services.AddSingleton<IdentityCodeServiceImpl>();

            services.AddScoped<UserService, UserServiceImpl>();
            services.AddScoped<FollowedService, FollowedServiceImpl>();
            services.AddScoped<FollowingService, FollowingServiceImpl>();
            services.AddScoped<PostService, PostServiceImpl>();
            services.AddScoped<CommentService, CommentServiceImpl>();
            services.AddScoped<FollowService, FollowServiceImpl>();
            services.AddScoped<SendMailService, SendMailServiceImpl>();
            services.AddScoped<IdentityCodeService, IdentityCodeServiceImpl>();
            services.AddTransient<SendMailService, SendMailServiceImpl>();
            services.AddTransient<IdentityCodeService, IdentityCodeServiceImpl>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Demo", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "bearer",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new List<string>()
                    }
                });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();


            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            // 添加Swagger有关中间件
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Demo v1");
            });

            app.UseHttpsRedirection();
            app.UseMvc();
        }

    }
}
