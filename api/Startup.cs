using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Fisher.Bookstore.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Fisher.Bookstore.Models;

namespace Fisher.Bookstore.Api
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
            services.AddDbContext<BookstoreContext>(options => options.UseNpgsql(Configuration.GetConnectionString("BookstoreContext")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
             .AddEntityFrameworkStores<BookstoreContext>()
             .AddDefaultTokenProviders();

            services.AddCors(options => {
                options.AddPolicy("CorsPolicy", builder =>{
                    builder.WithOrigins("http://localhost:4201")
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });

            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(JwtBearerOptions =>
             {
                 JwtBearerOptions.TokenValidationParameters = new TokenValidationParameters()
                 {
                     ValidateActor = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidIssuer = Configuration["JWTConfiguration:Issuer"],
                     ValidAudience = Configuration["JWTConfiguration:Audience"],
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWTConfiguration.Key"]))
                 };
             });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseAuthentication();

            app.UseHttpsRedirection();
            app.UseCors("CorsPolicy");
            app.UseMvc();
        }
    }
}