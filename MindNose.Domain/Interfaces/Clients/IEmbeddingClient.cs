namespace MindNose.Domain.Interfaces.Clients;
public interface IEmbeddingClient
{
    Task<double[][]> GetSentenceEmbeddingAsync(string[] sentences);
    float CosineSimilarity(double[] firstVector, double[] secondVector);
}