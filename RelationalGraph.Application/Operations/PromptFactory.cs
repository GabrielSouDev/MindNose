namespace RelationalGraph.Application.Operations
{
    public static class PromptFactory
    {
        public static Prompt NewKnowledgeNode(string category, string term)
        {
            string prompt = @"Receba dois inputs:
                          termo: uma palavra ou conceito
                          categoria: o contexto ao qual o termo pertence
                          
                          Tarefa:
                          - Calcule um peso (de 0 a 1) que representa a relevância do termo dentro da categoria.
                          - Gere um resumo explicando o termo no contexto da categoria.
                          - Identifique termos com base nesse resumo relacionado a categoria.
                          - Se não encontrar termos relacionados relevantes, retorne apenas os mais próximossemanticamente.
                          - Para cada termo relacionado, calcule seu peso em relação a categoria (de 0 a 1).
                          - Para cada termo relacionado, calcule seu peso em relação ao termo original (de 0 a 1).
                          
                          Retorne no formato:
                          {{
                            ""Category"": ""<categoria>"",
                            ""Term"": ""<termo>"",
                            ""Summary"": ""<explicação do termo dentro da categoria>"",
                            ""WeigthCategoryToTerm"": 0.0,     
                            ""RelatedTerms"": [
                              {{""Term"": ""<termo_relacionado_1>"", ""WeigthCategoryToTerm"": 0.0, ""WeigthTermToTerm"": 0.0}},
                              {{""Term"": ""<termo_relacionado_2>"", ""WeigthCategoryToTerm"": 0.0, ""WeigthTermToTerm"": 0.0}},
                              {{""Term"": ""<termo_relacionado_3>"", ""WeigthCategoryToTerm"": 0.0, ""WeigthTermToTerm"": 0.0}}
                            ]
                          }}
                          
                          Responda estritamente no formato JSON como mostrado acima.
                          Input real: termo: {0} categoria: {1}";

            var prompMessage = string.Format(prompt, term, category);
            return new Prompt(prompMessage);
        }
    }
}