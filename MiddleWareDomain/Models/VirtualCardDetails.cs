namespace MiddleWareDomain.Models
{
    public class VirtualCardDetails
    {
        public string CardId { get; set; }
        public string CustomerId { get; set; }
        public string CardName { get; set; }
        public string CardPAN { get; set; }
        public string CardType { get; set; }
        public string Cvv { get; set; }
        public string CardProvider { get; set; }
        public string CardExpiryDate { get; set; }
        public string CardPin { get; set; }
        public string CardStatus { get; set; }
        public bool IsActive {  get; set;  }
    }


    public class VirtualCardDetailsDto
    {
        public string CustomerId { get; set; }
        public string CardType { get; set; }
        public string CardProvider { get; set; }
    }
}
