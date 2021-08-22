namespace RestApi.Models.Requests
{
    public class Registration
    {
        public string Name { get; set; }

        public string Mail { get; set; }

        public string Password { get; set; }

        public string Country { get; set; }

        public string City { get; set; }

        public string Street { get; set; }
    }
}