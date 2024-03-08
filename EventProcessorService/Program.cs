using EventProcessorService;
using EventProcessorService.Processors;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<EventProcessorClientService>();
// Adds an EventProcessorClient for use for the lifetime of the application.
builder.Services
		.AddSingleton(EventProcessor.CreateEventProcessorClient(builder.Configuration));

builder.Services.AddTransient<IApplicationProcessor, ApplicationProcessor>();


builder.Services
			  .Configure<HostOptions>(options =>
			  {
				  options.ShutdownTimeout = TimeSpan.FromSeconds(90);
			  });

var host = builder.Build();
host.Run();


