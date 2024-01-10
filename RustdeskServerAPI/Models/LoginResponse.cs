namespace RustdeskServerAPI.Models
{
    public class LoginResponse
    {
        string access_token { get; set; }
        string type { get; set; }
        string tfa_type { get; set; }
        UserPayload user { get; set; }
    }
}
