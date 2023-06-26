using Newtonsoft.Json;

namespace TransactionService.Models
{
    public class Balance
    {
        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("date")]
        public DateTime? Date { get; set; }
    }
}
