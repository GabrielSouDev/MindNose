using MindNose.Domain.CMDs;
using MindNose.Domain.Request;

namespace MindNose.Domain.Operations;

public class PromptNodeFactory
{
    public Prompt NewTermResult(LinksRequest request)
    {
        string prompt = $@"Você é um especialista técnico na área de {request.Category}. 
            Para o termo ""{request.Term}"", gere uma resposta estruturada com os seguintes campos:

            Campos obrigatórios:
            1. CanonicalDefinition: Definição concisa em 1–2 frases.
            2. MainFunction: Função ou propósito principal em 1–2 frases.
            3. ConceptualCategory: Categoria conceitual a que o termo pertence (ex.: ""Linguagem de programação"", ""Estrutura de dados"").
            4. RelatedTerms: Array JSON com {request.RelatedTermQuantity} termos relacionados.

            Regras:
            - Todos os campos devem ser curtos e precisos.
            - Não incluir exemplos, histórico ou comparações.
            - Saída obrigatoriamente no formato JSON:

            {{
              ""CanonicalDefinition"": ""..."",
              ""MainFunction"": ""..."",
              ""ConceptualCategory"": ""..."",
              ""RelatedTerms"": [""Term1"", ""Term2"", ""Term3""]
            }}";


        return new Prompt(prompt);
    }

    public Prompt NewCategoryResult(string category)
    {

        string prompt =$@"Você é um especialista em conceitos gerais.
            Para a categoria geral ""{category}"", gere uma resposta estruturada com os seguintes campos:

            1.Definição: Uma ou duas frases explicando claramente o que esta categoria abrange de forma conceitual.
            2.Função: Uma frase explicando o propósito principal ou objetivo desta categoria.

            Requisitos:
            -Mantenha os campos curtos, claros e objetivos.
            - Não inclua exemplos, termos específicos ou histórico.
            - Saída obrigatoriamente no seguinte formato Json:
            {{
              ""Definition"": ""..."",
              ""Function"": ""...""
            }}";

        return new Prompt(prompt);
    }

    public Prompt NewRelatedTermSummaries(string category, IEnumerable<string> relatedTerms)
    {
        string prompt = $@"Você é um especialista técnico na área de {category}.
            Para os seguintes termos: {string.Join(", ", relatedTerms)}.
            
            Gere uma resposta estruturada com os seguintes Campos obrigatórios:
            1. CanonicalDefinition: Definição concisa em 1–2 frases.
            2. MainFunction: Função ou propósito principal em 1–2 frases.
            3. ConceptualCategory: Categoria conceitual a que o termo pertence (ex.: ""Linguagem de programação"", ""Estrutura de dados"").

            Regras:
            - Todos os campos devem ser curtos e precisos.
            - Não incluir exemplos, histórico ou comparações.
            - Saída obrigatoriamente no formato JSON:
            [
                {{
                  ""Title"": ""Term1"",
                  ""CanonicalDefinition"": ""..."",
                  ""MainFunction"": ""..."",
                  ""ConceptualCategory"": ""...""
                }},
                {{
                  ""Title"": ""Term2"",
                  ""CanonicalDefinition"": ""..."",
                  ""MainFunction"": ""..."",
                  ""ConceptualCategory"": ""...""
                }}
            ]";

        return new Prompt(prompt);
    }
}