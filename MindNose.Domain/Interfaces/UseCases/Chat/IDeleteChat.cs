namespace MindNose.Domain.Interfaces.UseCases.Chat;

public interface IDeleteChat
{
    Task ExecuteAsync(Guid id);
}