using Newtonsoft.Json;

namespace TransactionService.Models
{
    public class Transaction
    {
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("amount")]
        public double Amount { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("date")]
        public DateTime? Date { get; set; }
    }
}
