using MindNose.Domain.Interfaces.Clients;
using MindNose.Domain.Interfaces.Services;
using MindNose.Domain.Results;

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
            List<string> sentences = new(); 

            sentences.Add($"{termResult.Category.Title}: {termResult.Category.Summary}");
            sentences.Add($"{termResult.Term.Title}: {termResult.Term.Summary}");
            sentences.AddRange(termResult.RelatedTerms.Select(rt => $"{rt.Title}: {rt.Summary}").ToList());

            var sentenceEmbeddings = await _embeddingClient.GetSentenceEmbeddingAsync(sentences.ToArray());

            var similarityCategoryToTerm = _embeddingClient.CosineSimilaritySIMD(sentenceEmbeddings[0], sentenceEmbeddings[1]);
            termResult.CategoryToTermWeigth = similarityCategoryToTerm;

            var relatedTermsEmbeddings = sentenceEmbeddings.Skip(2).ToArray();

            var RelatedTermsList = new List<RelatedTermResult>();
            for (int candidateIndex = 0; candidateIndex < relatedTermsEmbeddings.Length; candidateIndex++)
            {
                var similarityRelatedTermToCategory =
                    _embeddingClient.CosineSimilaritySIMD(relatedTermsEmbeddings[candidateIndex], sentenceEmbeddings[0]);

                var similarityRelatedTermToTerm = 
                    _embeddingClient.CosineSimilaritySIMD(relatedTermsEmbeddings[candidateIndex], sentenceEmbeddings[1]);

                termResult.RelatedTerms[candidateIndex].CategoryToRelatedTermWeigth = similarityRelatedTermToCategory;
                termResult.RelatedTerms[candidateIndex].InitialTermToRelatedTermWeigth = similarityRelatedTermToTerm;
            }

            termResult.RelatedTerms = termResult.RelatedTerms
                .OrderByDescending(termScorePair => termScorePair.InitialTermToRelatedTermWeigth)
                .ToList();

            return termResult;
        }
    }
}
