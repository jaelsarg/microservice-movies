using Microsoft.Extensions.Logging;

namespace UnitTests.Mocks;

public class TestLogger<T> : ILogger<T>
{
    public List<string> LoggedMessages { get; } = [];
    public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;
    public bool IsEnabled(LogLevel logLevel) => true;
    public void Log<TState>(
        LogLevel logLevel, 
        EventId eventId, 
        TState state, 
        Exception? exception, 
        Func<TState, Exception?, string> formatter
    )
    {
        Console.WriteLine(formatter(state, exception));
        LoggedMessages.Add(formatter(state, exception));
    }
}