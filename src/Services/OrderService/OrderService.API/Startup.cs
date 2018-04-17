using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
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
using OrderService.API.Infrastructure;
using OrderService.API.Infrastructure.Filters;
//using OrderService.API.Messaging;
using OrderService.API.Providers;
using OrderService.API.Settings;
using RabbitMQ.Client;

namespace OrderService.API
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
                checks.AddSqlCheck("OrdersDb", Configuration["ConnectionString"], TimeSpan.FromMinutes(minutes));
            });

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            }).AddControllersAsServices();

            services.AddDbContext<OrderContext>(options =>
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
                    Title = "ContactBookOnContainers - OrderService HTTP API",
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

            services.AddTransient<IOrdersService, OrdersService>();
            services.AddTransient<ICustomerService, CustomerService>();
            services.AddTransient<IProductService, ProductService>();
            //services.AddTransient<IMessageQ, MessageQ>();
            //services.Configure<MqSettings>(Configuration.GetSection("MqSettings"));

            var container = new ContainerBuilder();
            container.Register( c =>
            {
                return Bus.Factory.CreateUsingRabbitMq(sbc =>
                {
                    sbc.Host("rabbitmq", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    sbc.ExchangeType = ExchangeType.Fanout;
                });
            })
            .As<IBusControl>()
            .As<IBus>()
            .As<IPublishEndpoint>()
            .SingleInstance();

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
                  c.SwaggerEndpoint("/swagger/v1/swagger.json", "OrderService.API V1");
              });

            var bus = ApplicationContainer.Resolve<IBusControl>();
            var busHandle = TaskUtil.Await(() => bus.StartAsync());
            lifetime.ApplicationStopping.Register(() => busHandle.Stop());
        }
    }
}
