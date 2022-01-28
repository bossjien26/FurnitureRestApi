using System;
using System.Text;
using System.Threading.Tasks;
using DbEntity;
using Enum;
using Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Middlewares.Authentication;
using RestApi.Models.Requests;
using RestApi.src.Models;
using Services;
using Services.Interface;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;

        private readonly MailHelper _mailHelper;

        private readonly IOrderService _service;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrderController(DbContextEntity context, ILogger<OrderController> logger
        , IHttpContextAccessor httpContextAccessor, MailHelper mailHelper)
        {
            _service = new OrderService(context);
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _mailHelper = mailHelper;
        }

        [Authorize()]
        [HttpGet]
        [Route("{perPage}")]
        public IActionResult GetUserOrderMany(int perPage)
        {
            var userJWT = (JwtToken)_httpContextAccessor.HttpContext.Items["httpContextUser"];
            return Ok(_service.GetUserOrderMany(Convert.ToInt32(userJWT.Id), perPage, 10));
        }

        [Authorize()]
        [HttpGet]
        [Route("show/{id}")]
        public async Task<IActionResult> GetUserOrder(int id)
        {
            var userJWT = (JwtToken)_httpContextAccessor.HttpContext.Items["httpContextUser"];
            return Ok(await _service.GetUserOrder(id, Convert.ToInt32(userJWT.Id)));
        }

        [Authorize()]
        [HttpPost]
        [Route("")]
        public async Task<IActionResult> Insert(CreateOrderRequest request)
        {
            var userJWT = (JwtToken)_httpContextAccessor.HttpContext.Items["httpContextUser"];
            var order = await InsertOrder(request, Convert.ToInt32(userJWT.Id));
            SendMail(order);
            return Created(userJWT.Id, order);
        }

        private async Task<Entities.Order> InsertOrder(CreateOrderRequest request, int userId)
        {
            var order = new Entities.Order()
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

        private void SendMail(Entities.Order order)
        {
            try
            {
                _mailHelper.SendMail(new Mailer()
                {
                    MailTo = order.RecipientMail,
                    NameTo = order.Recipient,
                    Subject = MailTitle(order.Id),
                    Content = MailContent(order)
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }

        private string MailTitle(int orderId)
        {
            var mailTitle = new StringBuilder();
            mailTitle.AppendFormat("<h2>訂單編號 {0} <h2>", orderId);
            return mailTitle.ToString();
        }

        private string MailContent(Entities.Order order)
        {
            var mailContent = new StringBuilder();
            mailContent.Append("<h2>ＸＸＸ購物網站<h2>");
            mailContent.AppendFormat("<h3>訂單編號：{0} </h3>", order.Id);
            mailContent.AppendFormat("<h3>收款人：{0} </h3>", order.Recipient);
            mailContent.AppendFormat("<a href='/{0}'>查詢詳細資訊</a>", order.Id);
            return mailContent.ToString();
        }

        [Authorize(RoleEnum.SuperAdmin, RoleEnum.Admin, RoleEnum.Staff)]
        [HttpPut]
        [Route("")]
        public async Task<IActionResult> Update(UpdateOrderRequest request)
        {
            var userJWT = (JwtToken)_httpContextAccessor.HttpContext.Items["httpContextUser"];
            var order = await _service.GetUserOrder(request.OrderId, Convert.ToInt32(userJWT.Id));
            if (order == null)
            {
                return NotFound();
            }
            UpdateOrder(order, request);
            return Ok();
        }

        private void UpdateOrder(Entities.Order order, UpdateOrderRequest request)
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