using Manage.Application.Interface;
using Manage.Application.Services;
using Manage.Core.Entities;
using Manage.Core.Repository;
using Manage.Core.Repository.Base;
using Manage.Infrastructure.Data;
using Manage.Infrastructure.Repository;
using Manage.Infrastructure.Repository.Base;
using Manage.WebApi.Interface;
using Manage.WebApi.Services;
using Manage.WebApi.Utilities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manage.WebApi
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
            services.AddAutoMapper(typeof(Startup));
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //manage.core
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IEmployeePersonalDetailsRepository, EmployeePersonalDetailsRepository>();
            services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            services.AddScoped<ILeaveRepository, LeaveRepository>();
            services.AddScoped<IEmployeeLeaveRepository, EmployeeLeaveRepository>();
            services.AddScoped<IAdministrationRepository, AdministrationRepository>();



            //manage.application
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IEmployeePersonalDetailsService, EmployeePersonalDetailsService>();
            services.AddScoped<ILeaveService, LeaveService>();
            services.AddScoped<IEmployeeLeaveService, EmployeeLeaveService>();
            services.AddScoped<IAdministrationService, AdministrationService>();


            //manage.webapi
            services.AddScoped<IEmployeePageService, EmployeePageService>();
            services.AddScoped<IDepartmentPageService, DepartmentPageService>();
            services.AddScoped<IEmployeePersonalDetailsPageService, EmployeePersonalDetailsPageService>();
            services.AddScoped<ILeavePageService, LeavePageService>();
            services.AddScoped<IEmployeeLeavePageService, EmployeeLeavePageService>();
            services.AddScoped<IAdministrationPageService, AdministrationPageService>();
            services.AddScoped<IUploadImageHelper, UploadImageHelper>();
            services.AddScoped<IFileUploadHelper, FileUploadHelper>();

            services.AddControllersWithViews();

            services.AddDbContextPool<ManageContext>(options => options
                .UseSqlServer(Configuration.GetConnectionString("ManageConnection"),
                x => x.MigrationsAssembly("Manage.WebApi")));

            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                //password setting
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequireNonAlphanumeric = false;

                //signin requirenments
                options.SignIn.RequireConfirmedAccount = true;

            })

          .AddRoles<ApplicationRole>()
          .AddEntityFrameworkStores<ManageContext>()
          .AddDefaultTokenProviders();

            services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                }
                   
                    
            ); 



        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
