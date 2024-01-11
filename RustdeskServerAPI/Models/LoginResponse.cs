namespace RustdeskServerAPI.Models
{
    public class LoginResponse
    {
        public string? access_token { get; set; }
        public string type { get; set; }
        public string tfa_type { get; set; }
        public UserPayload User { get; set; }
    }
}
