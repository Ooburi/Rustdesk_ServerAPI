namespace RustdeskServerAPI.Models
{
    public class UserPeers
    {
        ////data: "{"tags":["Polzovateli","Komputers"],
        //"peers":[{"id":"905905111","hash":"","username":"","hostname":"","platform":"","alias":"Dmitriy","tags":["Polzovateli"]},{"id":"900900900","hash":"","username":"","hostname":"","platform":"","alias":"Alexey","tags":["Polzovateli"]},{"id":"100000900","hash":"","username":"","hostname":"","platform":"","alias":"MyComputer","tags":["Komputers","Polzovateli"]}],
        //"tag_colors":"{\"Polzovateli\":4290721292}"}"
        public int? Id { get; set; }
        public int? UserId { get; set; }
        public List<Peer>? Peers { get; set; }
        public string[]? Tags { get; set; }
        public string TagColors { get; set; }
    }
}
