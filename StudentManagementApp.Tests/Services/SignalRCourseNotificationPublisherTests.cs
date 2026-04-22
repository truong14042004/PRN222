using Microsoft.AspNetCore.SignalR;
using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.Hubs;
using StudentManagementApp.Services;
using Xunit;

namespace StudentManagementApp.Tests.Services;

public class SignalRCourseNotificationPublisherTests
{
    [Fact]
    public async Task PublishCourseChangedAsync_broadcasts_to_all_connected_clients()
    {
        var allProxy = new CapturingClientProxy();
        var clients = new FakeHubClients(allProxy);
        var hubContext = new FakeHubContext(clients);
        var publisher = new SignalRCourseNotificationPublisher(hubContext);
        var notification = new CourseChangedNotification
        {
            Action = "Updated",
            CourseId = 7,
            Title = "Khóa học đã cập nhật",
            Message = "Khóa học IELTS vừa được cập nhật.",
            OccurredAt = DateTime.Now
        };

        await publisher.PublishCourseChangedAsync(notification);

        Assert.Equal("CourseChanged", allProxy.MethodName);
        var payload = Assert.Single(allProxy.Arguments);
        Assert.Same(notification, payload);
    }

    private sealed class FakeHubContext : IHubContext<NotificationHub>
    {
        public FakeHubContext(IHubClients clients)
        {
            Clients = clients;
        }

        public IHubClients Clients { get; }

        public IGroupManager Groups { get; } = new FakeGroupManager();
    }

    private sealed class FakeHubClients : IHubClients
    {
        private readonly IClientProxy _all;

        public FakeHubClients(IClientProxy all)
        {
            _all = all;
        }

        public IClientProxy All => _all;

        public IClientProxy AllExcept(IReadOnlyList<string> excludedConnectionIds) => throw new NotImplementedException();

        public IClientProxy Client(string connectionId) => throw new NotImplementedException();

        public IClientProxy Clients(IReadOnlyList<string> connectionIds) => throw new NotImplementedException();

        public IClientProxy Group(string groupName) => throw new NotImplementedException();

        public IClientProxy GroupExcept(string groupName, IReadOnlyList<string> excludedConnectionIds) => throw new NotImplementedException();

        public IClientProxy Groups(IReadOnlyList<string> groupNames) => throw new NotImplementedException();

        public IClientProxy User(string userId) => throw new NotImplementedException();

        public IClientProxy Users(IReadOnlyList<string> userIds) => throw new NotImplementedException();
    }

    private sealed class FakeGroupManager : IGroupManager
    {
        public Task AddToGroupAsync(string connectionId, string groupName, CancellationToken cancellationToken = default) => Task.CompletedTask;

        public Task RemoveFromGroupAsync(string connectionId, string groupName, CancellationToken cancellationToken = default) => Task.CompletedTask;
    }

    private sealed class CapturingClientProxy : IClientProxy
    {
        public string? MethodName { get; private set; }

        public List<object?> Arguments { get; } = [];

        public Task SendCoreAsync(string method, object?[] args, CancellationToken cancellationToken = default)
        {
            MethodName = method;
            Arguments.Clear();
            Arguments.AddRange(args);
            return Task.CompletedTask;
        }
    }
}
