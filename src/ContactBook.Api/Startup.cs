using System;
using AutoMapper;
using ContactBook.Domain;
using ContactBook.Domain.AddContact;
using ContactBook.Infrastructure.Data.Query.Queries.GetContacts;
using ContactBook.Infrastructure.Data.Service.Resources.Cache;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ContactBook.Api {
    public class Startup {
        public Startup (IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            services.AddMvc (options => {
                    options.Filters.Add<ValidatorFilter> ();
                })
                .AddFluentValidation (fvc =>
                    fvc.RegisterValidatorsFromAssemblyContaining<GetContactsQueryValidator> ())
                .SetCompatibilityVersion (CompatibilityVersion.Version_2_1);

            // services.AddCors (c => {
            //     c.AddPolicy ("AllowOrigin", options => options.AllowAnyOrigin ());
            // });

            services.AddCors (options => {
                options.AddPolicy ("AllowOrigin",
                    builder => {
                        builder.AllowAnyOrigin ()
                            .AllowAnyHeader ()
                            .AllowAnyMethod ();
                    });
            });

            services.Configure<MvcOptions> (options => {
                options.Filters.Add (new CorsAuthorizationFilterFactory ("AllowOrigin"));
            });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen (options => {
                options.DescribeAllEnumsAsStrings ();
                options.SwaggerDoc ("v1", new Swashbuckle.AspNetCore.Swagger.Info {
                    Title = "ContactBook API",
                        Version = "v1",
                        Description = "",
                        TermsOfService = "Terms Of Service"
                });
            });
            var mappingConfig = new MapperConfiguration (mc => {
                mc.AddProfile (new DefaultProfile ());
            });

            IMapper mapper = mappingConfig.CreateMapper ();
            services.AddSingleton (mapper);
            services.AddMediatR (typeof (GetContactsQueryHandler).Assembly);
            services.AddMediatR (typeof (AddContactCommandHandler).Assembly);

            var redisCacheSettings = new RedisCacheSettings ();
            Configuration.GetSection (nameof (RedisCacheSettings)).Bind (redisCacheSettings);
            services.AddSingleton (redisCacheSettings);

            if (!redisCacheSettings.Enabled) {
                return;
            }

            var redisConnectionString = Environment.GetEnvironmentVariable ("REDIS_CONNECTION_STRING");
            if (string.IsNullOrEmpty (redisConnectionString))
                redisConnectionString = redisCacheSettings.ConnectionString;

            services.AddStackExchangeRedisCache (options => options.Configuration = redisConnectionString);
            services.AddSingleton<IResponseCacheService, ResponseCacheService> ();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            } else {
                app.UseHsts ();
            }
            app.UseCors ();
            app.UseDefaultFiles ();
            app.UseStaticFiles ();

            app.UseHttpsRedirection ();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger ();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI (c => {
                c.SwaggerEndpoint ("/swagger/v1/swagger.json", "My API V1");
            });

            app.UseMvc ();
        }

    }

    public class RedisCacheSettings {
        public bool Enabled { get; set; }

        public string ConnectionString { get; set; }
    }
}