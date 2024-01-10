namespace RustdeskServerAPI.Models
{
    public class LoginRequest
    {
        string? username { get; set; }
        string? password { get; set; }
        string? id { get; set; }
        string? uuid { get; set; }
        bool? autoLogin { get; set; }
        string? type { get; set; }
        string? verificationCode { get; set; }
        string? tfaCode { get; set; }
    }
}
