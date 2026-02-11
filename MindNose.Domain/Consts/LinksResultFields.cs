using UsageClass = MindNose.Domain.Results.Usage;
using MindNose.Domain.Results;

namespace MindNose.Domain.Consts;

public static class LinksResultFields
{
    // LinksResult
    public const string Category = nameof(LinksResult.Category);
    public const string Term = nameof(LinksResult.Term);
    public const string CategoryToTermWeigth = nameof(LinksResult.CategoryToTermWeigth);
    public const string RelatedTerms = nameof(LinksResult.RelatedTerms);
    public const string Usage = nameof(LinksResult.Usage);
    public const string WasCreated = nameof(LinksResult.WasCreated);
    public const string CreatedAt = nameof(LinksResult.CreatedAt);

    // CategoryResult (inherits NodeResult)
    public const string Category_Title = nameof(NodeResult.Title);
    public const string Category_TitleId = nameof(NodeResult.TitleId);
    public const string Category_Summary = nameof(NodeResult.Summary);

    // TermResult (inherits NodeResult)
    public const string Term_Title = nameof(NodeResult.Title);
    public const string Term_TitleId = nameof(NodeResult.TitleId);
    public const string Term_Summary = nameof(NodeResult.Summary);
    public const string Term_CreatedAt = nameof(TermResult.CreatedAt);

    // RelatedTermResult (inherits NodeResult)
    public const string RelatedTerm_Title = nameof(NodeResult.Title);
    public const string RelatedTerm_TitleId = nameof(NodeResult.TitleId);
    public const string RelatedTerm_Summary = nameof(NodeResult.Summary);
    public const string RelatedTerm_CategoryToRelatedTermWeigth = nameof(RelatedTermResult.CategoryToRelatedTermWeigth);
    public const string RelatedTerm_InitialTermToRelatedTermWeigth = nameof(RelatedTermResult.InitialTermToRelatedTermWeigth);
    public const string RelatedTerm_CreatedAt = nameof(RelatedTermResult.CreatedAt);

    // Usage
    public const string Usage_PromptTokens = nameof(UsageClass.PromptTokens);
    public const string Usage_CompletionTokens = nameof(UsageClass.CompletionTokens);
    public const string Usage_TotalTokens = nameof(UsageClass.TotalTokens);
}
