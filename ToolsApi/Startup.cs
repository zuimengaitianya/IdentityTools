using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.AccessTokenValidation;

namespace ToolsApi
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
            services.AddControllers();

            //services.AddAuthentication("Bearer")
            //    //.AddIdentityServerAuthentication(option =>
            //    //{
            //    //    option.Authority = "http://localhost:5000";
            //    //    option.RequireHttpsMetadata = false;
            //    //    option.ApiName = "api1";
            //    //})
            //    .AddJwtBearer("Bearer", options =>
            //    {
            //        options.Authority = "http://localhost:5000";
            //        options.RequireHttpsMetadata = false;
            //        options.Audience = "api1";

            //        //options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };

            //        //// if token does not contain a dot, it is a reference token
            //        //options.ForwardDefaultSelector = Selector.ForwardReferenceToken("introspection");
            //    })
            //    //.AddOAuth2Introspection("introspection", options =>
            //    //{
            //    //    options.Authority = "http://localhost:5000";
            //    //    options.ClientId = "ToolsApiClient";
            //    //    options.ClientSecret = "secret";

            //    //    //options.api
            //    //})
            //    ;

            //services.AddAuthentication("Bearer")
            //    .AddIdentityServerAuthentication(option =>
            //    {
            //        option.Authority = "http://localhost:5000";
            //        option.ApiName = "api1";
            //        option.ApiSecret = "secret";

            //        //option.EnableCaching = true;
            //        //option.CacheDuration = TimeSpan.FromMinutes(10);//that's the default
            //    });

            services.AddAuthentication("token")
                .AddOAuth2Introspection("token", options =>
                {
                    options.Authority = "http://localhost:5000";
                    options.ClientId = "api1"; //使用ApiName 不是 ClientID
                    options.ClientSecret = "secret"; //使用ApiSecret 不是 ClientSecret
                });

            //配置cors。这将允许http://localhost:5002到http://localhost:5001进行Ajax跨域调用.
            services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy.WithOrigins("http://localhost:5002")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //跨域访问
            app.UseCors("default");

            app.UseAuthentication();

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
