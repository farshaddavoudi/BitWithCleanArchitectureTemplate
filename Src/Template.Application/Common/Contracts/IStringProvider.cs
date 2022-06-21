namespace Template.Application.Common.Contracts;

public interface IStringProvider
{
    string? Message(string messageStringsKey);

    string Exception(string exceptionStringsKey);
}