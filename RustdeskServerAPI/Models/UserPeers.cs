namespace RustdeskServerAPI.Models
{
    public class UserPeers
    {
        public int? Id { get; set; }
        public int? UserId { get; set; }
        public List<Peer>? Peers { get; set; }
        public string[]? Tags { get; set; }
        public string TagColors { get; set; }
    }
}
