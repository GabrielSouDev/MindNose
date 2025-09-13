namespace MindNose.Domain.Exceptions;

public class LinksNotCreatedException : Exception
{
    public LinksNotCreatedException() { }

    public LinksNotCreatedException(string? message) : base(message) { }
}
