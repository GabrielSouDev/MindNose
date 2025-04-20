using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RelationalGraph.Application.Operations
{
    public class Query
    {
        public Query(string commandLine, string[] parameters)
        {
            CommandLine = commandLine;
            Parameters = parameters;
        }
        public string CommandLine { get; private set; }
        public string[] Parameters { get; private set; }
        public static Query CreateNode(string[] parameters) =>
            new Query("CREATE (n:Person {name: $personName}) RETURN n", parameters);
    }
}
