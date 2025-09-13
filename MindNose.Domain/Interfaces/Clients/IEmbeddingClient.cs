namespace MindNose.Domain.Interfaces.Clients;
public interface IEmbeddingClient
{
    Task InitializeAsync();
    float[] GetSentenceEmbedding(string sentences);
    float[][] GetSentenceEmbeddingsBatch(string[] sentences);
    double CosineSimilarity(float[] firstVector, float[] secondVector);
}