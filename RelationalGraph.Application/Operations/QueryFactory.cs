using RelationalGraph.Domain.Node;
using System.Collections.Generic;

namespace RelationalGraph.Application.Operations
{
    public static class QueryFactory
    {
        public static Query CreateKnowledgeNode(TermResult termResult) =>
            new Query(@"WITH $termResult AS initialTerm

                        MERGE (c:Category { Title: initialTerm.Category })
                        ON CREATE SET c.CreatedAt = initialTerm.CreatedAt
                        ON MATCH SET c.CreatedAt = coalesce(c.CreatedAt, initialTerm.CreatedAt)

                        MERGE (n:Term { Title: initialTerm.Term })
                        ON CREATE SET n.Summary = initialTerm.Summary,
                                      n.CreatedAt = initialTerm.CreatedAt,
                                      n.prompt_tokens = initialTerm.Usage.prompt_tokens,
                                      n.completion_tokens = initialTerm.Usage.completion_tokens,
                                      n.total_tokens = initialTerm.Usage.total_tokens
                        ON MATCH SET n.Summary = coalesce(n.Summary, initialTerm.Summary),
                                    n.CreatedAt = coalesce(n.CreatedAt, initialTerm.CreatedAt),
                                    n.prompt_tokens = coalesce(n.prompt_tokens, 0) + initialTerm.Usage.prompt_tokens,
                                    n.completion_tokens = coalesce(n.completion_tokens, 0) + initialTerm.Usage.completion_tokens,
                                    n.total_tokens = coalesce(n.total_tokens, 0) + initialTerm.Usage.total_tokens

                        MERGE (c)-[crel:CONTAINS]->(n)
                        ON CREATE SET crel.WeigthStartToEnd = initialTerm.WeigthCategoryToTerm,
                                      crel.CreatedAt = initialTerm.CreatedAt
                        ON MATCH SET crel.WeigthStartToEnd = coalesce(crel.WeigthStartToEnd, initialTerm.WeigthCategoryToTerm),
                                     crel.CreatedAt = coalesce(crel.CreatedAt, initialTerm.CreatedAt)

                        WITH initialTerm, c, crel, n
                        UNWIND initialTerm.RelatedTerms AS relatedTerm
                        MERGE (r:Term { Title: relatedTerm.Term })
                        ON CREATE SET r.CreatedAt = relatedTerm.CreatedAt
                        ON MATCH SET r.CreatedAt = coalesce(r.CreatedAt, relatedTerm.CreatedAt)

                        MERGE (n)-[rel:RELATED_TO]->(r)
                        ON CREATE SET rel.WeigthStartToEnd = relatedTerm.WeigthTermToTerm,
                                      rel.CreatedAt = relatedTerm.CreatedAt
                        ON MATCH SET rel.WeigthStartToEnd = coalesce(rel.WeigthStartToEnd, relatedTerm.WeigthTermToTerm),
                                     rel.CreatedAt = coalesce(rel.CreatedAt, relatedTerm.CreatedAt)

                        RETURN n, c, crel, rel, r", new { termResult });

        public static Query SearchKnowledgeNode(string category, string initialTerm) =>
            new Query(@"MATCH(n:Term { Title: $initialTerm })
                        WITH n
                        MATCH(c:Category { Title: $category})-[crel:CONTAINS]->(n)
                        MATCH(n)-[rel:RELATED_TO]->(r) LIMIT 10
                      
                        RETURN n, c, crel, rel, r", new { category, initialTerm });
    }

}