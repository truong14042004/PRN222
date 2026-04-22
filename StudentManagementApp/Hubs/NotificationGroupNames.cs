namespace StudentManagementApp.Hubs;

public static class NotificationGroupNames
{
    public const string Admins = "admins";

    public static string Student(int studentId) => $"student:{studentId}";
}
