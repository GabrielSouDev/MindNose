using RelationalGraph.Domain.Node;
using System.Collections.Generic;

namespace RelationalGraph.Application.Operations
{
    public static class QueryFactory
    {
        public static Query CreateKnowledgeNode(TermResult termResult) =>
            new Query(@"WITH $termResult AS initialTerm

                        MERGE (c:Category { Category: initialTerm.Category })
                        ON CREATE SET c.CreatedAt = initialTerm.CreatedAt
                        ON MATCH SET c.CreatedAt = coalesce(c.CreatedAt, initialTerm.CreatedAt)

                        MERGE (n:Term { Term: initialTerm.Term })
                        ON CREATE SET n.Summary = initialTerm.Summary,
                                      n.WeigthCategoryToTerm = initialTerm.WeigthCategoryToTerm,
                                      n.CreatedAt = initialTerm.CreatedAt,
                                      n.prompt_tokens = initialTerm.Usage.prompt_tokens,
                                      n.completion_tokens = initialTerm.Usage.completion_tokens,
                                      n.total_tokens = initialTerm.Usage.total_tokens
                        ON MATCH SET n.Summary = coalesce(n.Summary, initialTerm.Summary),
                                    n.WeigthCategoryToTerm = coalesce(n.WeigthCategoryToTerm, initialTerm.WeigthCategoryToTerm),
                                    n.CreatedAt = coalesce(n.CreatedAt, initialTerm.CreatedAt),
                                    n.prompt_tokens = coalesce(n.prompt_tokens, 0) + initialTerm.Usage.prompt_tokens,
                                    n.completion_tokens = coalesce(n.completion_tokens, 0) + initialTerm.Usage.completion_tokens,
                                    n.total_tokens = coalesce(n.total_tokens, 0) + initialTerm.Usage.total_tokens

                        MERGE (c)-[crel:CONTAINS]->(n)
                        ON CREATE SET crel.WeigthCategoryToTerm = initialTerm.WeigthCategoryToTerm,
                                      crel.CreatedAt = initialTerm.CreatedAt
                        ON MATCH SET crel.WeigthCategoryToTerm = coalesce(crel.WeigthCategoryToTerm, initialTerm.WeigthCategoryToTerm),
                                     crel.CreatedAt = coalesce(crel.CreatedAt, initialTerm.CreatedAt)

                        WITH initialTerm, c, crel, n
                        UNWIND initialTerm.RelatedTerms AS relatedTerm
                        MERGE (r:Term { Term: relatedTerm.Term })
                        ON CREATE SET r.CreatedAt = relatedTerm.CreatedAt
                        ON MATCH SET r.CreatedAt = coalesce(r.CreatedAt, relatedTerm.CreatedAt)

                        MERGE (n)-[rel:RELATED_TO]->(r)
                        ON CREATE SET rel.WeigthTermToTerm = relatedTerm.WeigthTermToTerm,
                                      rel.CreatedAt = relatedTerm.CreatedAt
                        ON MATCH SET rel.WeigthTermToTerm = coalesce(rel.WeigthTermToTerm, relatedTerm.WeigthTermToTerm),
                                     rel.CreatedAt = coalesce(rel.CreatedAt, relatedTerm.CreatedAt)

                        RETURN n, c, crel, rel, r", new { termResult });

        public static Query SearchKnowledgeNode(string category, string initialTerm) =>
            new Query(@"MATCH(n:Term { Term: $initialTerm })-[rel]->(r) LIMIT 10
                      
                        RETURN n, rel, r", new { initialTerm });
    }

}