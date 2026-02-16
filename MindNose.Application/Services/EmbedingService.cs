using MindNose.Domain.Interfaces.Clients;
using MindNose.Domain.Interfaces.Services;
using MindNose.Domain.Results;
using Tokenizers.HuggingFace.Tokenizer;

namespace MindNose.Domain.Services
{
    public class EmbedingService : IEmbeddingService
    {
        private readonly IEmbeddingClient _embeddingClient;

        public EmbedingService(IEmbeddingClient embeddingClient)
        {
            _embeddingClient = embeddingClient;
        }

        public async Task<LinksResult> MakeEmbeddingAsync(LinksResult termResult)
        {
            var sentences = new List<string>();
            int offset = 0;

            if (termResult.Category.Embedding is null)
            {
                sentences.Add($"{termResult.Category.Title}: \n{termResult.Category.GetSummary()}");
                offset = 1;
            }

            sentences.Add($"{termResult.Term.Title}: \n{termResult.Term.GetSummary()}");
            sentences.AddRange(termResult.RelatedTerms.Select(rt => $"{rt.Title}: \n{rt.GetSummary()}"));

            var sentenceEmbeddings = await _embeddingClient.GetSentenceEmbeddingAsync(sentences.ToArray());

            if (termResult.Category.Embedding is null)
                termResult.Category.Embedding = sentenceEmbeddings[0].ToList();

            termResult.Term.Embedding = sentenceEmbeddings[offset].ToList();

            termResult.Term.CategoryToTermWeigth = _embeddingClient.CosineSimilaritySIMD(
                termResult.Category.Embedding.ToArray(),
                termResult.Term.Embedding.ToArray()
            );

            var relatedTermsEmbeddings = sentenceEmbeddings.Skip(offset + 1).ToArray();

            for (int i = 0; i < relatedTermsEmbeddings.Length; i++)
            {
                termResult.RelatedTerms[i].Embedding = relatedTermsEmbeddings[i].ToList();

                termResult.RelatedTerms[i].CategoryToRelatedTermWeigth =
                    _embeddingClient.CosineSimilaritySIMD(relatedTermsEmbeddings[i], termResult.Category.Embedding.ToArray());

                termResult.RelatedTerms[i].InitialTermToRelatedTermWeigth =
                    _embeddingClient.CosineSimilaritySIMD(relatedTermsEmbeddings[i], termResult.Term.Embedding.ToArray());
            }

            termResult.RelatedTerms = termResult.RelatedTerms
                .OrderByDescending(rt => rt.InitialTermToRelatedTermWeigth)
                .ToList();

            return termResult;
        }
    }
}
