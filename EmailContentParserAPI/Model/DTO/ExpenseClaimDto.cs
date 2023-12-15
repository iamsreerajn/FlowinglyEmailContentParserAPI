namespace EmailContentParserAPI.Model.DTO
{
    public class ExpenseClaimDto
    {
        public int Id { get; set; }
        public string ExpenseName { get; set; }
        public string CostCenter { get; set; }
        public double Total { get; set; }
        public string PaymentMethod { get; set; }
        public double ProductPrice { get; set; }
        public double SalesTax { get; set; }

    }
}
