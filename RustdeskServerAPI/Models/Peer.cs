using System.ComponentModel.DataAnnotations;

namespace RustdeskServerAPI.Models
{
    public class Peer
    {        
        [Key]
        public int PeerId { get; set; }
        public string? id { get; set; }
        public string? hash { get; set; }
        public string? username { get; set; }
        public string? hostname { get; set; }
        public string? platform { get; set; }
        public string? alias { get; set; }
        public string[]? tags { get; set; }
        public bool? forceAlwaysRelay { get; set; }
        public string? rdpPort { get; set; }
        public string? rdpUsername { get; set; }
        public bool? online { get; set; } = true;
        public string? loginName { get; set; }
    }
}
