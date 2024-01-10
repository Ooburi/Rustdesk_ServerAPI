namespace RustdeskServerAPI.Models
{
    public class UserPayload
    {
        string Name { get; set; }
        string Email { get; set; }
        string Note { get; set; }
        /// <summary>
        /// 1 - normal, -1 - unverified, 0 - disabled
        /// </summary>
        byte Status { get; set; } 
        bool isAdmin { get; set; }
    }
}
