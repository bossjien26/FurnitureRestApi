namespace RestApi.Models.Response
{
    public class CartListResponse
    {
        public int InventoryId { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }
    }
}