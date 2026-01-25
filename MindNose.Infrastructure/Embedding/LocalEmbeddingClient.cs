using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;
using MindNose.Domain.Interfaces.Clients;
using MindNose.Domain.Interfaces.Commons;
using Tokenizers.HuggingFace.Tokenizer;

namespace MindNose.Infrastructure.HttpClients;

// TODO: limpar função
public class LocalEmbeddingClient : IEmbeddingClient, IInitializable, IDisposable
{
    private readonly Lazy<InferenceSession>? _onnxSessionLazy;
    private readonly Lazy<Tokenizer>? _tokenizerLazy;
    private const int MaximumSequenceLength = 256;

    public LocalEmbeddingClient(string embeddingModel)
    {
        var modelDirectoryPath = Path.Combine(AppContext.BaseDirectory, "EmbeddingModels",embeddingModel);
        var modelPath = Path.Combine(modelDirectoryPath, "model.onnx");
        var tokenizerPath = Path.Combine(modelDirectoryPath, "tokenizer.json");

        var options = new SessionOptions();
        options.AppendExecutionProvider_CPU();

        _onnxSessionLazy = new Lazy<InferenceSession>(() => new InferenceSession(modelPath, options));
        _tokenizerLazy = new Lazy<Tokenizer>(() => Tokenizer.FromFile(tokenizerPath));
    }

    public void Dispose()
    {
        if (_onnxSessionLazy?.IsValueCreated == true)
            _onnxSessionLazy.Value.Dispose();
    }

    public Task InitializeAsync()
    {
        var session = _onnxSessionLazy!.Value;
        var tokenizer = _tokenizerLazy!.Value;
        Console.WriteLine("- Modelo NLP carregado com sucesso!");
        
        return Task.CompletedTask;
    }


    public Task<double[][]> GetSentenceEmbeddingAsync(string[] sentences)
    {
        var tokenizer = _tokenizerLazy!.Value;
        var session = _onnxSessionLazy!.Value;

        int batchSize = sentences.Length;

        // Prepara tensores de entrada
        var inputIds = CreateInputTensor(sentences, tokenizer, batchSize, MaximumSequenceLength, idSelector: id => (long)id);
        var attentionMask = CreateInputTensor(sentences, tokenizer, batchSize, MaximumSequenceLength, idSelector: _ => 1L);
        var tokenTypeIds = CreateInputTensor(sentences, tokenizer, batchSize, MaximumSequenceLength, idSelector: _ => 0L);

        var inputs = new List<NamedOnnxValue>
        {
            NamedOnnxValue.CreateFromTensor("input_ids", inputIds),
            NamedOnnxValue.CreateFromTensor("attention_mask", attentionMask),
        };

        // Executa inferência
        using var results = session.Run(inputs);
        double[] rawOutput = results.First().AsEnumerable<float>().Select(f => (double)f).ToArray();

        // Extrai e normaliza embeddings
        return Task.FromResult(ExtractEmbeddings(rawOutput, batchSize));
    }

    private DenseTensor<long> CreateInputTensor(
    string[] sentences,
    Tokenizer tokenizer,
    int batchSize,
    int maxLength,
    Func<uint, long> idSelector)
    {
        var tensor = new long[batchSize * maxLength];

        for (int i = 0; i < batchSize; i++)
        {
            var encoding = tokenizer.Encode(sentences[i], addSpecialTokens: false).First();
            var selectedIds = encoding.Ids.Select(idSelector).ToArray();
            var padded = PadOrTruncate(selectedIds, maxLength);

            for (int j = 0; j < maxLength; j++)
            {
                tensor[i * maxLength + j] = padded[j];
            }
        }

        return new DenseTensor<long>(tensor, new[] { batchSize, maxLength });
    }

    private double[][] ExtractEmbeddings(double[] rawOutput, int batchSize)
    {
        int hiddenSize = rawOutput.Length / batchSize;
        var embeddings = new double[batchSize][];

        for (int i = 0; i < batchSize; i++)
        {
            var vec = new double[hiddenSize];
            Array.Copy(rawOutput, i * hiddenSize, vec, 0, hiddenSize);

            // L2 Normalização
            double norm = Math.Sqrt(vec.Sum(x => x * x));
            if (norm > 0)
            {
                for (int k = 0; k < hiddenSize; k++)
                    vec[k] /= norm;
            }

            embeddings[i] = vec;
        }

        return embeddings;
    }


    public float CosineSimilarity(double[] firstVector, double[] secondVector)
    {
        double dotProductSum = 0.0;
        double firstVectorNormSquared = 0.0;
        double secondVectorNormSquared = 0.0;

        for (int dimensionIndex = 0; dimensionIndex < firstVector.Length; dimensionIndex++)
        {
            dotProductSum += firstVector[dimensionIndex] * secondVector[dimensionIndex];
            firstVectorNormSquared += firstVector[dimensionIndex] * firstVector[dimensionIndex];
            secondVectorNormSquared += secondVector[dimensionIndex] * secondVector[dimensionIndex];
        }

        return (float)(dotProductSum / (Math.Sqrt(firstVectorNormSquared) * Math.Sqrt(secondVectorNormSquared)));
    }

    private static long[] PadOrTruncate(long[] arr, int maxLength)
    {
        if (arr.Length > maxLength) return arr.Take(maxLength).ToArray();
        if (arr.Length == maxLength) return arr;
        return arr.Concat(new long[maxLength - arr.Length]).ToArray();
    }
}