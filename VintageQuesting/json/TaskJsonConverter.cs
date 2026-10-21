using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using VintageQuesting.api;
using Vintagestory.API.Common;

namespace VintageQuesting.json
{
    public class TaskJsonConverter : JsonConverter
    {
        private readonly Dictionary<string, Type> _taskTypeMapping;

        public TaskJsonConverter(IReadOnlyDictionary<string, Type> taskTypeMapping)
        {
            _taskTypeMapping = new Dictionary<string, Type>(taskTypeMapping);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TaskBase) || typeof(TaskBase).IsAssignableFrom(objectType);
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            var jsObject = JObject.Load(reader);

            // Extract the "Type" field as discriminator
            var typeToken = jsObject["Type"];
            if (typeToken == null)
                throw new JsonException("Task must have a 'Type' field");

            var typeName = typeToken.Value<string>();
            if (!_taskTypeMapping.TryGetValue(typeName, out var targetType))
                throw new JsonException($"Unknown task type: {typeName}");

            // Deserialize to the correct type without passing a converter to avoid recursion
            return JsonConvert.DeserializeObject(jsObject.ToString(), targetType);
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
