namespace MindNose.Domain.CMDs;

public class Query
{
    public Query(string commandLine, object parameters)
    {
        CommandLine = commandLine;
        Parameters = parameters;
    }
    public string CommandLine { get; private set; }
    public object Parameters { get; private set; }

}