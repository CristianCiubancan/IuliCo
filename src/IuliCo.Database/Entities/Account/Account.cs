using Microsoft.EntityFrameworkCore;

namespace IuliCo.Database.Entities.Account
{
    public class Account
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Ip { get; set; }

        public Account()
        {
            Username = string.Empty; // Non-null default value
            Password = string.Empty; // Non-null default value
            Email = string.Empty; // Non-null default value
            Ip = string.Empty; // Non-null default value
        }
    }
}
