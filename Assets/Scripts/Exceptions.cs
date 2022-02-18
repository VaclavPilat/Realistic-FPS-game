using System;


// Exception for console warning
public class ConsoleWarningException : Exception
{
    public ConsoleWarningException (string message)
        : base(message)
    {
    }
}