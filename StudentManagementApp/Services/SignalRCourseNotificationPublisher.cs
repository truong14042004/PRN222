using Microsoft.AspNetCore.SignalR;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;
using StudentManagementApp.Hubs;

namespace StudentManagementApp.Services;

public class SignalRCourseNotificationPublisher : ICourseNotificationPublisher
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public SignalRCourseNotificationPublisher(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public Task PublishCourseChangedAsync(CourseChangedNotification notification) =>
        _hubContext.Clients
            .All
            .SendAsync("CourseChanged", notification);
}
