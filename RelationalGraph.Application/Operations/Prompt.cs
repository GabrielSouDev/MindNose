namespace RelationalGraph.Application.Operations
{
    public class Prompt
    {
        public Prompt(string message)
        {
            Message = message;
        }
        public string Message { get; private set; }
        public static Prompt CreateKnowledgeNode(string category, string term)
        {
            string prompt = @"Receba dois inputs:
                          termo: uma palavra ou conceito
                          categoria: o contexto ao qual o termo pertence
                          
                          Tarefa:
                          - Calcule um peso (de 0 a 1) que representa a relevância do termo dentro da categoria.
                          - Gere um resumo explicando o termo no contexto da categoria.
                          - Identifique até 3 termos relacionados com base nesse resumo.
                          - Se não encontrar termos relacionados relevantes, retorne apenas os mais próximossemanticamente.
                          - Para cada termo relacionado, calcule seu peso em relação ao termo original (de 0 a 1).
                          
                          Retorne no formato:
                          {{
                            ""termo"": ""<termo>"",
                            ""resumo"": ""<explicação do termo dentro da categoria>"",
                            ""pesoTermoCategoria"": 0.0,
                            ""termosRelacionados"": [
                              {{""termo"": ""<termo_relacionado_1>"", ""peso"": 0.0}},
                              {{""termo"": ""<termo_relacionado_2>"", ""peso"": 0.0}},
                              {{""termo"": ""<termo_relacionado_3>"", ""peso"": 0.0}}
                            ]
                          }}
                          
                          Responda estritamente no formato JSON como mostrado acima.
                          Input real: termo: {0} categoria: {1}";
            Console.WriteLine(prompt);
            Console.WriteLine(term);
            Console.WriteLine(category);
            var prompMessage = string.Format(prompt, term, category);
            return new Prompt(prompMessage);
        }
    }
}