using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RustdeskServerAPI.Models;
using System.Text;

namespace RustdeskServerAPI.Controllers
{

    public class ResponseData
    {
        public string Licensed_devices { get; set; } = "999";
        public string Data { get; set; }
    }
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
    public class MyPeer
    {
        public string id { get; set; }
        public string hash { get; set; }
        public string username { get; set; }
        public string hostname { get; set; }
        public string platform { get; set; }
        public string alias { get; set; }
        public string[] tags { get; set; }
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
        private readonly ILogger<UsersController> _logger;
        ApiContext _context;

        public AbController(ILogger<UsersController> logger, ApiContext db)
        {
            _logger = logger;
            _context = db;
        }

        public class ConvertedData
        {
            public string? Id { get; set; }
            public string? UserId { get; set; }
            public string[]? peers { get; set; }
            public string[]? tags { get; set; }
            public string Tag_colors { get; set; }
            public string? Access_token { get; set; }
        }
        [HttpGet(Name = "index2")]
        public async Task<object> Ab()
        {
            //answer myAnswer = new()
            //{
            //    data = JsonConvert.SerializeObject(new AnswerData()
            //    {
            //        tag_colors = "",
            //        tags = new string[] { "HUI", "HUYA", "HUE" },
            //        peers = new MyPeer[]
            //        {
            //                new()
            //                {
            //                    id = "6738295",
            //                    hash = "1poxeYro1uz29YqYdSu4iXmN+3keQG7OpIo3QNu\\/RuI=",
            //                    username = "wasp",
            //                    hostname = "lis.cbzeya.ru",
            //                    platform = "linux",
            //                    alias = "LIS",
            //                    tags = new string[]{""}
            //                }
            //        }
            //    })
            //};
            answer myAnswer = new();
            AnswerData myData = new();

            //string result = "";
            //ResponseData rawResult = new();

            string headers = HttpContext.Request.Headers.Authorization.ToString().Substring(7);
            ////string headers = "97660119-5fe0-437c-b758-ddaa4cc8de15";
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

            //if (user != null)
            //{
            //    var userPeers = await _context.UserPeers.Include(p=>p.Peers).FirstOrDefaultAsync(u => u.UserId == user.Id);
            //    if(userPeers != null)
            //    {
            //        ParsedData pd = new()
            //        {
            //            Id = user.UserId,
            //            UserId = user.UserId,
            //            Peers = userPeers.Peers.ToArray(),
            //            Tags = userPeers.Tags,
            //            Tag_colors = userPeers.TagColors,
            //            Access_token = headers
            //        };

            //        ConvertedData cd = new()
            //        {
            //            Id = user.UserId,
            //            UserId = user.UserId,
            //            peers = new string[pd.Peers.Length],
            //            tags = userPeers.Tags,
            //            Tag_colors = userPeers.TagColors,
            //            Access_token = headers
            //        };
            //        for(int i = 0; i < pd.Peers.Length; i++)
            //        {
            //            cd.peers[i] = JsonConvert.SerializeObject(pd.Peers[i]);
            //        }
            //        rawResult.Data = JsonConvert.SerializeObject(cd);
            //        result = JsonConvert.SerializeObject(rawResult);
            //    }
            //}


            //return result;
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
                var userPeers = await _context.UserPeers.FirstOrDefaultAsync(u => u.UserId == user.Id);
                try
                {
                    if (userPeers != null)
                    {
                        _context.UserPeers.Remove(userPeers);
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
                        try
                        {
                            await _context.UserPeers.AddAsync(userPeers);
                        }
                        catch (Exception e)
                        {
                            int i = 0;
                        }

                        await _context.SaveChangesAsync();
                    }
                }
                catch(Exception e)
                {
                    int y = 0;
                }
                
            }

            return Ok();
        }
    }
}
