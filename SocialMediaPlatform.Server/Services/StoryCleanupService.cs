using SocialMediaPlatform.Server.Data;

namespace SocialMediaPlatform.Server.Services;

public class StoryCleanupService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private const int DelayInMinutes = 10;
    
    public StoryCleanupService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var expiredStories = context.Stories.Where(s => s.CreatedAt < DateTime.UtcNow.AddMinutes(-1)).ToList();
                
                context.Stories.RemoveRange(expiredStories);
                await context.SaveChangesAsync();
            }
            await Task.Delay(TimeSpan.FromMinutes(DelayInMinutes), stoppingToken);
        }
    }
}