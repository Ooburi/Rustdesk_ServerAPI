using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RustdeskServerAPI.Models;
using System.Text;
using System.Security.Cryptography;

namespace RustdeskServerAPI.Controllers
{
    [ApiController]
    [Route("api/[Action]")]
    public class UsersController : ControllerBase
    {
       

        private readonly ILogger<UsersController> _logger;
        ApiContext _context;

        public UsersController(ILogger<UsersController> logger, ApiContext db)
        {
            _logger = logger;
            _context = db;
        }

        
        [HttpPost(Name = "currentUser")]
        public async Task<UserPayload> CurrentUser()
        {
            string headers = HttpContext.Request.Headers.Authorization;

            var user = new UserPayload()
            {
                isAdmin = false,
                Email = "",
                Name = "",
                Note = "",
                Status = -1
            };

            var _user = await _context.Users.FirstOrDefaultAsync(u => u.Token == headers.Substring(7));
            if (_user != null)
            {
                user.Email = _user.Email;
                user.isAdmin = _user.isAdmin;
                user.Name = _user.Name;
                user.Status = _user.Status;
                user.Note = _user.Note;
            }
            return user;
        }
        //[HttpGet(Name = "options")]
        //[Route("~/api/login-options")]
        //public string LOptions()
        //{
        //    return "";
        //}

        [HttpPost(Name = "login")]
        public async Task<LoginResponse> Login([FromBody] LoginRequest request)
        {
            string token = Guid.NewGuid().ToString();
            var response = new LoginResponse()
            {
                access_token = "",
                tfa_type = "",
                type = "access_token",
                User = new UserPayload()
                {
                    Status = -1,
                    Email = "",
                    isAdmin = false,
                    Name = "",
                    Note = "",                    
                }
            };

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == request.Username);

            if (user != null)
            {
                if (user.Password == string.Concat(new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(request.Password)).Select(b => b.ToString("X2"))))
                {
                    user.Status = 1;
                    user.UserId = request.Id;
                    user.Uuid = request.Uuid;
                    user.Token = token;
                    await _context.SaveChangesAsync();

                    response.User.Status = 1;
                    response.User.Email = user.Email;
                    response.User.isAdmin = user.isAdmin;
                    response.User.Name = user.Name;
                    response.User.Note = user.Note;
                    response.access_token = token;
                }
            }

            return response;
        }


        [HttpPost(Name = "logout")]
        public async Task<IActionResult> Logout([FromBody] LoginRequest request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == request.Id);
            if(user != null)
            {
                user.Status = -1;
                user.Token = "";
            }
            await _context.SaveChangesAsync();
            return Ok();
        }


    }
}
