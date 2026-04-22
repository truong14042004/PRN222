using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace StudentManagementApp.Tests.TestSupport;

internal sealed class TestSession : ISession
{
    private readonly Dictionary<string, byte[]> _store = new(StringComparer.Ordinal);

    public IEnumerable<string> Keys => _store.Keys;
    public string Id { get; } = Guid.NewGuid().ToString("N");
    public bool IsAvailable => true;

    public void Clear() => _store.Clear();

    public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

    public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

    public void Remove(string key) => _store.Remove(key);

    public void Set(string key, byte[] value) => _store[key] = value;

    public bool TryGetValue(string key, out byte[] value)
    {
        if (_store.TryGetValue(key, out var existing))
        {
            value = existing;
            return true;
        }

        value = Array.Empty<byte>();
        return false;
    }
}

internal sealed class TestSessionFeature : ISessionFeature
{
    public ISession Session { get; set; } = new TestSession();
}

internal sealed class TestTempDataProvider : ITempDataProvider
{
    private readonly Dictionary<string, object> _values = new(StringComparer.Ordinal);

    public IDictionary<string, object> LoadTempData(HttpContext context) => new Dictionary<string, object>(_values);

    public void SaveTempData(HttpContext context, IDictionary<string, object> values)
    {
        _values.Clear();
        foreach (var pair in values)
        {
            _values[pair.Key] = pair.Value;
        }
    }
}

internal static class PageModelTestExtensions
{
    public static (DefaultHttpContext HttpContext, TestSession Session, ITempDataDictionary TempData) AttachHttpContext(this PageModel model)
    {
        var httpContext = new DefaultHttpContext();
        var session = new TestSession();
        httpContext.Features.Set<ISessionFeature>(new TestSessionFeature { Session = session });

        model.PageContext = new PageContext(new ActionContext(httpContext, new RouteData(), new ActionDescriptor()));
        var tempData = new TempDataDictionary(httpContext, new TestTempDataProvider());
        model.TempData = tempData;

        return (httpContext, session, tempData);
    }
}
