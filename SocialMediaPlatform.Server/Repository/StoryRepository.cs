using SocialMediaPlatform.Server.Data;
using SocialMediaPlatform.Server.Dtos.Story;
using SocialMediaPlatform.Server.Models;

namespace SocialMediaPlatform.Server.Repository;

public class StoryRepository
{
    private readonly ApplicationDbContext _context;
    private readonly FollowRepository _followRepo;

    
    public StoryRepository(ApplicationDbContext context, FollowRepository followRepo)
    {
        _context = context;
        _followRepo = followRepo;
    }

    public void CreateStory(Story story)
    {
        _context.Stories.Add(story);
        _context.SaveChanges();
    }

    public Story? GetStoryById(int storyId)
    {
        return _context.Stories.FirstOrDefault(s => s.Id == storyId);
    }

    public void DeleteStory(Story story)
    {
        _context.Stories.Remove(story);
        _context.SaveChanges();
    }

    public List<StoryWithUserDto> GetStoriesByUsername(string username)
    {
        var stories = _context.Stories
            .Where(s => s.User.UserName == username).OrderByDescending(s => s.CreatedAt)
            .Select(s => new StoryWithUserDto
            {
                Id = s.Id,
                UserId = s.UserId,
                MediaUrl = s.MediaUrl,
                CreatedAt = s.CreatedAt,
                Username = s.User.UserName
            }).ToList();
        return stories;
    }
    public List<StoryWithUserDto> GetStoriesForFollowing(string followerId)
    {
        var followingList = _followRepo.GetFollowingByUser(followerId);
        var stories = _context.Stories
            .Where(s => followingList.Contains(s.User.UserName)).OrderByDescending(s => s.CreatedAt)
            .Select(s => new StoryWithUserDto
            {
                Id = s.Id,
                UserId = s.UserId,
                MediaUrl = s.MediaUrl,
                CreatedAt = s.CreatedAt,
                Username = s.User.UserName
            }).ToList();
        return stories;
    }
}