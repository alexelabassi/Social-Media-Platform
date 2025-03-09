namespace SocialMediaPlatform.Server.Models;

public class Story
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public ApplicationUser User { get; set; }
    public string MediaUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}