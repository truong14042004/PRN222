using Microsoft.AspNetCore.SignalR;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;
using StudentManagementApp.Hubs;

namespace StudentManagementApp.Services;

public class SignalREnrollmentNotificationPublisher : IEnrollmentNotificationPublisher
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public SignalREnrollmentNotificationPublisher(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public Task PublishEnrollmentConfirmedAsync(EnrollmentConfirmedNotification notification) =>
        _hubContext.Clients
            .Group(NotificationGroupNames.Student(notification.StudentId))
            .SendAsync("EnrollmentConfirmed", notification);
}
