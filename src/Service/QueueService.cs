using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;
using Newtonsoft.Json;

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
            SQSClient = new AmazonSQSClient("",
                                            "",
                                            RegionEndpoint.USEast2);
        }

        public async Task Pub(string message)
        {
            var queueResponse = await SQSClient.GetQueueUrlAsync("AWS-SQS-DOTNET-STANDARD");

            var request = new SendMessageRequest
            {
                QueueUrl = queueResponse.QueueUrl,
                MessageBody = JsonConvert.SerializeObject(message)
            };

            var response = await SQSClient.SendMessageAsync(request);

            if (response.HttpStatusCode is not System.Net.HttpStatusCode.OK)
                throw new Exception("It was not possible to send message to the queue.");
        }
    }
}
