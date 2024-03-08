namespace EventProcessorService.Processors
{
	public interface IApplicationProcessor
	{
		Task Process(string body);
	}
}
