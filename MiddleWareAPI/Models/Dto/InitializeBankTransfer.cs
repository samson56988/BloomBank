namespace MiddleWareAPI.Models.Dto
{
    public class InitializeBankTransfer
    {
        public string TransactionReference { set; get; }
        public string SenderName { get; set; }
        public string SenderAccountNumber { get; set; }
        public string PayeeAccountNumber { get; set; }
        public string PayeeName { get; set; }
        public string Narration { get; set; }
        public string PlatForm { get; set; }
        public decimal Amount { get; set; }
    }
}
