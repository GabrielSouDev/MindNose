using UsageClass = MindNose.Domain.Results.Usage;
using MindNose.Domain.Results;

namespace MindNose.Domain.Consts;

public static class LinksResultFields
{
    public const string Category = nameof(LinksResult.Category);
    public const string Term = nameof(LinksResult.Term);
    public const string RelatedTerms = nameof(LinksResult.RelatedTerms);
    public const string WasCreated = nameof(LinksResult.WasCreated);
    public const string CreatedAt = nameof(LinksResult.CreatedAt);
}

public static class NodeFields
{
    public const string Title = nameof(NodeResult.Title);
    public const string TitleId = nameof(NodeResult.TitleId);
    public const string Embedding = nameof(NodeResult.Embedding);
    public const string CanonicalDefinition = nameof(TermResult.CanonicalDefinition);
    public const string MainFunction = nameof(TermResult.MainFunction);
    public const string ConceptualCategory = nameof(TermResult.ConceptualCategory);
    public const string Definition = nameof(CategoryResult.Definition);
    public const string Function = nameof(CategoryResult.Function);
    public const string CreatedAt = nameof(NodeResult.CreatedAt);
}

public static class RelationshipFields
{
    public const string CategoryToTermWeigth = nameof(LinksResult.Term.CategoryToTermWeigth);
    public const string CategoryToRelatedTermWeigth = nameof(RelatedTermResult.CategoryToRelatedTermWeigth);
    public const string InitialTermToRelatedTermWeigth = nameof(RelatedTermResult.InitialTermToRelatedTermWeigth);
}

public static class UsageFields
{
    // Usage
    public const string PromptTokens = nameof(UsageClass.PromptTokens);
    public const string CompletionTokens = nameof(UsageClass.CompletionTokens);
    public const string TotalTokens = nameof(UsageClass.TotalTokens);
}