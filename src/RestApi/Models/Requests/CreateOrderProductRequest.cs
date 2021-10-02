using System.Collections.Generic;

namespace RestApi.Models.Requests
{
    public class CreateOrderProductRequest
    {
        public int orderId { get; set; }

        public List<CreateOrderProductListRequest> productList { get; set; }
    }
}