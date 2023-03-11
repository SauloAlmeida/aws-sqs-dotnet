using AWSSQSDotnet.DTO;

namespace AWSSQSDotnet.Service
{
    public class ConsumerBackgroundService : BackgroundService
    {
        private readonly IQueueService _queue;

        public ConsumerBackgroundService(IQueueService queue) => _queue = queue;

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var messages = await _queue.ConsumeAsync(stoppingToken);

                    MesssageHandler(messages);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }

                await Task.Delay(2000);
            }
        }

        private void MesssageHandler(IEnumerable<MessageOuput> messages)
        {
            foreach (var item in messages)
            {
                Console.WriteLine(item.Content);

                _queue.DeleteMessageAsync(item);
            }
        }
    }
}
