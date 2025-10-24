using MindNose.Domain.CMDs;
using MindNose.Domain.Request;
using MindNose.Domain.Results;
using Neo4j.Driver;

namespace MindNose.Domain.Operations
{
    public static class PromptFactory
    {
        public static Prompt NewTermResult(LinksRequest request)
        {
            string prompt = $@"
                Receba tres inputs:
                - Categoria: contexto de dominio.
                - Explicação de Categoria: explicação do contexto de dominio.
                - Termo: uma palavra ou conceito dentro do dominio categoria.

                Tarefa:
                - Referente ao Termo Inicial:
                      * Em Português do Brasil gere um resumo explicando o Termo no contexto da Categoria.
                - Determine no minimo {request.RelatedTermQuantity} Termos relacionados ao resumo gerado anteriormente sem ambiguidades dentro do mesmo dominio.

                Formato esperado:
                {{
                  ""Category"": ""<categoria>"",
                  ""CategorySummary"": ""<Explicação de Categoria>"",
                  ""Term"": ""<termo>"",
                  ""TermSummary"": ""<explicação do termo gerado>"",
                  ""RelatedTerms"": 
                    [
                        ""<termo relacionado gerado>"",
                        ""<termo relacionado gerado>""
                        ...
                    ]
                }}

                * Retorne estritamente um único JSON, iniciando com '{{' e terminando com '}}',
                Sem texto adicional fora do JSON,
                Use exatamente os nomes das chaves mostrados.

                Input real:
                Categoria: {request.Category},
                Explicação de Categoria: {request.GetCategorySummary()},
                Termo: {request.Term}";
            return new Prompt(prompt);
        }

        public static Prompt NewParcialTermResult(LinksRequest request)
        {
            string prompt = $@"Receba dois inputs:
                            Categoria: o contexto ao qual o termo pertence
                            Termo: uma palavra ou conceito
                          
                            Tarefa:
                            - Gere um resumo explicando o Termo no contexto da Categoria.
                          
                            Retorne no formato:
                            {{
                              ""Category"": ""<categoria>"",
                              ""Term"": ""<termo>"",
                              ""Summary"": ""<explicação do termo dentro da categoria>""
                            }}
                          
                            * Responda estritamente no formato JSON como mostrado acima.

                            Input real: Categoria: {request.Category} Termo: {request.Term}";

            return new Prompt(prompt);
        }

        internal static Prompt NewCategoryResult(string category)
        {
            string prompt = $@"Receba um input:
                            Categoria;
                          
                            Tarefa:
                            - Gere um resumo explicando a definição de Categoria.
                          
                            Retorne no formato:
                            {{
                              ""Category"": ""<categoria>"",
                              ""Summary"": ""<explicação de categoria>""
                            }}
                          
                            * Responda estritamente no formato JSON como mostrado acima.

                            Input real: Categoria: {category}";

            return new Prompt(prompt);
        }

        public static Prompt NewRelatedTermSummaries(LinksResult linksResult)
        {
            var relatedTerms = linksResult.RelatedTerms.Select(rt => rt.Title).ToList();

            var prompt = @"Tarefa:
                            - Com base no JSON abaixo, gere uma explicação do RelatedTerm e insira no campo RelatedTermSummary.
                            - Tenha como base o contexto:";
            prompt += "\n";
            prompt += $"{linksResult.Category.Title}: {linksResult.Category.Summary}\n";
            prompt += "e com o contexto principal:";
            prompt += $"{linksResult.Term.Title}: {linksResult.Term.Summary}";
            prompt += "\n\n";

            prompt += "Formato esperado (responda **estritamente** neste formato JSON):\n";
            prompt += "[\n";
            foreach (var relatedTerm in relatedTerms)
            {
                prompt += $"  {{\n";
                prompt += $"    \"RelatedTerm\": \"{relatedTerm}\",\n";
                prompt += $"    \"RelatedTermSummary\": \"<explicação gerada>\"\n";
                prompt += $"  }},\n";
            }
            prompt = prompt.TrimEnd(',', '\n') + "\n]";

            return new Prompt(prompt);
        }
    }
}