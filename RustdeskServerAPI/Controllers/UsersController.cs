using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RustdeskServerAPI.Models;
using System.Text;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace RustdeskServerAPI.Controllers
{
    [ApiController]
    [Route("api/[Action]")]
    public class UsersController : ControllerBase
    {
        ApiContext _context;
        public UsersController(ApiContext db)
        {
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
        [HttpGet(Name = "peers")]
        public async Task<object> Peers()
        {
            string headers = HttpContext.Request.Headers.Authorization.ToString().Substring(7);
            var users = await _context.Users.ToListAsync();

            List<Peer> peers2 = new();

            var resp = new responsePeers()
            {
                total = 0,
                data = new List<peerForResponse>()
            };

            foreach (User user in users)
            {
                var userPeers = await _context.UserPeers.Include(p => p.Peers).FirstOrDefaultAsync(u => u.UserId == user.Id);

                List<Peer>? peers = userPeers?.Peers;
                if (peers != null)
                {
                    for (int i = 0; i < peers.Count; i++)
                    {
                        resp.total++;
                        resp.data.Add(new peerForResponse()
                        {
                            id = peers[i].id,
                            note = peers[i].alias,
                            status = 1,
                            user = user.Name,
                            user_name = user.Name,
                            info = new()
                            {
                                username = peers[i].username,
                                device_name = peers[i].hostname,
                                os = peers[i].platform
                            }
                        });
                    }
                }                
            }
            var result = JsonConvert.SerializeObject(resp);
            return resp;
        }
    
    [HttpGet(Name = "users")]
        public async Task<object> Users()
        {
            string headers = HttpContext.Request.Headers.Authorization.ToString().Substring(7);

            List<User> users = await _context.Users.ToListAsync();

            var resp = new responseUsers()
            {
                total = users.Count,
                data = new userForResponse[users.Count]
            };
            for (int i = 0; i < resp.data.Length; i++)
            {
                resp.data[i] = new userForResponse()
                {
                    name = users[i].Name,
                    email = users[i].Email,
                    is_admin = users[i].isAdmin,
                    note = users[i].Note,
                    status = users[i].Status
                };
            }
            var result = JsonConvert.SerializeObject(resp);
            return resp;
        }

        [HttpGet(Name = "reg")]
        public async Task<string> Reg(string login, string password)
        {
            try
            {
                User us = new User()
                {
                    UserId = "",
                    Name = login,
                    Email = "a@a.com",
                    isAdmin = true,
                    Login = login,
                    Password = string.Concat(new SHA1Managed().ComputeHash(Encoding.UTF8.GetBytes(password)).Select(b => b.ToString("X2"))),
                    Note = "user registered with api/reg",
                    Status = 0,
                    Uuid = "",
                    Token = ""
                };

                await _context.Users.AddAsync(us);
                await _context.SaveChangesAsync();
                return String.Format("Registered user {0}", login);
            } catch(Exception e)
            {
                return String.Format("Error: {0}", e.ToString());
            }
        }
    }
    
    public class userForResponse
    {
        public string name { get; set; }
        public string email { get; set; }
        public string note { get; set; }
        public int status { get; set; }
        public bool is_admin { get; set; }
    }
    public class peerForResponse
    {
        public string id { get; set; }
        public peerInfo info { get; set; }
        public string user { get; set; }
        public string user_name { get; set; }
        public int status { get; set; }
        public string note { get; set; }
    }
    public class peerInfo
    {
        public string username { get; set; }
        public string os { get; set; }
        public string device_name { get; set; }
    }
    public class responseUsers
    {
        public userForResponse[] data { get; set; }
        public int total { get; set; }
        //public string error { get; set; }
    }
    public class responsePeers
    {
        public List<peerForResponse> data { get; set; }
        public int total { get; set; }
        //public string error { get; set; }
    }
}
