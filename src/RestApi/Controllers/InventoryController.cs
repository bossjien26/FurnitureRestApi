using System.Linq;
using System.Threading.Tasks;
using DbEntity;
using Entities;
using Enum;
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
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        private readonly ILogger<InventoryController> _logger;

        public InventoryController(DbContextEntity context, ILogger<InventoryController> logger)
        {
            _inventoryService = new InventoryService(context);
            _logger = logger;
        }

        [Route("show/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetInventory(int id) => Ok(await _inventoryService.GetById(id));

        [Route("{perPage}")]
        [HttpGet]
        public IActionResult ShowMany(int perPage) => Ok(_inventoryService.GetMany(perPage, 10).ToList());

        [Authorize(RoleEnum.SuperAdmin, RoleEnum.Admin, RoleEnum.Staff)]
        [Route("")]
        [HttpPost]
        public async Task<IActionResult> Insert(CreateInventoryRequest request)
        {
            var inventory = await InsertInventory(request);

            return CreatedAtAction(nameof(inventory), new { id = inventory.Id }, new AutResultModel()
            {
                Status = true,
                Data = "Success"
            });
        }

        private async Task<Inventory> InsertInventory(CreateInventoryRequest request)
        {
            var inventory = new Inventory()
            {
                ProductId = request.ProductId,
                Price = request.Price,
                IsDisplay = request.IsDisplay,
                RelateAt = request.RelateAt,
                SKU = request.SKU,
                Quantity = request.Quantity
            };
            await _inventoryService.Insert(inventory);

            return inventory;
        }

        [Authorize(RoleEnum.SuperAdmin, RoleEnum.Admin, RoleEnum.Staff)]
        [Route("")]
        [HttpPut]
        public async Task<IActionResult> Update(UpdateInventoryRequest request)
        {
            var inventory = await _inventoryService.GetById(request.Id);
            if (inventory == null)
            {
                return NotFound();
            }

            UpdateInventory(inventory, request);

            return CreatedAtAction(nameof(inventory), new { id = inventory.Id }, new AutResultModel()
            {
                Status = true,
                Data = "Success"
            });
        }

        private void UpdateInventory(Inventory inventory, UpdateInventoryRequest request)
        {
            inventory.ProductId = request.ProductId;
            inventory.Price = request.Price;
            inventory.IsDisplay = request.IsDisplay;
            inventory.RelateAt = request.RelateAt;
            inventory.SKU = request.SKU;
            inventory.Quantity = request.Quantity;
            _inventoryService.Update(inventory);
        }

        [Authorize(RoleEnum.SuperAdmin, RoleEnum.Admin)]
        [Route("{id}")]
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var inventory = await _inventoryService.GetById(id);
            if (inventory == null)
            {
                return NotFound();
            }
            _inventoryService.Delete(inventory);
            return NoContent();
        }
    }
}