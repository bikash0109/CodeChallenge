using challenge.Data;
using challenge.Repositories;
using challenge.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace code_challenge
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
            services.AddDbContext<EmployeeContext>(options =>
            {
                options.UseInMemoryDatabase("EmployeeDB");
            });
            services.AddDbContext<CompensationContext>(options => {
                options.UseInMemoryDatabase("CompensationDB");
            });
            services.AddScoped<IEmployeeRepository,EmployeeRespository>();
            services.AddScoped<ICompensationRepository, CompensationRespository>();
            services.AddTransient<EmployeeDataSeeder>();
            services.AddTransient<CompensationDataSeeder>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddScoped<ICompensationService, CompensationService>();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, EmployeeDataSeeder seeder, CompensationDataSeeder compensationDataSeeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                seeder.Seed().Wait();
                compensationDataSeeder.Seed().Wait();
            }

            app.UseMvc();
        }
    }
}
