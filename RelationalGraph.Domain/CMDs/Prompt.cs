namespace RelationalGraph.Domain.CMDs;
public class Prompt
{
    public Prompt(string message)
    {
        Message = message;
    }
    public string Message { get; private set; }
}