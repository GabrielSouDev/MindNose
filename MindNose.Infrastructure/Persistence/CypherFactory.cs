using MindNose.Domain.Consts;
using MindNose.Domain.Nodes;
using MindNose.Domain.Request;
using MindNose.Domain.Results;
using Query = MindNose.Domain.CMDs.Query;


namespace MindNose.Infrastructure.Persistence;

public static class CypherFactory
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
            "MERGE (category:Category { TitleId: initialTerm.Category.TitleId }) " +
            "ON MATCH SET " +
                "category.Embedding = CASE " +
                    "WHEN category.Embedding IS NULL OR size(category.Embedding) = 0 " +
                    "THEN initialTerm.Category.Embedding " +
                    "ELSE category.Embedding " +
                "END, " +
                "category.PromptTokens     = coalesce(category.PromptTokens, 0) + initialTerm.Usage.PromptTokens, " +
                "category.CompletionTokens = coalesce(category.CompletionTokens, 0) + initialTerm.Usage.CompletionTokens, " +
                "category.TotalTokens      = coalesce(category.TotalTokens, 0) + initialTerm.Usage.TotalTokens " +

            // Termo principal
            "MERGE (term:Term { TitleId: initialTerm.Term.TitleId }) " +
            "ON CREATE SET " +
                "term.Title               = initialTerm.Term.Title, " +
                "term.CanonicalDefinition = initialTerm.Term.CanonicalDefinition, " +
                "term.MainFunction        = initialTerm.Term.MainFunction, " +
                "term.ConceptualCategory  = initialTerm.Term.ConceptualCategory, " +
                "term.Embedding           = initialTerm.Term.Embedding, " +
                "term.CreatedAt           = initialTerm.CreatedAt, " +
                "term.PromptTokens        = initialTerm.Usage.PromptTokens, " +
                "term.CompletionTokens    = initialTerm.Usage.CompletionTokens, " +
                "term.TotalTokens         = initialTerm.Usage.TotalTokens " +
            "ON MATCH SET " +
                "term.Title            = coalesce(term.Title, initialTerm.Term.Title), " +
                "term.CanonicalDefinition = coalesce(term.CanonicalDefinition, initialTerm.Term.CanonicalDefinition), " +
                "term.MainFunction        = coalesce(term.MainFunction, initialTerm.Term.MainFunction), " +
                "term.ConceptualCategory  = coalesce(term.ConceptualCategory, initialTerm.Term.ConceptualCategory), " +
                "term.Embedding           = coalesce(term.Embedding, initialTerm.Term.Embedding), " +
                "term.PromptTokens        = coalesce(term.PromptTokens, 0) + initialTerm.Usage.PromptTokens, " +
                "term.CompletionTokens    = coalesce(term.CompletionTokens, 0) + initialTerm.Usage.CompletionTokens, " +
                "term.TotalTokens         = coalesce(term.TotalTokens, 0) + initialTerm.Usage.TotalTokens " +

            // Relacionamento categoria -> termo principal
            "MERGE (category)-[relationshipContains:CONTAINS]->(term) " +
            "ON CREATE SET " +
                "relationshipContains.WeightStartToEnd = initialTerm.Term.CategoryToTermWeigth, " +
                "relationshipContains.CreatedAt        = initialTerm.CreatedAt " +
            "ON MATCH SET " +
                "relationshipContains.WeightStartToEnd = coalesce(relationshipContains.WeightStartToEnd, initialTerm.Term.CategoryToTermWeigth) " +

            // Termos relacionados
            "WITH term, category, relationshipContains, initialTerm " +
            "UNWIND initialTerm.RelatedTerms AS relatedTermParam " +

            // Nó do termo relacionado
            "MERGE (relatedTerm:Term { TitleId: relatedTermParam.TitleId }) " +
            "ON CREATE SET " +
                "relatedTerm.Title               = relatedTermParam.Title, " +
                "relatedTerm.CanonicalDefinition = relatedTermParam.CanonicalDefinition, " +
                "relatedTerm.MainFunction        = relatedTermParam.MainFunction, " +
                "relatedTerm.ConceptualCategory  = relatedTermParam.ConceptualCategory, " +
                "relatedTerm.Embedding           = relatedTermParam.Embedding, " +
                "relatedTerm.CreatedAt           = relatedTermParam.CreatedAt " +

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
            
           $"RETURN {NodeName.Term}, {NodeName.Category}, {NodeName.RelatedTerm}, " +
            $"{RelationshipType.RelationshipContains}, {RelationshipType.RelationshipRelated}, {RelationshipType.RelationshipContainsRelated}", 
            new { termResult }
        );

    public static Query GetLinks(LinksRequest request) =>
        new Query(
            "WITH $request AS request " +
            "MATCH path=(category:Category { TitleId: request.CategoryId })-[relationshipContains:CONTAINS]->(term:Term { TitleId: request.TermId }) " +

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

            "MERGE (category:Category { TitleId: newCategory.Category.TitleId }) " +
            "ON CREATE SET " +
                "category.Title            = newCategory.Category.Title, " +
                "category.Definition       = newCategory.Category.Definition, " +
                "category.Function         = newCategory.Category.Function, " +
                "category.Embedding        = newCategory.Category.Embedding, " +
                "category.CreatedAt        = newCategory.CreatedAt, " +
                "category.PromptTokens     = newCategory.Usage.PromptTokens, " +
                "category.CompletionTokens = newCategory.Usage.CompletionTokens, " +
                "category.TotalTokens      = newCategory.Usage.TotalTokens " +
            "ON MATCH SET " +
                "category.Title            = coalesce(category.Title, newCategory.Category.Title), " +
                "category.Definition       = coalesce(category.Definition, newCategory.Category.Definition), " +
                "category.Function         = coalesce(category.Function, newCategory.Category.Function), " +
                "category.Embedding        = coalesce(category.Embedding, newCategory.Category.Embedding), " +
                "category.PromptTokens     = coalesce(category.PromptTokens, 0) + newCategory.Usage.PromptTokens, " +
                "category.CompletionTokens = coalesce(category.CompletionTokens, 0) + newCategory.Usage.CompletionTokens, " +
                "category.TotalTokens      = coalesce(category.TotalTokens, 0) + newCategory.Usage.TotalTokens "
            , new { categoryResult });
}