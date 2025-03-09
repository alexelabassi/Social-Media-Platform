using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialMediaPlatform.Server.Dtos.Story;
using SocialMediaPlatform.Server.Migrations;
using SocialMediaPlatform.Server.Models;
using SocialMediaPlatform.Server.Repository;

namespace SocialMediaPlatform.Server.Controllers;

[ApiController]
public class StoryController : ControllerBase
{
    private readonly StoryRepository _storyRepo;
    private readonly UserManager<ApplicationUser> _userManager;
    
    public StoryController(StoryRepository storyRepo, UserManager<ApplicationUser> userManager, FollowRepository followRepo)
    {
        _storyRepo = storyRepo;
        _userManager = userManager;
    }

    [HttpPost("story/create")]
    [Authorize]
    public IActionResult CreateStory([FromBody] CreateStoryDto storyDto)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return Unauthorized(userId);
        }

        var story = new Story { UserId = userId, MediaUrl = storyDto.MediaUrl, CreatedAt = DateTime.UtcNow };
        _storyRepo.CreateStory(story);
        return Ok(story);
    }
    
    [HttpDelete("story/delete/{storyId}")]
    [Authorize]
    public IActionResult DeleteStory([FromRoute] int storyId)
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return Unauthorized(userId);
        }

        var story = _storyRepo.GetStoryById(storyId);
        if (story == null)
        {
            return NotFound();
        }

        if (story.UserId != userId)
        {
            return Unauthorized("This is not your story!");
        }

        _storyRepo.DeleteStory(story);
        return Ok();
    }

    [HttpGet("story/get/user/{username}")]
    [Authorize]
    public IActionResult GetStoriesForUser([FromRoute] string username)
    {
        var stories = _storyRepo.GetStoriesByUsername(username);
        if (stories == null || !stories.Any())
        {
            return NotFound();
        }

        return Ok(stories);
    }
    [HttpGet("story/get/following")]
    [Authorize]
    public IActionResult GetStoriesForFollowing()
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return Unauthorized(userId);
        }

        var stories = _storyRepo.GetStoriesForFollowing(userId);
        if (!stories.Any())
        {
            return NotFound();
        }

        return Ok(stories);
    }
}