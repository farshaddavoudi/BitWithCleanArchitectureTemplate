using Bit.Core.Contracts;
using Bit.Core.Models;
using Serilog;
using Serilog.Context;
using Serilog.Exceptions;
using Template.Domain.Shared;

namespace Template.Infrastructure.LogStore;

public class SeqLogStore : ILogStore
{
    public Task SaveLogAsync(LogEntry logEntry)
    {
        SaveLog(logEntry);

        return Task.CompletedTask;
    }

    public void SaveLog(LogEntry logEntry)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.WithProperty("ApplicationName", AppMetadata.SolutionName)
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .Enrich.WithMachineName()
            .Enrich.WithClientIp()
            .Enrich.WithClientAgent()
            .WriteTo.Seq("http://log.app.ataair.ir", apiKey: "iW1OLXzF01n3gSJ70qkR")
            .CreateLogger();

        foreach (var prop in logEntry.GetType().GetProperties())
            LogContext.PushProperty(prop.Name, prop.GetValue(logEntry));

        var trackingCode = logEntry.LogData.SingleOrDefault(l => l.Key == "X-Correlation-ID")?.Value?.ToString();

        if (!string.IsNullOrWhiteSpace(trackingCode))
            LogContext.PushProperty("TrackingCode", trackingCode);

        string message = string.IsNullOrWhiteSpace(trackingCode) ? logEntry.Message : $"TrackingCode: {trackingCode}";

        if (logEntry.Severity == LogSeverity.Fatal.ToString())
            Log.Logger.Fatal(message);

        else
            Log.Logger.Error(message);
    }
}

public enum LogSeverity
{
    Warning,
    Fatal
}