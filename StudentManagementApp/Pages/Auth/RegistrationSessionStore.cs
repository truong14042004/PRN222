using System.Text.Json;
using Microsoft.AspNetCore.Http;
using StudentManagementApp.BLL.DTOs;

namespace StudentManagementApp.Pages.Auth;

internal static class RegistrationSessionStore
{
    private const string PendingRegistrationKey = "PendingRegistration";

    public static void Save(ISession session, CreateUserDto registration)
    {
        session.SetString(PendingRegistrationKey, JsonSerializer.Serialize(registration));
    }

    public static CreateUserDto? Get(ISession session)
    {
        var payload = session.GetString(PendingRegistrationKey);
        if (string.IsNullOrWhiteSpace(payload))
        {
            return null;
        }

        try
        {
            return JsonSerializer.Deserialize<CreateUserDto>(payload);
        }
        catch (JsonException)
        {
            Clear(session);
            return null;
        }
    }

    public static void Clear(ISession session)
    {
        session.Remove(PendingRegistrationKey);
    }
}
