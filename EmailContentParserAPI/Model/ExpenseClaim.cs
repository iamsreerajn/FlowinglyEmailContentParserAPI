using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EmailContentParserAPI.Model
{
    public class ExpenseClaim
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ExpenseName { get; set; }
        public string CostCenter { get; set; }
        public double Total { get; set; }
        public string PaymentMethod { get; set; }
        public double ProductPrice { get; set; }
        public double SalesTax { get; set; }
        public DateTime CreatedDate{ get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
