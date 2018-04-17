using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using ContactService.API.ContactProviders;
using ContactService.API.Infrastructure;
using ContactService.API.Infrastructure.Filters;
using ContactService.API.Messaging.Consumer;
//using ContactService.API.Messaging;
using ContactService.API.Settings;
using MassTransit;
using MassTransit.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.HealthChecks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ContactService.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IContainer ApplicationContainer { get; private set; }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddHealthChecks(checks =>
            {
                var minutes = 1;
                if (int.TryParse(Configuration["HealthCheck:Timeout"], out var minutesParsed))
                {
                    minutes = minutesParsed;
                }
                checks.AddSqlCheck("ContactsDb", Configuration["ConnectionString"], TimeSpan.FromMinutes(minutes));
            });

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            }).AddControllersAsServices();

            services.AddDbContext<ContactBookContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionString"],
                                     sqlServerOptionsAction: sqlOptions =>
                                     {
                                         sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                                         //Configuring Connection Resiliency: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency 
                                         sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                                     });

                // Changing default behavior when client evaluation occurs to throw. 
                // Default in EF Core would be to log a warning when client evaluation is performed.
                options.ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));
                //Check Client vs. Server evaluation: https://docs.microsoft.com/en-us/ef/core/querying/client-eval
            });

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info
                {
                    Title = "ContactBookOnContainers - ContactService HTTP API",
                    Version = "v1",
                });
            });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            services.AddTransient<IContactsService, ContactsService>();
            //services.AddSingleton<IMessageQ, MessageQ>();

            //services.Configure<MqSettings>(Configuration.GetSection("MqSettings"));

            var container = new ContainerBuilder();

            container.RegisterType<CustomerUpdateEventConsumer>();

            container.Register(context =>
                {
                    var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
                    {
                        var host = cfg.Host(new Uri("rabbitmq://rabbitmq/"), h =>
                        {
                            h.Username("guest");
                            h.Password("guest");
                        });

                        cfg.ReceiveEndpoint(host, "customer_queue", e =>
                        {
                            e.LoadFrom(context);
                            //e.Consumer<CustomerUpdateEventConsumer>();
                        });
                    });

                    return busControl;
                })
                .SingleInstance()
                .As<IBusControl>()
                .As<IBus>();

            container.Populate(services);
            ApplicationContainer = container.Build();

            return new AutofacServiceProvider(ApplicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime lifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            app.UseMvcWithDefaultRoute();

            app.UseSwagger()
              .UseSwaggerUI(c =>
              {
                  c.SwaggerEndpoint("/swagger/v1/swagger.json", "ContactService.API V1");
              });

            var bus = ApplicationContainer.Resolve<IBusControl>();
            var busHandle = TaskUtil.Await(() => bus.StartAsync());
            lifetime.ApplicationStopping.Register(() => busHandle.Stop());
            //ConfigureEventBus(app);

        }


        //private void ConfigureEventBus(IApplicationBuilder app)
        //{
        //    var eventbus = app.ApplicationServices.GetRequiredService<IMessageQ>();
        //    var lifeTime = app.ApplicationServices.GetRequiredService<ILifetimeScope>();
            
        //    while (true)
        //    {
        //        eventbus.ConsumeMessage();
        //    }
        //}
    }
}
