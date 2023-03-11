using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace AWSSQSDotnet.Service
{
    public interface IQueueService
    {
        Task Pub(string message);
    }

    public class QueueService : IQueueService
    {
        private readonly AmazonSQSClient SQSClient;

        public QueueService()
        {
            SQSClient = new AmazonSQSClient(Environment.GetEnvironmentVariable("QUEUE_ACCESS_KEY"),
                                            Environment.GetEnvironmentVariable("QUEUE_SECRET_KEY"),
                                            RegionEndpoint.USEast2);
        }

        public async Task Pub(string message)
        {
            var queueResponse = await SQSClient.GetQueueUrlAsync("AWS-SQS-DOTNET-STANDARD");

            var request = new SendMessageRequest
            {
                QueueUrl = queueResponse.QueueUrl,
                MessageBody = message
            };

            var response = await SQSClient.SendMessageAsync(request);

            if (response.HttpStatusCode is not System.Net.HttpStatusCode.OK)
                throw new Exception("It was not possible to send message to the queue.");
        }
    }
}
