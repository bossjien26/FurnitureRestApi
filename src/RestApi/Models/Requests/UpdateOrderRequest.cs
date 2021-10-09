namespace RestApi.Models.Requests
{
    public class UpdateOrderRequest
    {
        public int OrderId { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string Recipient { get; set; }

        public string RecipientMail { get; set; }

        public string RecipientPhone { get; set; }

        public string Sender { get; set; }
    }
}