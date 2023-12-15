namespace EmailContentParserAPI.HelperUtilities
{
    public class ECPUtilities
    {
        public const string ROOT_KEY = "expense";
        public const string NODE_TO_BE_VALIDATED = "cost_center";
        public const double SalesTaxPercentage= 20;
        public const string Pattern= @"<[^>]+>.*";
        public const string ExpensePattern = @"<[^>]+>.*</[^>]+>";
        //public const string ReservationPattern = @"<[^>]+>.*?</[^>]+>";
        public const string ReservationPattern = @"<[^>]+>.*";
        public const string ExpenseNode = "expense";
        public const string ReservationNode = "vendor";
        public  enum PaymentMethods
        {
            Cash,
            Card,
            Loan
        }
    }
}
