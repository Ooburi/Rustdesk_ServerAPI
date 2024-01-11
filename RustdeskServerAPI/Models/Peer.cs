using System.ComponentModel.DataAnnotations;

namespace RustdeskServerAPI.Models
{
    public class Peer
    {
        [Key]
        public int PeerId { get; set; }
        public string Id { get; set; }
        public string Hash { get; set; }
        public string Username { get; set; }
        public string Hostname { get; set; }
        public string Platform { get; set; }
        public string Alias { get; set; }
        public string[] Tags { get; set; }
        public bool ForceAlwaysRelay { get; set; }
        public string RdpPort { get; set; }
        public string RdpUsername { get; set; }
        public bool Online { get; set; } = false;
        public string LoginName { get; set; }
    }
}
