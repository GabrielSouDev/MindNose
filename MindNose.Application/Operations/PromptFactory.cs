namespace MindNose.Domain.Operations;

public class PromptFactory
{
    public readonly PromptNodeFactory Node;
    public readonly PromptChatFactory Chat;

    public PromptFactory(PromptNodeFactory node, PromptChatFactory chat)
    {
        Node = node;
        Chat = chat;
    }
}