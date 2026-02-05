namespace MindNose.Domain.Interfaces.Clients;
public interface IEmbeddingClient
{
    Task<double[][]> GetSentenceEmbeddingAsync(string[] sentences);
    double CosineSimilaritySIMD(double[] firstVector, double[] secondVector);
}