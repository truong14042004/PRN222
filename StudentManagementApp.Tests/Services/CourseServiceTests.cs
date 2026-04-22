using StudentManagementApp.BLL.DTOs;
using StudentManagementApp.BLL.Services;
using StudentManagementApp.DAL.Models;
using StudentManagementApp.DAL.Repositories;
using Xunit;

namespace StudentManagementApp.Tests.Services;

public class CourseServiceTests
{
    [Fact]
    public async Task CreateAsync_publishes_created_course_notification()
    {
        var repository = new FakeCourseRepository();
        var publisher = new CapturingCourseNotificationPublisher();
        var service = new CourseService(repository, publisher);

        await service.CreateAsync(new CreateCourseDto
        {
            Name = "IELTS Intensive",
            Level = "B2",
            Description = "Evening intensive course",
            TuitionFee = 2500000,
            ThumbnailUrl = "https://example.com/ielts.jpg"
        });

        var notification = Assert.Single(publisher.PublishedNotifications);
        Assert.Equal("Created", notification.Action);
        Assert.NotNull(notification.Course);
        Assert.Equal(notification.CourseId, notification.Course!.Id);
        Assert.Equal("IELTS Intensive", notification.Course.Name);
        Assert.True(notification.Course.IsActive);
    }

    [Fact]
    public async Task UpdateAsync_publishes_updated_course_notification()
    {
        var repository = new FakeCourseRepository(
            new Course
            {
                Id = 5,
                Name = "English Basics",
                Level = "A1",
                Description = "Starter",
                TuitionFee = 1500000,
                IsActive = true
            });
        var publisher = new CapturingCourseNotificationPublisher();
        var service = new CourseService(repository, publisher);

        await service.UpdateAsync(new UpdateCourseDto
        {
            Id = 5,
            Name = "English Basics Updated",
            Level = "A2",
            Description = "Starter updated",
            TuitionFee = 1750000,
            ThumbnailUrl = "https://example.com/a2.jpg",
            IsActive = false
        });

        var notification = Assert.Single(publisher.PublishedNotifications);
        Assert.Equal("Updated", notification.Action);
        Assert.NotNull(notification.Course);
        Assert.Equal(5, notification.CourseId);
        Assert.Equal("English Basics Updated", notification.Course!.Name);
        Assert.Equal("A2", notification.Course.Level);
        Assert.False(notification.Course.IsActive);
    }

    [Fact]
    public async Task DeleteAsync_publishes_deleted_course_notification_with_deleted_snapshot()
    {
        var repository = new FakeCourseRepository(
            new Course
            {
                Id = 9,
                Name = "Business English",
                Level = "B1",
                Description = "Weekend class",
                TuitionFee = 2200000,
                IsActive = true
            });
        var publisher = new CapturingCourseNotificationPublisher();
        var service = new CourseService(repository, publisher);

        await service.DeleteAsync(9);

        Assert.Null(await repository.GetByIdAsync(9));

        var notification = Assert.Single(publisher.PublishedNotifications);
        Assert.Equal("Deleted", notification.Action);
        Assert.Equal(9, notification.CourseId);
        Assert.NotNull(notification.Course);
        Assert.Equal("Business English", notification.Course!.Name);
    }

    private sealed class FakeCourseRepository : ICourseRepository
    {
        private readonly List<Course> _courses;
        private int _nextId;

        public FakeCourseRepository(params Course[] courses)
        {
            _courses = courses.ToList();
            _nextId = _courses.Count == 0 ? 1 : _courses.Max(c => c.Id) + 1;
        }

        public Task<IEnumerable<Course>> GetAllAsync() =>
            Task.FromResult(_courses.AsEnumerable());

        public Task<Course?> GetByIdAsync(int id) =>
            Task.FromResult(_courses.FirstOrDefault(c => c.Id == id));

        public Task AddAsync(Course entity)
        {
            if (entity.Id == 0)
            {
                entity.Id = _nextId++;
            }

            _courses.Add(entity);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Course entity) => Task.CompletedTask;

        public Task DeleteAsync(int id)
        {
            var course = _courses.FirstOrDefault(c => c.Id == id);
            if (course is not null)
            {
                _courses.Remove(course);
            }

            return Task.CompletedTask;
        }

        public Task<IEnumerable<Course>> GetActiveCoursesAsync() =>
            Task.FromResult(_courses.Where(c => c.IsActive).AsEnumerable());
    }

    private sealed class CapturingCourseNotificationPublisher : ICourseNotificationPublisher
    {
        public List<CourseChangedNotification> PublishedNotifications { get; } = [];

        public Task PublishCourseChangedAsync(CourseChangedNotification notification)
        {
            PublishedNotifications.Add(notification);
            return Task.CompletedTask;
        }
    }
}
