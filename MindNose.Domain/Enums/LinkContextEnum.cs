namespace MindNose.Domain.Enums;

// TODO: Para quando implementar Logs de consulta
public enum LinkContextEnum
{
    ManualSearch,        // busca feita pelo usuário
    SuggestedByAI,       // IA sugeriu o termo
    UserNavigation,      // usuário clicou navegando
    SystemGenerated,     // ligação automática do sistema
    ImportedFromSource,  // importado de outro lugar
    Unknown              // fallback
}
