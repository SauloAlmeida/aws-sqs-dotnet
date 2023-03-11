using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using AWSSQSDotnet.DTO;
using System.Net;

namespace AWSSQSDotnet.Service
{
    public interface IQueueService
    {
        Task PublishAsync(string message);
        Task<IEnumerable<MessageOuput>> ConsumeAsync(CancellationToken ct);
        Task DeleteMessageAsync(MessageOuput message);
    }

    public class QueueService : IQueueService
    {
        private readonly AmazonSQSClient SQSClient;
        private readonly string QUEUE_URL;

        public QueueService()
        {
            SQSClient = new AmazonSQSClient(Environment.GetEnvironmentVariable("QUEUE_ACCESS_KEY"),
                                            Environment.GetEnvironmentVariable("QUEUE_SECRET_KEY"),
                                            RegionEndpoint.USEast2);

            QUEUE_URL = GetQueueUrl();
        }

        private string GetQueueUrl()
        {
            string queueName = Environment.GetEnvironmentVariable("QUEUE_NAME");

            return SQSClient.GetQueueUrlAsync(queueName).GetAwaiter().GetResult().QueueUrl;
        }

        public async Task<IEnumerable<MessageOuput>> ConsumeAsync(CancellationToken ct)
        {
            try
            {
                var response = await SQSClient.ReceiveMessageAsync(QUEUE_URL, ct);

                if (response?.Messages?.Any() is false) return Enumerable.Empty<MessageOuput>();

                return response.Messages.Select(s => new MessageOuput()
                {
                    Id = s.MessageId,
                    ReceiptId = s.ReceiptHandle,
                    Content = s.Body
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Enumerable.Empty<MessageOuput>();
            }
        }

        public async Task PublishAsync(string message)
        {
            var request = new SendMessageRequest
            {
                QueueUrl = QUEUE_URL,
                MessageBody = message
            };

            var response = await SQSClient.SendMessageAsync(request);

            if (response.HttpStatusCode is HttpStatusCode.OK) return;

            throw new Exception("It was not possible to send message to the queue.");
        }

        public async Task DeleteMessageAsync(MessageOuput message) => await SQSClient.DeleteMessageAsync(QUEUE_URL, message.ReceiptId);
    }
}
