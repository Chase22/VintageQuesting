using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System;
using VintageQuesting.api;

namespace VintageQuesting.json
{
    public class TaskContractResolver : DefaultContractResolver
    {
        private readonly TaskJsonConverter _converter;

        public TaskContractResolver(TaskJsonConverter converter)
        {
            _converter = converter;
        }

        protected override JsonContract CreateContract(Type objectType)
        {
            var contract = base.CreateContract(objectType);

            // For TaskBase types, use the custom converter
            if (typeof(TaskBase).IsAssignableFrom(objectType))
            {
                contract.Converter = _converter;
            }

            return contract;
        }
    }
}
