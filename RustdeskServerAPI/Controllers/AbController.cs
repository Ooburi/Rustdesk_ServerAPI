using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RustdeskServerAPI.Models;
using System.Text;

namespace RustdeskServerAPI.Controllers
{
    public class PostedData
    {
        public string data;
    }
    public class ParsedData
    {
        public string? Id { get; set; }
        public string? UserId { get; set; }
        public Peer[]? Peers { get; set; }
        public string[]? Tags { get; set; }
        public string Tag_colors { get; set; }
        public string? Access_token { get; set; }
    }
    
    public class AnswerData
    {
        public string[] tags { get; set; }
        public Peer[] peers { get; set; }
        public string tag_colors { get; set; }
    }
    public class answer
    {
        public string licensed_devices { get; set; } = "999";
        public string data { get; set; }
    }
    [ApiController]
    [Route("api/[Action]")]
    public class AbController : ControllerBase
    {
        ApiContext _context;

        public AbController(ApiContext db)
        {
            _context = db;
        }

        [HttpGet(Name = "index2")]
        public async Task<object> Ab()
        {
            answer myAnswer = new();
            AnswerData myData = new();

            string headers = HttpContext.Request.Headers.Authorization.ToString().Substring(7);
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Token == headers);

            if(user != null)
            {
                var userPeers = await _context.UserPeers.Include(p => p.Peers).FirstOrDefaultAsync(u => u.UserId == user.Id);
                if (userPeers != null)
                {
                    myData.tag_colors = userPeers.TagColors;
                    myData.peers = userPeers.Peers.ToArray();
                    myData.tags = userPeers.Tags;
                }                    
            }
            myAnswer.data = JsonConvert.SerializeObject(myData);

            var res = JsonConvert.SerializeObject(myAnswer);
            return res;
        }
        
        [HttpPost(Name = "index")]
        [Route("~/api/ab")]
        public async Task<IActionResult> AbPost([FromBody] object data)
        {
            string dataInJson = data.ToString();
            PostedData fromJson = JsonConvert.DeserializeObject<PostedData>(dataInJson);
            ParsedData parsed = JsonConvert.DeserializeObject<ParsedData>(fromJson.data);

            string headers = HttpContext.Request.Headers.Authorization.ToString().Substring(7);
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Token == headers);

            if (user != null)
            {
                var userPeers = await _context.UserPeers.Include(p=>p.Peers).FirstOrDefaultAsync(u => u.UserId == user.Id);
                try
                {
                    if (userPeers != null)
                    {
                        _context.UserPeers.Remove(userPeers);
                        await _context.SaveChangesAsync();

                        await _context.SaveChangesAsync();
                        userPeers.Peers = parsed.Peers.ToList();
                        userPeers.TagColors = parsed.Tag_colors;
                        userPeers.Tags = parsed.Tags;
                        await _context.UserPeers.AddAsync(userPeers);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        userPeers = new();
                        userPeers.UserId = user.Id;
                        userPeers.Peers = parsed.Peers.ToList();
                        userPeers.TagColors = parsed.Tag_colors;
                        userPeers.Tags = parsed.Tags;
                        
                        await _context.UserPeers.AddAsync(userPeers);
                        await _context.SaveChangesAsync();
                    }
                }
                catch(Exception e)
                {
                }
            }
            return Ok();
        }
    }
}
