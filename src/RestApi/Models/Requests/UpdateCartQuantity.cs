namespace RestApi.Models.Requests
{
    public class UpdateCartQuantity
    {
        public int InventoryId { get; set; }

        public int Quantity { get; set; }
    }
}