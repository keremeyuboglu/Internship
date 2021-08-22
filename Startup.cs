using Altamira.Data;
using Altamira.Data.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Reflection;
using System.Text;

namespace Altamira
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


            //var log = new LoggerConfiguration()
            //    .WriteTo.Elasticsearch(new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions
            //    (new Uri(Configuration["ElasticConfiguration:Uri"]))
            //    {
            //        IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower()}-{DateTime.UtcNow:yyyy-MM}"

            //    })
            //    .CreateLogger();
            //services.AddSingleton(log);
            Log.Logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(Configuration["ElasticConfiguration:Uri"]))
        {
            AutoRegisterTemplate = true,
            IndexFormat = $"{Assembly.GetExecutingAssembly().GetName().Name.ToLower()}-{DateTime.UtcNow:yyyy-MM-dd}" // index is called altamira-yy-mm-dd
        })
        .CreateLogger();




            services.AddDbContext<AltamiraContext>
               (opt => opt.UseSqlServer(Configuration["Data:SqlCon:ConnectionString"], x => x.UseNetTopologySuite())); // DBContext is added to connect to DB, plus NetTopology is used for Geo location
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["JwtToken:Issuer"],
                        ValidAudience = Configuration["JwtToken:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtToken:Key"]))
                    };
                });
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("Redis");
                options.InstanceName = "RedisDemo_Altamira";
            });
            services.AddScoped<IAltamiraRepo, AltamiraRepo>();
            services.AddAutoMapper(typeof(Startup));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Altamira", Version = "v1" });
                c.EnableAnnotations();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
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
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            /*
             * 
             * 
   
            string elasticSearchConnectionsStr = string.Empty;
            string connectionStringByEnvironmentVariables = string.Empty;
            elasticSearchConnectionsStr = Convert.ToString(Configuration["ConnectionStrings:ElasticSearchConnection"]);

            var log = new LoggerConfiguration()
                .Enrich.WithThreadId()
                .Enrich.WithThreadName()
                .Enrich.WithMachineName()
                .Enrich.WithEnvironmentUserName()
                .Enrich.WithExceptionDetails()
                .Enrich.FromLogContext()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticSearchConnectionsStr))
                {
                    ModifyConnectionSettings = x => x.BasicAuthentication(Configuration["Elastic:User"], Configuration["Elastic:Pass"]),
                    IndexDecider = (@event, offset) => Configuration["Elastic:Index"],
                    CustomFormatter = new ElasticsearchJsonFormatter()
                })
                .CreateLogger();
            Log.Information("CondoLife Starting...");
            services.AddSingleton(log);

             */

            loggerFactory.AddSerilog();
            if (env.IsDevelopment() || env.EnvironmentName == "Docker")
            {
                var data = System.IO.File.ReadAllText(@"seed.json");
                Seeder.Seed(data, app.ApplicationServices);
                // USING SWAGGER API
                app.UseDeveloperExceptionPage();
                app.UseSwagger().UseAuthentication();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Altamira v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
