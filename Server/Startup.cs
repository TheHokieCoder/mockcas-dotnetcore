namespace Server
{
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Caching.Memory;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Services;
	using System;


	public class Startup
	{
		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseMvc();
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMemoryCache();
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

			IServiceProvider serviceProvider = services.BuildServiceProvider();

			IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
			IUsersService usersService = new UsersService(configuration);
			services.AddSingleton(usersService);

			IMemoryCache memoryCache = serviceProvider.GetRequiredService<IMemoryCache>();
			ITicketService ticketService = new MemoryCacheTicketService(memoryCache);
			services.AddSingleton(ticketService);
		}

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}
	}
}
