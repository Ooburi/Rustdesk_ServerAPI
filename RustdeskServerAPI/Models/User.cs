namespace RustdeskServerAPI.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string Uuid { get; set; }
        public string Email { get; set; }
        public bool isAdmin { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        /// <summary>
        /// 1 - authorized
        /// -1 - unverified
        /// 0 - disabled
        /// </summary>
        public int Status { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }


    }
}
