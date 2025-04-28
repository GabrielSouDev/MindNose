using RelationalGraph.Domain.Node;

namespace RelationalGraph.Application.Operations
{
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
}