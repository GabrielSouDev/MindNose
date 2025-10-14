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

        public async Task<TermResult> MakeEmbeddingAsync(TermResult termResult)
        {
            List<string> sentences = new(); 

            sentences.Add(termResult.Category);
            sentences.Add(termResult.Term);
            sentences.AddRange(termResult.RelatedTerms.Select(rt => rt.Term).ToList());

            var sentenceEmbeddings = await _embeddingClient.GetSentenceEmbedding(sentences.ToArray());

            var similarityCategoryToTerm = _embeddingClient.CosineSimilarity(sentenceEmbeddings[0], sentenceEmbeddings[1]);
            termResult.CategoryToTermWeigth = similarityCategoryToTerm;

            var relatedTermsEmbeddings = sentenceEmbeddings.Skip(2).ToArray();

            var RelatedTermsList = new List<RelatedTerm>();
            for (int candidateIndex = 0; candidateIndex < relatedTermsEmbeddings.Length; candidateIndex++)
            {
                var similarityRelatedTermToCategory =
                    _embeddingClient.CosineSimilarity(relatedTermsEmbeddings[candidateIndex], sentenceEmbeddings[0]);

                var similarityRelatedTermToTerm = 
                    _embeddingClient.CosineSimilarity(relatedTermsEmbeddings[candidateIndex], sentenceEmbeddings[1]);

                termResult.RelatedTerms[candidateIndex].CategoryToRelatedTermWeigth = similarityRelatedTermToCategory;
                termResult.RelatedTerms[candidateIndex].InitialTermToRelatedTermWeigth = similarityRelatedTermToTerm;
            }

            termResult.RelatedTerms = termResult.RelatedTerms
                .OrderByDescending(termScorePair => termScorePair.InitialTermToRelatedTermWeigth)
                .Take(10)
                .ToList();

            return termResult;
        }
    }
}
