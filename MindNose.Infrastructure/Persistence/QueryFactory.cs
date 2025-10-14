using MindNose.Domain.Consts;
using MindNose.Domain.Request;
using MindNose.Domain.Results;
using Query = MindNose.Domain.CMDs.Query;


namespace MindNose.Infrastructure.Persistence;

public static class QueryFactory
{
    public static Query WarmUpWithOutResults() =>
        new Query(
            "MATCH (n)-[r]->(m)" +
            "RETURN 1");
    public static Query CompleteWarmUp() =>
        new Query(
           "MATCH (n)-[r]->(m)" +
           "RETURN count(n) AS nodeCount, count(r) AS relationshipCount");
    public static Query CreateLinks(TermResult termResult) =>
        new Query(
            "WITH $termResult AS initialTerm " +

            // Categoria
            "MERGE (category:Category { Title: initialTerm.Category }) " +
            "ON CREATE SET category.CreatedAt = initialTerm.CreatedAt " +
            "ON MATCH  SET category.CreatedAt = coalesce(category.CreatedAt, initialTerm.CreatedAt) " +

            // Termo principal
            "MERGE (term:Term { Title: initialTerm.Term }) " +
            "ON CREATE SET " +
              "term.Summary          = initialTerm.Summary, " +
              "term.CreatedAt        = initialTerm.CreatedAt, " +
              "term.PromptTokens     = initialTerm.Usage.prompt_tokens, " +
              "term.CompletionTokens = initialTerm.Usage.completion_tokens, " +
              "term.TotalTokens      = initialTerm.Usage.total_tokens " +
            "ON MATCH SET " +
              "term.Summary          = coalesce(term.Summary, initialTerm.Summary), " +
              "term.CreatedAt        = coalesce(term.CreatedAt, initialTerm.CreatedAt), " +
              "term.PromptTokens     = coalesce(term.PromptTokens, 0) + initialTerm.Usage.prompt_tokens, " +
              "term.CompletionTokens = coalesce(term.CompletionTokens, 0) + initialTerm.Usage.completion_tokens, " +
              "term.TotalTokens      = coalesce(term.TotalTokens, 0) + initialTerm.Usage.total_tokens " +

            // Relacionamento categoria -> termo principal
            "MERGE (category)-[relationshipContains:CONTAINS]->(term) " +
            "ON CREATE SET " +
              "relationshipContains.WeightStartToEnd = initialTerm.CategoryToTermWeigth, " +
              "relationshipContains.CreatedAt        = initialTerm.CreatedAt " +
            "ON MATCH SET " +
              "relationshipContains.WeightStartToEnd = coalesce(relationshipContains.WeightStartToEnd, initialTerm.CategoryToTermWeigth), " +
              "relationshipContains.CreatedAt        = coalesce(relationshipContains.CreatedAt, initialTerm.CreatedAt) " +

            // Termos relacionados
            "WITH term, category, relationshipContains, initialTerm " +
            "UNWIND initialTerm.RelatedTerms AS relatedTermParam " +

            // Nó do termo relacionado
            "MERGE (relatedTerm:Term { Title: relatedTermParam.Term }) " +
            "ON CREATE SET relatedTerm.CreatedAt = relatedTermParam.CreatedAt " +
            "ON MATCH  SET relatedTerm.CreatedAt = coalesce(relatedTerm.CreatedAt, relatedTermParam.CreatedAt) " +

            // Relacionamento termo principal -> termo relacionado
            "MERGE (term)-[relationshipRelated:RELATED_TO]->(relatedTerm) " +
            "ON CREATE SET " +
              "relationshipRelated.WeightStartToEnd = relatedTermParam.InitialTermToRelatedTermWeigth, " +
              "relationshipRelated.CreatedAt        = relatedTermParam.CreatedAt " +
            "ON MATCH SET " +
              "relationshipRelated.WeightStartToEnd = coalesce(relationshipRelated.WeightStartToEnd, relatedTermParam.InitialTermToRelatedTermWeigth), " +
              "relationshipRelated.CreatedAt        = coalesce(relationshipRelated.CreatedAt, relatedTermParam.CreatedAt) " +

            // Relacionamento categoria -> termo relacionado
            "MERGE (category)-[relationshipContainsRelated:CONTAINS]->(relatedTerm) " +
            "ON CREATE SET " +
              "relationshipContainsRelated.WeightStartToEnd = relatedTermParam.CategoryToRelatedTermWeigth, " +
              "relationshipContainsRelated.CreatedAt        = relatedTermParam.CreatedAt " +
            "ON MATCH SET " +
              "relationshipContainsRelated.WeightStartToEnd = coalesce(relationshipContainsRelated.WeightStartToEnd, relatedTermParam.CategoryToRelatedTermWeigth), " +
              "relationshipContainsRelated.CreatedAt        = coalesce(relationshipContainsRelated.CreatedAt, relatedTermParam.CreatedAt) " +
            
           $"RETURN {NodeType.Term}, {NodeType.Category}, {NodeType.RelatedTerm}, " +
            $"{RelationshipType.RelationshipContains}, {RelationshipType.RelationshipRelated}, {RelationshipType.RelationshipContainsRelated}", 
            new { termResult }
        );

    //public static Query GetLinks(LinksRequest request) =>
    //    new Query(
    //        "WITH $request AS request " +
            
    //        $"MATCH (category:Category {{ Title: request.Category }}) " +
    //        $"MATCH (category)-[relationshipContains:CONTAINS*1..{request.LengthPath}]->(term:Term {{ Title: request.Term }}) " +

    //        $"OPTIONAL MATCH (term)-[relationshipRelated:RELATED_TO*1..{request.LengthPath}]->(relatedTerm) " +
    //        $"OPTIONAL MATCH (category)-[relationshipContainsRelated:CONTAINS*1..{request.LengthPath}]->(relatedTerm) " +
            
    //         "RETURN term, category, relationshipContains, relationshipRelated, relationshipContainsRelated, relatedTerm " +
    //        $"LIMIT {request.Limit} " +
    //        $"SKIP {request.Skip}",
    //        new { request }
    //    );
    public static Query GetLinks(LinksRequest request)
    {
        return new Query(
            "WITH $request AS request " +
            "MATCH path=(category:Category { Title: request.Category })-[relationshipContains:CONTAINS]->(term:Term { Title: request.Term }) " +

           $"OPTIONAL MATCH relatedPath = (term)-[:RELATED_TO*1..{request.LengthPath}]->(relatedTerm) " +
            "UNWIND CASE WHEN relatedPath IS NULL THEN [] ELSE relationships(relatedPath) END AS relationshipRelated " +

            "RETURN term, category, relatedTerm, relationshipContains, relationshipRelated " +
            "ORDER BY relationshipRelated.WeightStartToEnd " +
           $"SKIP {request.Skip} " +
           $"LIMIT {request.Limit}", new { request });
    }
        
}