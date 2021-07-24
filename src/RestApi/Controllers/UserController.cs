using DbEntity;
using Microsoft.AspNetCore.Mvc;
using src.Entities;
using System.Linq;
using src.Services.Service;
using src.Services.IService;
using ResApi.src.Models.Response;

namespace ResApi.src.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserInfoService _repository;
        private DbContextEntity _context;

        public UserController(DbContextEntity context)
        {
            _repository = new UserInfoService(context);
            _context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(_repository.GetAllUser().ToList());
        }

        [HttpGet]
        [Route("InsertUser")]
        public IActionResult InsertUser()
        {
            var user = new User()
            {
                Mail = "bbbbb"
            };
            _repository.Insert(user);
            return Ok(user);
        }


        //TODO:Category will return null
        [HttpPut]
        [Route("UpdateUser")]
        public IActionResult UpdateUser(User user)
        {
            if(_repository.FindUser(user.Id).Result == null)
            {
                return Ok(new RegistrationResponse()
                {
                    Status = false,
                    Data = "Not Find"
                });
            }
            
            _repository.Update(user);
            return Ok(new RegistrationResponse()
            {
                Status = true,
                Data = "Update"
            });
        }
    }
}