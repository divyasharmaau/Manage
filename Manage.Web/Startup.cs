using Manage.Core.Entities;
using Manage.Infrastructure.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Manage.Core.Repository.Base;
using Manage.Infrastructure.Repository.Base;
using Manage.Core.Repository;
using Manage.Infrastructure.Repository;
using Manage.Application.Interface;
using Manage.Application.Services;
using Manage.Web.Interface;
using Manage.Web.Services;

namespace Manage.Web
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
          

            //manage.application
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<IDepartmentService, DepartmentService>();
            services.AddScoped<IEmployeePersonalDetailsService, EmployeePersonalDetailsService>();
            services.AddScoped<ILeaveService, LeaveService>();
            services.AddScoped<IEmployeeLeaveService, EmployeeLeaveService>();

            //manage.web
            services.AddScoped<IEmployeePageService, EmployeePageService>();
            services.AddScoped<IDepartmentPageService, DepartmentPageService>();
            services.AddScoped<IEmployeePersonalDetailsPageService, EmployeePersonalDetailsPageService>();
            services.AddScoped<ILeavePageService, LeavePageService>();
            services.AddScoped<IEmployeeLeavePageService, EmployeeLeavePageService>();

            services.AddControllersWithViews();

            services.AddDbContextPool<ManageContext>(options => options.UseSqlServer(Configuration.GetConnectionString("ManageConnection"), x => x.MigrationsAssembly("Manage.Web")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                //password setting
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequireNonAlphanumeric = false;

                //signin requirenments
                options.SignIn.RequireConfirmedAccount = true;
            })
                .AddRoles<IdentityRole>()
               .AddEntityFrameworkStores<ManageContext>()
               .AddDefaultTokenProviders();


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
