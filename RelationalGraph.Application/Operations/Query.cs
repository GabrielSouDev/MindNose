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

        //TESTAR E CRIAR NOVAS QUERYS - TESTAR MERGE
        public static Query CreateKnowledgeNode(TermResult termResult) =>
            new Query(@"WITH $termResult AS initialTerm
                        CREATE (n:Term { Term: initialTerm.Term, 
                                         Summary: initialTerm.Summary, 
                                         WeigthCategoryToTerm: initialTerm.WeigthCategoryToTerm, 
                                         CreatedAt: initialTerm.CreatedAt})
                        WITH initialTerm, n
                        UNWIND initialTerm.RelatedTerms AS relatedTerm
                        CREATE (r:Term { Term: relatedTerm.Term,
                                         WeigthCategoryToTerm: relatedTerm.WeigthCategoryToTerm,
                                         WeigthTermToTerm: relatedTerm.WeigthTermToTerm,
                                         CreatedAt: relatedTerm.CreatedAt})
                        CREATE (n)-[:RELATED_TO { prompt_tokens: initialTerm.Usage.prompt_tokens,
                                                  completion_tokens: initialTerm.Usage.completion_tokens,
                                                  total_tokens: initialTerm.Usage.total_tokens }]->(r)
                        RETURN n", new { termResult });
    }
}