namespace SocialMediaPlatform.Server.Dtos.Story;

public class StoryWithUserDto
{
    public int Id { get; set; }
    public string UserId { get; set; } 
    public string MediaUrl { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string Username { get; set; } = string.Empty;
}