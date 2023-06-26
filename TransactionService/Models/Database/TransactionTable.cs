using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TransactionService.Models.Database
{
    [Table("transactions")]
    public class TransactionTable
    {
        [Key]
        [Column("id")]
        public string Id { get; set; }

        [Column("amount")]
        public string Amount { get; set; }

        [Column("description")]
        public string Description { get; set; }

        [Column("date")]
        public string Date { get; set; }
    }
}
