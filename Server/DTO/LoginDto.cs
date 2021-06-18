namespace Server.Models
{
    /// Model podataka za logovanje korisnika.
    public class LoginDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}