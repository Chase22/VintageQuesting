using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace VintageQuesting.api
{
    public class QuestManager
    {   

        private static QuestManager? _instance;
        public static QuestManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new InvalidOperationException("QuestManager has not been initialized");
                }
                return _instance;
            }
        }

        [MemberNotNull(nameof(_instance))]
        public static void Initialize(string questJson, JsonSerializer questSerializer)
        {
            if (_instance != null)
            {
                throw new InvalidOperationException("QuestManager has already been initialized");
            }

            using (var reader = new StringReader(questJson))
            {
                var quests = questSerializer.Deserialize<Dictionary<string, QuestDefinition>>(new JsonTextReader(reader));
                if (quests == null)
                {
                    throw new InvalidOperationException("Failed to deserialize quest definitions");
                }
                _instance = new QuestManager(quests);
            };

        }

        public readonly IReadOnlyDictionary<string, QuestDefinition> Quests;

        public QuestManager(IReadOnlyDictionary<string, QuestDefinition> quests)
        {
            Quests = quests;
        }

        public QuestDefinition GetQuest(string questId)
        {
            if (Quests.TryGetValue(questId, out var quest))
            {
                return quest;
            }
            throw new KeyNotFoundException($"Quest with ID '{questId}' not found");
        }

        public bool TryGetQuest(string questId, [MaybeNullWhen(false)] out QuestDefinition quest)
        {
            return Quests.TryGetValue(questId, out quest);
        }
    }
}
