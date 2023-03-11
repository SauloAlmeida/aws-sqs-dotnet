using Newtonsoft.Json;

namespace AWSSQSDotnet.DTO
{
    public class ModelInput
    {
        public string Message { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
