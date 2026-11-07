namespace AgriStream.Web.Models
{
    public class ApplicationUser
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = "Farmer"; // Farmer, Agronomist, Admin
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}