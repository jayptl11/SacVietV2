using Microsoft.EntityFrameworkCore;
using SacViet.Models;

namespace SacViet.Services
{
    public class ScheduledPublishService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ScheduledPublishService> _logger;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(1); // Check every minute

        public ScheduledPublishService(
            IServiceProvider serviceProvider,
            ILogger<ScheduledPublishService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Scheduled Publish Service started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await PublishScheduledArticlesAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while publishing scheduled articles");
                }

                await Task.Delay(_interval, stoppingToken);
            }

            _logger.LogInformation("Scheduled Publish Service stopped");
        }

        private async Task PublishScheduledArticlesAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<SacVietContext>();

            var now = DateTime.Now;

            // Find all approved articles with scheduled publish date that has arrived
            var articlesToPublish = await context.Articles
                .Where(a => a.Status == "Approved" 
                    && a.ScheduledPublishDate.HasValue 
                    && a.ScheduledPublishDate.Value <= now)
                .ToListAsync();

            if (articlesToPublish.Any())
            {
                _logger.LogInformation($"Publishing {articlesToPublish.Count} scheduled articles");

                foreach (var article in articlesToPublish)
                {
                    article.Status = "Published";
                    article.PublishedAt = DateTime.Now;
                    
                    _logger.LogInformation(
                        $"Published article {article.ArticleId}: {article.Title} " +
                        $"(scheduled for {article.ScheduledPublishDate?.ToString("dd/MM/yyyy HH:mm")})");
                }

                await context.SaveChangesAsync();
                
                _logger.LogInformation($"Successfully published {articlesToPublish.Count} scheduled articles");
            }
        }
    }
}