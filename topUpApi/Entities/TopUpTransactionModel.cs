namespace topUpApi.Entities
{
    public class TopUpTransactionModel
    {
        public int Id { get; set; }
        public int BeneficiaryId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }         
        public int UserId { get; set; }// Add transaction date property
                                                      // Add other transaction properties as needed
    }


    public class TopUpOptions
    {
        public int amount { get; set; }
        public string Status { get; set; }
        // Add transaction date property
                                       // Add other transaction properties as needed
    }
}
