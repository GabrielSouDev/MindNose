using MindNose.Domain.CMDs;
using MindNose.Domain.IAChat;
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
                Explicação de Categoria: {request.CategorySummary},
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
            var relatedTerms = linksResult.RelatedTerms.Select(rt => rt.TitleId).ToList();

            var prompt = @"Tarefa:
                            - Com base no JSON abaixo, gere uma explicação do RelatedTerm e insira no campo RelatedTermSummary.
                            - Tenha como base o contexto:";
            prompt += "\n";
            prompt += $"{linksResult.Category.TitleId}: {linksResult.Category.Summary}\n";
            prompt += "e com o contexto principal:";
            prompt += $"{linksResult.Term.TitleId}: {linksResult.Term.Summary}";
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

        internal static Prompt SendAIChat(ChatRequest request)
        {
            string prompt = $@"
                Você é um redator técnico especializado. 
                Sua tarefa é escrever um artigo completo e coeso que responda diretamente à pergunta central abaixo, utilizando os dados técnicos que fornecerei    em         seguida     como base argumentativa.
                
                ### Objetivo:
                - Produzir um artigo com **estrutura narrativa fluida**, que conecte os conceitos apresentados
                - **Responder à pergunta central** com clareza, profundidade e embasamento técnico
                - Utilizar os dados fornecidos como **referência para construir os argumentos**
                
                ### Estrutura esperada:
                1. **Título atrativo e relevante**
                2. **Introdução envolvente**, que contextualize o tema e antecipe a pergunta central
                3. **Desenvolvimento contínuo**, com parágrafos conectados e uso dos dados técnicos como base
                4. **Conclusão reflexiva**, que sintetize os principais pontos e responda à pergunta central
                
                ### Dados técnicos:
                Os dados virão no formato:
                `Título: Resumo`
                
                Utilize esses dados como base para construir o artigo. Não os repita como tópicos isolados — integre-os ao texto de forma natural e explicativa.
                
                ";

            if (request.ElementsHeader?.Count > 0)
            {
                foreach (var element in request.ElementsHeader)
                {
                    prompt += $"{element.Title}: {element.Summary}\n";
                }
            }

            prompt += "### Pergunta central: \n";
            prompt += request.Message.Text + "\n";

            prompt += @"Escreva o artigo com linguagem profissional, objetiva e fluida. 
                Evite listas ou tópicos desconectados. Construa uma narrativa que una os conceitos e leve à resposta da pergunta central.";

            return new Prompt(prompt);

        }
    }
}