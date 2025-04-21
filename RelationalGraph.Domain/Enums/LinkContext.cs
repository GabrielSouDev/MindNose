namespace RelationalGraph.Domain.Enums
{
    public enum LinkContext
    {
        ManualSearch,        // busca feita pelo usuário
        SuggestedByAI,       // IA sugeriu o termo
        UserNavigation,      // usuário clicou navegando
        SystemGenerated,     // ligação automática do sistema
        ImportedFromSource,  // importado de outro lugar
        Unknown              // fallback
    }
}
