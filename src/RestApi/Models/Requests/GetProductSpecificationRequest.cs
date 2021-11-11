namespace RestApi.Models.Requests
{
    public class GetProductSpecificationRequest
    {
        public int ProductId { get; set; }
        public int Offset { get; set; }

        public int[] PreviousSpecifications { get; set; }
    }
}