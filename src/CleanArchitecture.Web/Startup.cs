using System;
using System.Diagnostics;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.SharedKernel;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.Infrastructure.Data;
using CleanArchitecture.Infrastructure.DomainEvents;
using CleanArchitecture.Web.Api;
using CleanArchitecture.Web.ApiModels;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StructureMap;
using StructureMap.AutoMocking;

namespace CleanArchitecture.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            // Moved to Program.cs
            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase(Guid.NewGuid().ToString()));
            //options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddMvc();

            // Add StructureMap container
            var container = new Container();

            container.Configure(config =>
            {
                config.Scan(_ =>
                {
                    _.AssemblyContainingType(typeof(Startup)); // Web
                    _.AssemblyContainingType(typeof(BaseEntity)); // Core
                    _.Assembly("CleanArchitecture.Infrastructure"); // Infrastructure
                    _.WithDefaultConventions();
                    _.ConnectImplementationsToTypesClosing(typeof(IHandle<>));
                });

                // TODO: Add Registry Classes

                // TODO: Move to Infrastucture Registry
                config.For(typeof(IRepository<>)).Add(typeof(EfRepository<>));
                config.For<IGuestbookRepository>().Use<GuestbookRepository>();
                config.For<IMessageSender>().Use<EmailMessageSenderService>();

                //Populate the container using the service collection
                config.Populate(services);
            });

            return container.GetInstance<IServiceProvider>();
            services.AddTransient<IRepository<ToDoItem>, EfRepository<ToDoItem>>();
        }

        public void ConfigureTesting(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory)
        {
            this.Configure(app, env, loggerFactory);

            PopulateTestData(app);
        }

    private void PopulateTestData(IApplicationBuilder app)
    {
        var dbContext = app.ApplicationServices.GetService<AppDbContext>();

        // reset the database
        dbContext.Database.EnsureDeleted();

        dbContext.ToDoItems.Add(new ToDoItem()
        {
            Title = "Test Item 1",
            Description = "Test Description One"
        });
        dbContext.ToDoItems.Add(new ToDoItem()
        {
            Title = "Test Item 2",
            Description = "Test Description Two"
        });
        dbContext.SaveChanges();

        // add Guestbook test data; specify Guestbook ID for use in tests
        var guestbook = new Guestbook() { Name = "Test Guestbook", Id=1 };
        dbContext.Guestbooks.Add(guestbook);
        guestbook.Entries.Add(new GuestbookEntry()
        {
            EmailAddress = "test@test.com",
            Message = "Test message"
        });
        dbContext.SaveChanges();
    }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
