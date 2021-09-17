using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using RestApi.Test.Repositories;

namespace RestApi.Test.Controllers
{
    public class BaseController : BaseRepositoryTest
    {
        private readonly string _token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJtYWlsIjoiamFuQGV4YW1wbGUuY29tIiwicGFzc3dvcmQiOiJhYWFhYWFhIiwibmJmIjoxNjMxNzk2NjUwLCJleHAiOjE2MzI0MDE0NTAsImlhdCI6MTYzMTc5NjY1MH0.EByJ_a9FbVlcXbHmmWNdAXCeqqEKQda1tpvk9_6CGco";

        private readonly WebApplicationFactory<Startup> _factory;

        protected HttpClient _httpClient;

        public BaseController()
        {
            _factory = new WebApplicationFactory<Startup>();
            _httpClient = _factory.WithWebHostBuilder(builder =>
                { }).CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false,
                });

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }

        protected StringContent PostType(object request)
        {
            return new StringContent(JsonConvert.SerializeObject(request).ToString(),
            Encoding.UTF8, "application/json");
        }
    }
}