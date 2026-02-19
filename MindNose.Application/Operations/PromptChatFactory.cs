using MindNose.Domain.CMDs;
using MindNose.Domain.Enums;
using MindNose.Domain.IAChat;
using System.Text;

namespace MindNose.Domain.Operations;

public class PromptChatFactory
{
    private readonly Dictionary<OutputMode, Func<ChatRequest, string>> _strategies;
    private readonly ConversationGuideRepository _conversationGuideRepository;

    public PromptChatFactory(ConversationGuideRepository conversationGuideRepository)
    {
        _conversationGuideRepository = conversationGuideRepository;
        _strategies = new()
        {
            { OutputMode.Formal, AIChatFormal },
            { OutputMode.Article, AIChatArticle },
            { OutputMode.Summary, AIChatSummary },
            { OutputMode.Conversational, AIChatConversational },
            { OutputMode.Technical, AIChatTechnical },
            { OutputMode.Creative, AIChatCreative },
            { OutputMode.Code, AIChatCode }
        };
    }

    public Prompt SendAIChat(ChatRequest request)
    {
        if (_strategies.TryGetValue(request.OutputMode, out var strategy)) 
        { 
            return new Prompt(strategy(request)); 
        }
        
        return new Prompt(AIChatInformal(request));
    }

    private string AIChatInformal(ChatRequest request)
    {
        var sb = new StringBuilder();

        sb.AppendLine("### PAPEL");
        sb.AppendLine("Você é um redator simpático e informal, capaz de explicar assuntos de forma leve, clara e próxima do leitor.");
        sb.AppendLine();

        sb.AppendLine("### OBJETIVO");
        sb.AppendLine("Responda diretamente à MENSAGEM do usuário de forma clara e descontraída, usando um tom amistoso.");
        sb.AppendLine("Use os dados técnicos apenas quando forem relevantes para a compreensão, sem forçar sua inclusão.");
        sb.AppendLine();

        sb.AppendLine("### REGRAS");
        sb.AppendLine("- Linguagem leve, próxima e fácil de entender.");
        sb.AppendLine("- Evitar termos excessivamente técnicos ou jargões.");
        sb.AppendLine("- Integrar dados apenas se contribuírem para a resposta.");
        sb.AppendLine("- Evitar listas, bullet points ou tópicos desconectados.");
        sb.AppendLine("- Não repetir saudações, introduções genéricas ou convites para perguntar.");
        sb.AppendLine();

        sb.AppendLine("<MENSAGEM>");
        sb.AppendLine("Mensagem do usuário:");
        sb.AppendLine(request.Message.Text);
        sb.AppendLine("</MENSAGEM>");
        sb.AppendLine();

        var context = ExtractContext(request);
        if (!string.IsNullOrEmpty(context))
        {
            sb.AppendLine(context);
            sb.AppendLine();
        }

        sb.AppendLine("Responda diretamente à MENSAGEM do usuário de forma natural, informal e envolvente, sem introduções genéricas.");

        return sb.ToString();
    }

    private string AIChatCode(ChatRequest request)
    {
        var sb = new StringBuilder();

        sb.AppendLine("### PAPEL");
        sb.AppendLine("Você é um especialista em programação e desenvolvimento de software.");
        sb.AppendLine();

        sb.AppendLine("### OBJETIVO");
        sb.AppendLine("Forneça respostas com exemplos de código precisos e funcionais.");
        sb.AppendLine("Sempre use blocos de código Markdown com a linguagem correta.");
        sb.AppendLine();

        sb.AppendLine("### REGRAS");
        sb.AppendLine("- Incluir exemplos de código claros e comentados.");
        sb.AppendLine("- Usar ```csharp``` ou ```bash``` etc. para formatar.");
        sb.AppendLine("- Evitar informações irrelevantes.");
        sb.AppendLine();

        sb.AppendLine("<MENSAGEM>");
        sb.AppendLine(request.Message.Text);
        sb.AppendLine("</MENSAGEM>");
        sb.AppendLine();

        var context = ExtractContext(request);
        if (!string.IsNullOrEmpty(context))
        {
            sb.AppendLine(context);
            sb.AppendLine();
        }

        sb.AppendLine("Responda apenas com código e explicação técnica mínima.");

        return sb.ToString();
    }

    private string AIChatCreative(ChatRequest request)
    {
        var sb = new StringBuilder();

        sb.AppendLine("### PAPEL");
        sb.AppendLine("Você é um redator criativo e imaginativo, capaz de escrever com estilo literário e envolvente.");
        sb.AppendLine();

        sb.AppendLine("### OBJETIVO");
        sb.AppendLine("Produza uma resposta criativa, original e cativante.");
        sb.AppendLine("Use os dados técnicos apenas quando forem relevantes e integrados de forma natural à narrativa.");
        sb.AppendLine();

        sb.AppendLine("### REGRAS");
        sb.AppendLine("- Linguagem expressiva, imaginativa e envolvente.");
        sb.AppendLine("- Integrar informações técnicas de forma sutil.");
        sb.AppendLine("- Evitar listas ou estrutura rígida.");
        sb.AppendLine("- Explorar metáforas, analogias ou narrativas inventivas quando apropriado.");
        sb.AppendLine();

        sb.AppendLine("<MENSAGEM>");
        sb.AppendLine("Mensagem do usuário:");
        sb.AppendLine(request.Message.Text);
        sb.AppendLine("</MENSAGEM>");
        sb.AppendLine();

        var context = ExtractContext(request);
        if (!string.IsNullOrEmpty(context))
        {
            sb.AppendLine(context);
            sb.AppendLine();
        }

        sb.AppendLine("Responda a MENSAGEM do usuário de forma imaginativa e cativante.");

        return sb.ToString();
    }

    private string AIChatTechnical(ChatRequest request)
    {
        var sb = new StringBuilder();

        sb.AppendLine("### PAPEL");
        sb.AppendLine("Você é um especialista técnico com conhecimento aprofundado, capaz de explicar conceitos de forma precisa e formal.");
        sb.AppendLine();

        sb.AppendLine("### OBJETIVO");
        sb.AppendLine("Fornecer uma resposta técnica detalhada e rigorosa.");
        sb.AppendLine("Use os dados técnicos apenas quando forem relevantes, integrando-os naturalmente à explicação.");
        sb.AppendLine("Evite estrutura narrativa de artigo (título, introdução ou conclusão).");
        sb.AppendLine();

        sb.AppendLine("### REGRAS");
        sb.AppendLine("- Linguagem formal, objetiva e clara.");
        sb.AppendLine("- Não inventar informações ou exemplos fictícios.");
        sb.AppendLine("- Integrar dados técnicos apenas se forem úteis para a explicação.");
        sb.AppendLine("- Priorizar profundidade, detalhamento e precisão.");
        sb.AppendLine();

        sb.AppendLine("<MENSAGEM>");
        sb.AppendLine("Mensagem do usuário:");
        sb.AppendLine(request.Message.Text);
        sb.AppendLine("</MENSAGEM>");
        sb.AppendLine();

        var context = ExtractContext(request);
        if (!string.IsNullOrEmpty(context))
        {
            sb.AppendLine(context);
            sb.AppendLine();
        }

        sb.AppendLine("Responda a MENSAGEM do usuário de forma técnica, aprofundada e objetiva.");

        return sb.ToString();
    }

    private string AIChatConversational(ChatRequest request)
    {
        var sb = new StringBuilder();

        sb.AppendLine("### PAPEL");
        sb.AppendLine("Você é um assistente conversacional, capaz de interagir de forma natural e fluida.");
        sb.AppendLine();

        sb.AppendLine("### OBJETIVO");
        sb.AppendLine("Responda à MENSAGEM mantendo um fluxo de diálogo natural.");
        sb.AppendLine("Use os dados técnicos apenas se forem realmente necessários para esclarecer ou enriquecer a conversa.");
        sb.AppendLine();

        sb.AppendLine("### REGRAS");
        sb.AppendLine("- Linguagem natural, clara e amigável.");
        sb.AppendLine("- Respostas objetivas e diretas, evitando longas divagações.");
        sb.AppendLine("- Não inventar informações.");
        sb.AppendLine("- Integrar dados técnicos apenas se relevantes para a conversa.");
        sb.AppendLine();

        sb.AppendLine("<MENSAGEM>");
        sb.AppendLine("Mensagem do usuário:");
        sb.AppendLine(request.Message.Text);
        sb.AppendLine("</MENSAGEM>");
        sb.AppendLine();

        var context = ExtractContext(request);
        if (!string.IsNullOrEmpty(context))
        {
            sb.AppendLine(context);
            sb.AppendLine();
        }

        sb.AppendLine("Responda a MENSAGEM do usuário de forma natural e envolvente, mantendo o tom de conversa.");

        return sb.ToString();
    }

    private string AIChatSummary(ChatRequest request)
    {
        var sb = new StringBuilder();

        sb.AppendLine("### PAPEL");
        sb.AppendLine("Você é um redator especializado em síntese e resumo de informações.");
        sb.AppendLine();

        sb.AppendLine("### OBJETIVO");
        sb.AppendLine("Produza um resumo conciso e claro da pergunta e do contexto fornecido.");
        sb.AppendLine("Use os dados técnicos apenas se forem relevantes para a síntese.");
        sb.AppendLine();

        sb.AppendLine("### REGRAS");
        sb.AppendLine("- Linguagem clara e objetiva.");
        sb.AppendLine("- Evitar detalhes irrelevantes.");
        sb.AppendLine("- Não inventar informações.");
        sb.AppendLine("- Manter fidelidade aos dados fornecidos.");
        sb.AppendLine();

        sb.AppendLine("<MENSAGEM>");
        sb.AppendLine("Mensagem do usuário:");
        sb.AppendLine(request.Message.Text);
        sb.AppendLine("</MENSAGEM>");
        sb.AppendLine();

        var context = ExtractContext(request);
        if (!string.IsNullOrEmpty(context))
        {
            sb.AppendLine(context);
            sb.AppendLine();
        }

        sb.AppendLine("Responda a MENSAGEM do usuário com um resumo conciso, mantendo clareza e fidelidade.");

        return sb.ToString();
    }

    private string AIChatArticle(ChatRequest request)
    {
        var sb = new StringBuilder();

        sb.AppendLine("### PAPEL");
        sb.AppendLine("Você é um redator técnico especialista em escrita analítica e argumentativa.");
        sb.AppendLine("Seu texto deve ser coeso, técnico, profundo e estruturado como um artigo profissional.");
        sb.AppendLine();

        sb.AppendLine("### OBJETIVO");
        sb.AppendLine("Produzir um artigo completo que responda com precisão à pergunta central.");
        sb.AppendLine("Use os dados técnicos fornecidos como base argumentativa quando forem relevantes.");
        sb.AppendLine("Caso algum dado não contribua diretamente para a resposta, não é obrigatório utilizá-lo.");
        sb.AppendLine();

        sb.AppendLine("### REGRAS OBRIGATÓRIAS");
        sb.AppendLine("- Não inventar dados técnicos específicos.");
        sb.AppendLine("- Limitar-se ao que pode ser inferido com segurança.");
        sb.AppendLine("- Não criar exemplos fictícios.");
        sb.AppendLine("- Não usar listas ou bullet points.");
        sb.AppendLine("- Integrar os dados naturalmente ao desenvolvimento do texto.");
        sb.AppendLine("- Manter linguagem profissional, objetiva e fluida.");
        sb.AppendLine();

        sb.AppendLine("### ESTRUTURA ESPERADA");
        sb.AppendLine("1. Título relevante e técnico");
        sb.AppendLine("2. Introdução contextualizando o tema");
        sb.AppendLine("3. Desenvolvimento contínuo e argumentativo");
        sb.AppendLine("4. Conclusão que responda explicitamente à MENSAGEM central");
        sb.AppendLine();

        sb.AppendLine("<MENSAGEM>");
        sb.AppendLine("Mensagem do usuário:");
        sb.AppendLine(request.Message.Text);
        sb.AppendLine("</MENSAGEM>");
        sb.AppendLine();

        var context = ExtractContext(request);
        if (!string.IsNullOrEmpty(context))
        {
            sb.AppendLine(context);
            sb.AppendLine();
        }

        sb.AppendLine("Escreva agora o artigo completo com base na MENSAGEM do usuário.");

        return sb.ToString();
    }

    private string AIChatFormal(ChatRequest request)
    {
        var sb = new StringBuilder();

        sb.AppendLine("### PAPEL");
        sb.AppendLine("Você é um redator profissional, capaz de escrever com tom formal e corporativo.");
        sb.AppendLine();

        sb.AppendLine("### OBJETIVO");
        sb.AppendLine("Responda à MENSAGEM com linguagem formal e objetiva.");
        sb.AppendLine("Use os dados técnicos apenas quando forem relevantes.");
        sb.AppendLine();

        sb.AppendLine("### REGRAS");
        sb.AppendLine("- Linguagem formal, clara e coesa.");
        sb.AppendLine("- Integrar dados apenas se forem relevantes.");
        sb.AppendLine("- Evitar linguagem coloquial ou exemplos fictícios.");
        sb.AppendLine("- Estrutura lógica e coesa.");
        sb.AppendLine();

        sb.AppendLine("<MENSAGEM>");
        sb.AppendLine("Mensagem do usuário:");
        sb.AppendLine(request.Message.Text);
        sb.AppendLine("</MENSAGEM>");
        sb.AppendLine();

        var context = ExtractContext(request);
        if (!string.IsNullOrEmpty(context))
        {
            sb.AppendLine(context);
            sb.AppendLine();
        }

        sb.AppendLine("Responda a MENSAGEM do usuário com tom formal, profissional e coeso.");

        return sb.ToString();
    }

    //private string ExtractConversationHistory(ChatRequest request)
    //{
    //    var sb = new StringBuilder();

    //    sb.AppendLine("<HISTORICO_CONVERSA>");
    //    sb.AppendLine("Use para manter coerência e continuidade temática.");
    //    sb.AppendLine("Não repita trechos literalmente.");
    //    sb.AppendLine();

    //    if (request.PreviousMessages?.Count > 0)
    //    {
    //        foreach (var msg in request.PreviousMessages)
    //        {
    //            sb.AppendLine($"{msg.Role}: {msg.Text}");
    //        }
    //    }

    //    sb.AppendLine("</HISTORICO_CONVERSA>");

    //    return sb.ToString();
    //}

    private string ExtractTechnicalData(ChatRequest request)
    {
        var sb = new StringBuilder();

        if (request.ElementsHeader?.Count > 0)
        {
            sb.AppendLine("<DADOS_TECNICOS>");
            sb.AppendLine("Use apenas quando forem úteis para a resposta.");
            sb.AppendLine();

            foreach (var element in request.ElementsHeader)
            {
                sb.AppendLine($"{element.Title}: {element.Summary}");
            }

            sb.AppendLine("</DADOS_TECNICOS>");
        }

        return sb.ToString();
    }

    private string ExtractContext(ChatRequest request)
    {
        var sb = new StringBuilder();

        var technicalData = ExtractTechnicalData(request);

        if (!string.IsNullOrEmpty(technicalData))
        {
            sb.AppendLine("<CONTEXTO>");
            sb.AppendLine(technicalData);
            sb.AppendLine("</CONTEXTO>");
        }

        return sb.ToString();
    }
}