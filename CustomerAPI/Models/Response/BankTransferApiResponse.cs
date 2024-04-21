namespace CustomerAPI.Models.Response
{
    public class BankTransferApiResponse
    {
        public string TransactionRef { get; set; }
        public string AccountNumber { get; set; }
        public string ApiResponse { get; set; }
        public string Status { get; set; }
    }
}
