namespace AWSSQSDotnet.Service
{
    public interface IQueueService
    {
        void Pub(string message);
    }

    public class QueueService : IQueueService
    {
        public void Pub(string message)
        {
        }
    }
}
