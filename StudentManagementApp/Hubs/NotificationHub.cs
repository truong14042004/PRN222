using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;

namespace StudentManagementApp.Hubs;

public class NotificationHub : Hub
{
    public override async Task OnConnectedAsync()
    {
        var session = Context.GetHttpContext()?.Session;
        var userRole = session?.GetString("UserRole");
        if (userRole == "Admin")
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, NotificationGroupNames.Admins);
        }

        if (userRole == "Student" &&
            int.TryParse(session?.GetString("UserId"), out var studentId))
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, NotificationGroupNames.Student(studentId));
        }

        await base.OnConnectedAsync();
    }
}
