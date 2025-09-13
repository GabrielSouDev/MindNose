using MindNose.Domain.CMDs;
using MindNose.Domain.Request;

namespace MindNose.Domain.Operations
{
    public static class PromptFactory
    {
        public static Prompt NewTermResult(LinksRequest request)
        {
            string prompt = $@"
                Receba dois inputs:
                - Categoria: o contexto de dominio.
                - Termo: uma palavra ou conceito dentro do dominio categoria.

                Tarefa:
                - Referente ao Termo Inicial:
                      * Em Português do Brasil gere um resumo explicando o Termo no contexto da Categoria.
                - Determine no minimo 10 Termos relacionados ao resumo gerado anteriormente sem ambiguidades dentro do mesmo dominio.

                Retorne estritamente um único JSON, iniciando com '{{' e terminando com '}}',
                sem texto adicional fora do JSON.
                Use exatamente os nomes das chaves mostrados.

                Formato esperado:
                {{
                  ""Category"": ""<categoria>"",
                  ""Term"": ""<termo>"",
                  ""Summary"": ""<explicação do termo dentro da categoria>"",
                  ""RelatedTerms"": 
                    [
                        ""<termo relacionado>"",
                        ""<termo relacionado>""
                        ...
                    ]
                }}

                Input real:
                Categoria: {request.Category}
                Termo: {request.Term}";
            return new Prompt(prompt);
        }

        public static Prompt NewTermResultOld(LinksRequest request)
        {
            string prompt = $@"
                Receba dois inputs:
                - Categoria: o contexto de dominio.
                - Termo: uma palavra ou conceito dentro do dominio categoria.

                Tarefa:
                - Referente ao Termo Inicial:
                      * Em Português do Brasil gere um resumo explicando o Termo no contexto da Categoria.
                      * Calcule um peso entre o Termo e a Categoria (de 0.00 a 1.00).
                - Identifique Termos relacionados e presentes neste Resumo.
                - Para cada termo relacionado no contexto da Categoria:
                      * Calcule um peso entre a Categoria (de 0.00 a 1.00).
                      * Calcule um peso entre o Termo Inicial (de 0.00 a 1.00).

                Retorne estritamente em um único objeto JSON, iniciando com '{{' e terminando com '}}',
                sem texto adicional fora do JSON.
                Use exatamente os nomes das chaves mostrados.
                O campo ""Term"" deve sempre estar presente e conter o termo inicial.

                Formato esperado:
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

                Input real:
                Categoria: {request.Category}
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
                              ]
                            }}
                          
                            Responda estritamente no formato JSON como mostrado acima.
                            Input real: Categoria: {request.Category} Termo: {request.Term}";

            return new Prompt(prompt);
        }
    }
}