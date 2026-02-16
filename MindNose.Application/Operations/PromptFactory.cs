using MindNose.Domain.CMDs;
using MindNose.Domain.IAChat;
using MindNose.Domain.Request;
using MindNose.Domain.Results;
using Neo4j.Driver;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using Tokenizers.HuggingFace.Decoders;

namespace MindNose.Domain.Operations
{
    public static class PromptFactory
    {
        public static Prompt NewTermResult(LinksRequest request)
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

        public static Prompt NewCategoryResult(string category)
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

        public static Prompt NewRelatedTermSummaries(string category, IEnumerable<string> relatedTerms)
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

        public static Prompt SendAIChat(ChatRequest request)
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