namespace CustomerPortal.Models.Request
{
    public class BankTransferRequest
    {
        public string TransactionReference { get; set; }
        public string SenderName { get; set; }
        public string SenderAccountNumber { get; set; }
        public string PayeeAccountNumber { get; set; }
        public string PayeeName { get; set; }
        public string Narration {  get; set; }
        public string Platform { get; set; }
        public string Amount { get; set; }
    }
}
