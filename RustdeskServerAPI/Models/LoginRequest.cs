namespace RustdeskServerAPI.Models
{
    public class LoginRequest
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Id { get; set; }
        public string? Uuid { get; set; }
        public bool? AutoLogin { get; set; }
        public string? Type { get; set; }
        public string? VerificationCode { get; set; }
        public string? TfaCode { get; set; }
    }
}
