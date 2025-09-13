# MindNose
Exploração recursiva de termos ligados a um dominio intitulado.

Tecnologias:
    - API - ASP.NET Core Web com controllers.

    - Banco de dados - Neo4j
        * banco de dados relacionado a grafos, utilizado para persistir nós e conexões de termos e categoria e seus "pesos".

    - IA - LLM via HTTPClient OpenRouter.
        * Utilizado para alimentar o grafo com novos dados.

    - Embedding Local - ML Onnx Runtime
        *Modelos: all-MiniLM-L6-v2-onnx, E5-Base (Convertido para Onnx).
        *Realiza comparações de "pesos" entre novos termos e categoria.

Usando via Docker:
///

Usando via Docker Compose:
///
