using Microsoft.AspNetCore.Mvc;
using RustdeskServerAPI.Models;

namespace RustdeskServerAPI.Controllers
{
    [ApiController]
    [Route("api/[Action]")]
    public class AbController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        ApiContext _context;

        public AbController(ILogger<UsersController> logger, ApiContext db)
        {
            _logger = logger;
            _context = db;
        }


        [HttpGet(Name = "index")]
        public string Ab()
        {
            var headers = HttpContext.Request.Headers.Authorization.ToString();
            return "";
        }

        [HttpPost(Name = "index")]
        public string Ab(string id)
        {
            var headers = HttpContext.Request.Headers.Authorization.ToString();
            return "";
        }
    }
}
