using StudentManagementApp.BLL.DTOs;

namespace StudentManagementApp.BLL.Services;

public interface ICourseNotificationPublisher
{
    Task PublishCourseChangedAsync(CourseChangedNotification notification);
}
