using MindNose.Domain.Interfaces.Clients;
using MindNose.Domain.Interfaces.Services;
using MindNose.Domain.Results;

namespace MindNose.Domain.Services
{
    public class LocalEmbeddingService : IEmbeddingService
    {
        private readonly IEmbeddingClient _embeddingClient;

        public LocalEmbeddingService(IEmbeddingClient embeddingClient)
        {
            _embeddingClient = embeddingClient;
        }

        public Task<TermResult> MakeEmbeddingAsync(TermResult termResult)
        {
            throw new NotImplementedException();
        }

        //public Task<TermResult> MakeEmbeddingAsync(TermResult termResult)
        //{
        //    var category = termResult.Category;
        //    var term = termResult.Term;

        //    var relatedTerms = termResult.RelatedTerms.Select(rt => rt.Term).ToArray();

        //    var categoryEmbeddingVector = _embeddingClient.GetSentenceEmbedding(category);
        //    var initialTermEmbeddingVector = _embeddingClient.GetSentenceEmbedding(term);
        //    var candidateTermsEmbeddingsMatrix = _embeddingClient.GetSentenceEmbeddings(relatedTerms);

        //    var similarityCategoryToTerm = _embeddingClient.CosineSimilarity(categoryEmbeddingVector, initialTermEmbeddingVector);

        //    termResult.CategoryToTermWeigth = similarityCategoryToTerm;

        //    var RelatedTermsList = new List<RelatedTerm>();
        //    for (int candidateIndex = 0; candidateIndex < relatedTerms.Length; candidateIndex++)
        //    {
        //        var similarityRelatedTermToCategory =
        //            _embeddingClient.CosineSimilarity(candidateTermsEmbeddingsMatrix[candidateIndex], categoryEmbeddingVector);

        //        var similarityRelatedTermToTerm = 
        //            _embeddingClient.CosineSimilarity(candidateTermsEmbeddingsMatrix[candidateIndex], initialTermEmbeddingVector);

        //        var relatedTerm = new RelatedTerm()
        //        {
        //            Term = relatedTerms[candidateIndex],
        //            CategoryToRelatedTermWeigth = similarityRelatedTermToCategory,
        //            InitialTermToRelatedTermWeigth = similarityRelatedTermToTerm,
        //            CreatedAt = termResult.RelatedTerms[candidateIndex].CreatedAt
        //        };

        //        RelatedTermsList.Add(relatedTerm);
        //    }

        //    termResult.RelatedTerms = RelatedTermsList
        //        .OrderByDescending(termScorePair => termScorePair.InitialTermToRelatedTermWeigth)
        //        .Take(10)
        //        .ToList();

        //    return termResult;
        //}
    }
}
