using System.Threading.Tasks;
using DbEntity;
using Entities;
using Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Middlewares.Authentication;
using RestApi.Models.Requests;
using Services;
using Services.Interface;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;

        private readonly IOrderService _service;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderController(DbContextEntity context, ILogger<OrderController> logger
        , IHttpContextAccessor httpContextAccessor)
        {
            _service = new OrderService(context);
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize(RoleEnum.SuperAdmin, RoleEnum.Admin, RoleEnum.Staff, RoleEnum.Customer)]
        [HttpGet]
        [Route("{perPage}")]
        public IActionResult GetUserOrderMany(int perPage)
        {
            var user = (User)_httpContextAccessor.HttpContext.Items["User"];
            return Ok(_service.GetUserOrderMany(user.Id, perPage, 5));
        }

        [Authorize(RoleEnum.SuperAdmin, RoleEnum.Admin, RoleEnum.Staff, RoleEnum.Customer)]
        [HttpGet]
        [Route("show/{id}")]
        public async Task<IActionResult> GetUserOrder(int id)
        {
            var user = (User)_httpContextAccessor.HttpContext.Items["User"];
            return Ok(await _service.GetUserOrder(id, user.Id));
        }

        [Authorize(RoleEnum.SuperAdmin, RoleEnum.Admin, RoleEnum.Staff)]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Insert(CreateOrderRequest request)
        {
            var user = (User)_httpContextAccessor.HttpContext.Items["User"];
            await InsertOrder(request, user.Id);
            return Created(user.Id.ToString(), request);
        }

        private async Task<Order> InsertOrder(CreateOrderRequest request, int userId)
        {
            var order = new Order()
            {
                Street = request.Street,
                City = request.City,
                Country = request.Country,
                Recipient = request.Recipient,
                RecipientMail = request.RecipientMail,
                Sender = request.Sender,
                UserId = userId,
                RecipientPhone = request.RecipientPhone
            };

            await _service.Insert(order);
            return order;
        }

        [Authorize(RoleEnum.SuperAdmin, RoleEnum.Admin, RoleEnum.Staff, RoleEnum.Customer)]
        [HttpPut]
        [Route("")]
        public async Task<IActionResult> Update(UpdateOrderRequest request)
        {
            var user = (User)_httpContextAccessor.HttpContext.Items["User"];
            var order = await _service.GetUserOrder(request.OrderId, user.Id);
            if (order == null)
            {
                return NotFound();
            }
            UpdateOrder(order, request);
            return Ok();
        }

        private void UpdateOrder(Order order, UpdateOrderRequest request)
        {
            order.City = request.City;
            order.Country = request.Country;
            order.Recipient = request.Recipient;
            order.RecipientMail = request.RecipientMail;
            order.RecipientPhone = request.RecipientPhone;
            order.Street = request.Street;
            order.Country = request.Country;
            order.Sender = request.Sender;

            _service.Update(order);
        }
    }
}