﻿/*
 * Magic, Copyright(c) Thomas Hansen 2019 - 2020, thomas@servergardens.com, all rights reserved.
 * See the enclosed LICENSE file for details.
 */

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using magic.library;
using magic.lambda.logging.helpers;

namespace magic.backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            services.AddControllers().AddNewtonsoftJson();

            /*
             * Notice, necessary for magic.lambda.caching to function.
             */
            services.AddMemoryCache();

            /*
             * Initializing Magic.
             *
             * Notice, must be done AFTER you invoke "AddControllers".
             */
            services.AddMagic(Configuration, Configuration["magic:license"]);
        }

        public void Configure(IApplicationBuilder app)
        {
            /*
             * Initializing Magic.
             *
             * Notice, must be done BEFORE you invoke "UseEndpoints".
             */
            app.UseMagic(Configuration);

            app.UseHttpsRedirection();
            app.UseCors(x => x.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());
            app.UseAuthentication();
            app.UseRouting().UseEndpoints(conf => conf.MapControllers());

            // Creating a log entry for having started application.
            try
            {
                var logger = app.ApplicationServices.GetService(typeof(ILogger)) as ILogger;
                logger.Info("Magic was successfully started");
            }
            catch
            {
                // In case Magic has not yet been setup, the above will throw an exception.
            }
        }
    }
}
