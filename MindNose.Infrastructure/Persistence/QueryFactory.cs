using MindNose.Domain.Consts;
using MindNose.Domain.Nodes;
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

    public static Query CreateLinks(LinksResult termResult) =>
        new Query(
            "WITH $termResult AS initialTerm " +

            // Categoria
            "MERGE (category:Category { Title: initialTerm.Category.Title }) " +
            "ON MATCH SET " +
                "category.PromptTokens     = coalesce(category.PromptTokens, 0) + initialTerm.Usage.prompt_tokens, " +
                "category.CompletionTokens = coalesce(category.CompletionTokens, 0) + initialTerm.Usage.completion_tokens, " +
                "category.TotalTokens      = coalesce(category.TotalTokens, 0) + initialTerm.Usage.total_tokens " +

            // Termo principal
            "MERGE (term:Term { Title: initialTerm.Term.Title }) " +
            "ON CREATE SET " +
                "term.Summary          = initialTerm.Term.Summary, " +
                "term.CreatedAt        = initialTerm.CreatedAt, " +
                "term.PromptTokens     = initialTerm.Usage.prompt_tokens, " +
                "term.CompletionTokens = initialTerm.Usage.completion_tokens, " +
                "term.TotalTokens      = initialTerm.Usage.total_tokens " +
            "ON MATCH SET " +
                "term.Summary          = coalesce(term.Summary, initialTerm.Term.Summary), " +
                "term.PromptTokens     = coalesce(term.PromptTokens, 0) + initialTerm.Usage.prompt_tokens, " +
                "term.CompletionTokens = coalesce(term.CompletionTokens, 0) + initialTerm.Usage.completion_tokens, " +
                "term.TotalTokens      = coalesce(term.TotalTokens, 0) + initialTerm.Usage.total_tokens " +

            // Relacionamento categoria -> termo principal
            "MERGE (category)-[relationshipContains:CONTAINS]->(term) " +
            "ON CREATE SET " +
                "relationshipContains.WeightStartToEnd = initialTerm.CategoryToTermWeigth, " +
                "relationshipContains.CreatedAt        = initialTerm.CreatedAt " +
            "ON MATCH SET " +
                "relationshipContains.WeightStartToEnd = coalesce(relationshipContains.WeightStartToEnd, initialTerm.CategoryToTermWeigth) " +

            // Termos relacionados
            "WITH term, category, relationshipContains, initialTerm " +
            "UNWIND initialTerm.RelatedTerms AS relatedTermParam " +

            // Nó do termo relacionado
            "MERGE (relatedTerm:Term { Title: relatedTermParam.Title }) " +
            "ON CREATE SET " +
                "relatedTerm.Summary   = relatedTermParam.Summary, " +
                "relatedTerm.CreatedAt = relatedTermParam.CreatedAt " +

            // Relacionamento termo principal -> termo relacionado
            "MERGE (term)-[relationshipRelated:RELATED_TO]->(relatedTerm) " +
            "ON CREATE SET " +
                "relationshipRelated.WeightStartToEnd = relatedTermParam.InitialTermToRelatedTermWeigth, " +
                "relationshipRelated.CreatedAt        = relatedTermParam.CreatedAt " +
            "ON MATCH SET " +
                "relationshipRelated.WeightStartToEnd = " +
                    "coalesce(relationshipRelated.WeightStartToEnd, relatedTermParam.InitialTermToRelatedTermWeigth) " +

            // Relacionamento categoria -> termo relacionado
            "MERGE (category)-[relationshipContainsRelated:CONTAINS]->(relatedTerm) " +
            "ON CREATE SET " +
                "relationshipContainsRelated.WeightStartToEnd = relatedTermParam.CategoryToRelatedTermWeigth, " +
                "relationshipContainsRelated.CreatedAt        = relatedTermParam.CreatedAt " +
            "ON MATCH SET " +
                "relationshipContainsRelated.WeightStartToEnd = " +
                    "coalesce(relationshipContainsRelated.WeightStartToEnd, relatedTermParam.CategoryToRelatedTermWeigth) " +
            
           $"RETURN {NodeType.Term}, {NodeType.Category}, {NodeType.RelatedTerm}, " +
            $"{RelationshipType.RelationshipContains}, {RelationshipType.RelationshipRelated}, {RelationshipType.RelationshipContainsRelated}", 
            new { termResult }
        );

    public static Query GetLinks(LinksRequest request) =>
        new Query(
            "WITH $request AS request " +
            "MATCH path=(category:Category { Title: request.Category })-[relationshipContains:CONTAINS]->(term:Term { Title: request.Term }) " +

           $"OPTIONAL MATCH relatedPath = (term)-[:RELATED_TO*1..{request.LengthPath}]->(relatedTerm) " +
            "UNWIND CASE WHEN relatedPath IS NULL THEN [] ELSE relationships(relatedPath) END AS relationshipRelated " +

            "OPTIONAL MATCH (category)-[relationshipContainsRelated:CONTAINS]->(relatedTerm) " +

            "RETURN term, category, relatedTerm, relationshipContains, relationshipRelated, relationshipContainsRelated " +
            "ORDER BY relationshipRelated.WeightStartToEnd " +
           $"SKIP {request.Skip} " +
           $"LIMIT {request.Limit}", new { request });

    public static Query GetCategories() =>
        new Query(
            "MATCH (category:Category)" +
            "RETURN category");

    public static Query GetCategory(string categoryName) =>
        new Query(
            "WITH $categoryName AS cName " +
            "MATCH (category:Category { Title: cName })" +
            "RETURN category", new { categoryName });

    public static Query CreateCategory(LinksResult categoryResult) =>
        new Query(
            "WITH $categoryResult AS newCategory " +

            "MERGE (category:Category { Title: newCategory.Category.Title }) " +
            "ON CREATE SET " +
                "category.Summary          = newCategory.Category.Summary, " +
                "category.CreatedAt        = newCategory.CreatedAt, " +
                "category.PromptTokens     = newCategory.Usage.prompt_tokens, " +
                "category.CompletionTokens = newCategory.Usage.completion_tokens, " +
                "category.TotalTokens      = newCategory.Usage.total_tokens " +
            "ON MATCH SET " +
                "category.Summary          = coalesce(category.Summary, newCategory.Category.Summary), " +
                "category.PromptTokens     = coalesce(category.PromptTokens, 0) + newCategory.Usage.prompt_tokens, " +
                "category.CompletionTokens = coalesce(category.CompletionTokens, 0) + newCategory.Usage.completion_tokens, " +
                "category.TotalTokens      = coalesce(category.TotalTokens, 0) + newCategory.Usage.total_tokens "
            , new { categoryResult });
}