namespace MindNose.Domain.Interfaces.Clients;
public interface IEmbeddingClient
{
    Task<double[][]> GetSentenceEmbedding(string[] sentences);
    float CosineSimilarity(double[] firstVector, double[] secondVector);
}