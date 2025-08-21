using System.Text.Json;
using System.Text.Json.Serialization;

namespace RelationalGraph.Domain.Nodes
{
    public class PropertiesConverter : JsonConverter<IProperties>
    {
        public override IProperties Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Carregar o JSON atual
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                // Obter o objeto JSON
                var root = doc.RootElement;

                // Lógica para decidir qual tipo concreto instanciar
                if (root.TryGetProperty("CreatedAt", out _)) // Se a propriedade "CreatedAt" existir, é um CategoryProperties
                {
                    return JsonSerializer.Deserialize<CategoryProperties>(root.GetRawText(), options)!;
                }

                if (root.TryGetProperty("Term", out _)) // Se a propriedade "Term" existir, é um TermProperties
                {
                    return JsonSerializer.Deserialize<TermProperties>(root.GetRawText(), options)!;
                }

                throw new JsonException("Tipo de propriedade desconhecido.");
            }
        }

        public override void Write(Utf8JsonWriter writer, IProperties value, JsonSerializerOptions options)
        {
            // Usar o JsonSerializer para serializar o objeto concreto
            JsonSerializer.Serialize(writer, value, value.GetType(), options);
        }
    }
}
