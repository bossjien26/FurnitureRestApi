namespace RestApi.Models.Requests
{
    public class CreateOrderProductListRequest
    {
        public int ProductId { get; set; }

        public string Specification { get; set; }

        public int Quality { get; set; }
    }
}