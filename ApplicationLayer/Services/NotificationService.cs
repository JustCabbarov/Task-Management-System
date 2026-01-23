using Contract.Services;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.AspNetCore.SignalR;
using Presentation.Hubs;

public class NotificationService : INotificationService
{
    private readonly INotificationRepository _repository;
    private readonly IHubContext<NotificationHub> _hub;

    public NotificationService(INotificationRepository repository, IHubContext<NotificationHub> hub)
    {
        _repository = repository;
        _hub = hub;
    }

    public async Task NotifyTaskAssignedAsync(string userId, string taskTitle)
    {
        var message = $"Yeni task: {taskTitle} təyin olundu";

        // DB-yə yaz
        await _repository.AddAsync(new Notification
        {
            UserId = userId,
            Message = message,
            IsRead = false,
            CreateAt = DateTime.UtcNow
        });

    
        await _hub.Clients.User(userId).SendAsync("ReceiveNotification", message);
    }

    public async Task NotifyMentionsAsync(Dictionary<string, string> userMessages)
    {
        if (userMessages == null || !userMessages.Any()) return;

        var notifications = userMessages
            .Select(kv => new Notification
            {
                UserId = kv.Key,
                Message = kv.Value,
                IsRead = false,
                CreateAt = DateTime.UtcNow
            })
            .ToList();

        await _repository.AddRangeAsync(notifications);
       
        var tasks = userMessages
            .Select(kv =>
            {
                var userId = kv.Key.ToLower();
                return _hub.Clients.User(userId).SendAsync("ReceiveNotification", kv.Value);
            })
            .ToList();

        await Task.WhenAll(tasks);
    }


    public async Task<List<Notification>> GetMyNotificationsAsync(string userId)
    {
        return await _repository.GetUserNotificationsAsync(userId);
    }

    public async Task MarkReadAsync(int id)
    {
        await _repository.MarkAsReadAsync(id);
    }
}
