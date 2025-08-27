using MindNose.Domain.CMDs;
using MindNose.Domain.Request;
using Neo4j.Driver;

namespace MindNose.Domain.Operations
{
    public static class PromptFactory
    {
        public static Prompt NewKnowledgeNode(LinksRequest request)
        {
            string prompt = @"Receba dois inputs:
                          Termo: uma palavra ou conceito.
                          Categoria: o contexto ao qual o termo pertence.
                          
                          Tarefa:
                          - Referente ao Termo Inicial:
                                * Em Português do Brasil gere um resumo explicando o Termo no contexto da Categoria.
                                * Calcule um peso entre o Termo e a Categoria (de 0.00 a 1.00).
                          - Identifique Termos relacionados e presentes neste Resumo.
                          - Para cada termo relacionado no contexto de Categoria:
                                * Calcule um peso entre a Cagegoria (de 0.00 a 1.00).
                                * Calcule um peso entre o Termo Inicial (de 0.00 a 1.00).
                          
                          Retorne no formato:
                          {{
                            ""Category"": ""<categoria>"",
                            ""Term"": ""<termo>"",
                            ""Summary"": ""<explicação do termo dentro da categoria>"",
                            ""WeigthCategoryToInitialTerm"": 0.0,     
                            ""RelatedTerms"": [
                              {{""Term"": ""<termo_relacionado_1>"", ""WeigthCategoryToRelatedTerm"": 0.0, ""WeigthInitialTermToRelatedTerm"": 0.0}},
                              {{""Term"": ""<termo_relacionado_2>"", ""WeigthCategoryToRelatedTerm"": 0.0, ""WeigthInitialTermToRelatedTerm"": 0.0}},
                              {{""Term"": ""<termo_relacionado_3>"", ""WeigthCategoryToRelatedTerm"": 0.0, ""WeigthInitialTermToRelatedTerm"": 0.0}}
                            ]
                          }}
                          
                          Responda estritamente no formato JSON como mostrado acima.
                          Input real: Categoria: {0} Termo: {1}";

            var prompMessage = string.Format(prompt, request.Category, request.Term);
            return new Prompt(prompMessage);
        }

        public static Prompt NewKnowledgeSummary(LinksRequest request)
        {
            string prompt = @"Receba dois inputs:
                          Categoria: o contexto ao qual o termo pertence
                          Termo: uma palavra ou conceito
                          
                          Tarefa:
                          - Gere um resumo explicando o Termo no contexto da Categoria.
                          
                          Retorne no formato:
                          {{
                            ""Category"": ""<categoria>"",
                            ""Term"": ""<termo>"",
                            ""Summary"": ""<explicação do termo dentro da categoria>""
                            ]
                          }}
                          
                          Responda estritamente no formato JSON como mostrado acima.
                          Input real: Categoria: {0} Termo: {1}"
            ;

            var prompMessage = string.Format(prompt, request.Category, request.Term);
            return new Prompt(prompMessage);
        }
    }
}