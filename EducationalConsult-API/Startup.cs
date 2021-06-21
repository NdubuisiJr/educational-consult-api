using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using EducationalConsultAPI.Utils;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using System;
using EducationalConsultAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using EducationalConsultAPI.DBContext;
using System.Reflection;
using System.IO;
using EducationalConsultAPI.Models;
using EducationalConsultAPI.Repositories;
using Microsoft.Extensions.FileProviders;
using static EducationalConsultAPI.Utils.Constants;

namespace EducationalConsultAPI {
    public class Startup {
        private string _connectionString;

        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            //Adds cors default policy
            services.AddCors(options => {
                options.AddDefaultPolicy(builder => {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            //Add file upload service
            services.AddSingleton<IFileProvider>(new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"))
            );

            //Add the use of controllers and views. Chain NewtonsoftJon and xml serializers
            services.AddControllersWithViews(setupAction => {
                setupAction.Filters.Add(new ProducesResponseTypeAttribute(
                            StatusCodes.Status500InternalServerError));
                setupAction.Filters.Add(new ProducesResponseTypeAttribute(
                            StatusCodes.Status400BadRequest));
                setupAction.Filters.Add(new ProducesResponseTypeAttribute(
                            StatusCodes.Status406NotAcceptable));
                setupAction.Filters.Add(new ProducesResponseTypeAttribute(
                            StatusCodes.Status401Unauthorized));
                setupAction.Filters.Add(new ProducesAttribute(
                            APPLICATION_JSON, new string[] { APPLICATION_XML }));
                setupAction.Filters.Add(new ConsumesAttribute(
                            APPLICATION_JSON, new string[] { APPLICATION_XML, MULTIPART_FORMDATA }));

                setupAction.ReturnHttpNotAcceptable = true;

            })
            .AddNewtonsoftJson(setupAction => {
                setupAction.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();
            })
            .AddXmlSerializerFormatters()
            .AddXmlDataContractSerializerFormatters()
            .ConfigureApiBehaviorOptions(setupAction => {
                setupAction.InvalidModelStateResponseFactory = context => {
                    var problemDetails = new ValidationProblemDetails(context.ModelState) {
                        Type = VALIDATION_ERROR_URL,
                        Title = VALIDATION_ERROR_MESSAGE,
                        Status = StatusCodes.Status422UnprocessableEntity,
                        Instance = context.HttpContext.Request.Path
                    };
                    problemDetails.Extensions.Add(TRACE_ID, context.HttpContext.TraceIdentifier);
                    return new UnprocessableEntityObjectResult(problemDetails) {
                        ContentTypes = { APPLICATION_PROBLEM_JSON }
                    };
                };
            });

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection(SECRET);
            services.Configure<Secret>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<Secret>();
            var key = Encoding.ASCII.GetBytes(appSettings.SigningKey);
            services.AddAuthentication(authOptions => {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwtBearerOptions => {
                jwtBearerOptions.RequireHttpsMetadata = false;
                jwtBearerOptions.SaveToken = true;

                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters() {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true
                };
            });

            //Add automapper services
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //Add SecurityService
            services.AddScoped<ISecurityService, SecurityService>();

            //Add repository services
            _connectionString = BuildConnectionString();
            services.AddDbContext<EducationalDbContext>(options => {
                options.UseNpgsql(_connectionString);
                Console.WriteLine($"Using DB={_connectionString}");
            });
            services.AddScoped<IRepository<User>, UserRepository>();
            services.AddScoped<IRepository<Group>, GroupRepository>();
            services.AddScoped<IRepository<School>, SchoolRepository>();
            services.AddScoped<IRepository<Class>, ClassRepository>();
            services.AddScoped<IRepository<Student>, StudentRepository>();
            services.AddScoped<IJoinRepository<UserGroup>, UserGroupRepository>();
            services.AddScoped<ICommunication, Communication>();

            //Add swagger setup
            services.AddSwaggerGen(setupAction => {
                setupAction.SwaggerDoc(DOCS_NAME,
                    new Microsoft.OpenApi.Models.OpenApiInfo() {
                        Title = DOCS_VERSION,
                        Version = DOCS_VERSION,
                        Description = DOCS_DESCRIPTION,
                        Contact = new Microsoft.OpenApi.Models.OpenApiContact {
                            Email = EMAIL,
                            Name = NAME,
                            Url = new Uri(GITHUB)
                        }
                    });
                var xmlDocFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var path = Path.Combine(AppContext.BaseDirectory, xmlDocFile);
                setupAction.IncludeXmlComments(path);
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            app.UseExceptionHandler(appBuilder => {
                appBuilder.Run(async context => {
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsync(EXCEPTION_MESSAGE);
                });
            });

            app.UseRouting();

            app.UseCors();

            app.UseAuthentication();

            app.UseAuthorization();

            if (env.IsDevelopment()) {
                app.UseSwagger();
                app.UseSwaggerUI(setupAction => {
                    setupAction.SwaggerEndpoint(SWAGGER_JSON_PATH, DOCS_NAME);
                    setupAction.RoutePrefix = "api/docs";
                });
            }

            app.UseStaticFiles();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }

        private string BuildConnectionString() {
            var host = Environment.GetEnvironmentVariable("HOST") ?? "localhost";
            var userId = Environment.GetEnvironmentVariable("USER_ID") ?? "postgres";
            var userPassword = Environment.GetEnvironmentVariable("USER_PASSWORD") ?? "test";
            var database = "educationalconsultDB";
            var connection = $"User ID={userId};Password={userPassword};Server={host};Port=5432;Database={database};Integrated Security=true;Pooling=true;";
            return connection;
        }
    }
}
