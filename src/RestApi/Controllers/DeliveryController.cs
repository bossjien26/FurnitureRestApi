using System.Collections.Generic;
using DbEntity;
using Enum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Middlewares;
using Middlewares.Authentication;
using RestApi.Models.Requests;
using Services;
using Services.Dto;
using Services.Interface;

namespace RestApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryService _service;

        private readonly ILogger<DeliveryController> _logger;

        public DeliveryController(DbContextEntity context, ILogger<DeliveryController> logger)
        {
            _service = new DeliveryService(context);

            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous()]
        public IActionResult ShowMany()
        {
            return Ok(_service.GetMany());
        }

        [Authorize(RoleEnum.SuperAdmin, RoleEnum.Admin, RoleEnum.Staff)]
        [HttpPut]
        [Route("")]
        public IActionResult Update(UpdateDeliveryRequest request)
        {
            var delivery = new Delivery()
            {
                Content = request.Content,
                Title = request.Title,
                Type = request.Type,
                Introduce = request.Introduce
            };
            _service.Update(delivery);
            return Ok();
        }
    }
}