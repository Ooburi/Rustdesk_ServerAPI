namespace RustdeskServerAPI.Models
{
    public class UserPayload
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
        /// <summary>
        /// 1 - normal, -1 - unverified, 0 - disabled
        /// </summary>
        public int Status { get; set; }
        public bool isAdmin { get; set; }
    }
}
