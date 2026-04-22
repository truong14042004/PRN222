using StudentManagementApp.BLL.DTOs;

namespace StudentManagementApp.BLL.Services;

public interface IEnrollmentNotificationPublisher
{
    Task PublishEnrollmentConfirmedAsync(EnrollmentConfirmedNotification notification);
}
