using Microsoft.Extensions.Logging;

namespace EventProcessorService.Processors
{
	internal class ApplicationProcessor : IApplicationProcessor
	{
		private readonly ILogger<ApplicationProcessor> _logger;

		public ApplicationProcessor(ILogger<ApplicationProcessor> logger)
        {
			_logger = logger;
		}
        public Task Process(string body)
		{
			_logger.LogInformation("Event body has been processed: {body}", body);
			return Task.CompletedTask;
		}
	}
}
