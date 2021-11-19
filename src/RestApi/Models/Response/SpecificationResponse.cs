using System.Collections.Generic;

namespace RestApi.Models.Response
{
    public class SpecificationResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<SpecificationContentResponse> SpecificationContentResponses { get; set; }
            = new List<SpecificationContentResponse>();
    }
}