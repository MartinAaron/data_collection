using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace data.collection.identity
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddScoped<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();
            services.AddScoped<IProfileService, ProfileServices>();
            var builder = services.AddIdentityServer()
                .AddInMemoryClients(Conf.GetClients())
                .AddInMemoryIdentityResources(Conf.GetIdentityResourceResources())
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                .AddInMemoryApiResources(Conf.GetApiResources())
                .AddInMemoryApiScopes(Conf.ApiScopes)
                .AddProfileService<ProfileServices>();

            builder.AddDeveloperSigningCredential();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context => { await context.Response.WriteAsync(" IdentityServer4 for OAuth2.0 "); });
            });
        }
    }
}