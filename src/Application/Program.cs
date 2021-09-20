using System;
using Microsoft.Extensions.Logging;

LogSomeStuff.TraditionalLog();
LogSomeStuff.MessageDefineLog();
LogSomeStuff.SourceGenerationLog();

public static class LogSomeStuff
{
    private const int SomeInt = 123456;
    private const string SomeString = "Some string";
    private static readonly SomeRecord SomeRecord = new(SomeInt, SomeString);
    private static readonly ILogger Logger = LoggerFactory
        .Create(c => c.AddConsole().SetMinimumLevel(LogLevel.Trace))
        .CreateLogger("LogSomeStuff");
    
    public static void TraditionalLog()
        => Logger.LogInformation(
            "Got some int {someInt}, some string {someString} and some record {someRecord}", 
            SomeInt,
            SomeString,
            SomeRecord);

    #region MessageDefineLog
    
    private static readonly Action<ILogger, int, string, SomeRecord, Exception?> LogSomethingAction =
        LoggerMessage.Define<int, string, SomeRecord>(
            LogLevel.Information,
            new EventId(0, "LogSomething"),
            "Got some int {someInt}, some string {someString} and some record {someRecord}"/*,
                new LogDefineOptions { SkipEnabledCheck = true }*/);
    
    public static void MessageDefineLog() => LogSomethingAction(Logger, SomeInt, SomeString, SomeRecord, null);
    
    #endregion MessageDefineLog
    
    #region SourceGenerationLog
    
    public static void SourceGenerationLog() => Logger.LogSomething(SomeInt, SomeString, SomeRecord);
    
    #endregion SourceGenerationLog
}

public record SomeRecord(int SomeInt, string SomeString);

public static partial class Log
{
    [LoggerMessage(
        EventId = 0,
        Level = LogLevel.Information,
        Message = "Got some int {someInt}, some string {someString} and some record {someRecord}")]
    public static partial void LogSomething(this ILogger logger, int someInt, string someString, SomeRecord someRecord);
}