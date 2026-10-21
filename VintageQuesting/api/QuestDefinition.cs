using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using VintageQuesting.json;
using Vintagestory.API.Server;

namespace VintageQuesting.api
{
    public class QuestDefinition
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<TaskBase> Tasks { get; set; }
        public List<QuestAction> AcceptActions { get; set; }
        public List<QuestAction> CompleteActions { get; set; }
    }

    public interface QuestAction
    {
        public string Name { get; }

        public void Execute(IServerPlayer actor);
    }

    public abstract class QuestAction<T>: QuestAction
    {
        public string Name { get; }
        protected T data { get; }

        public abstract void Execute(IServerPlayer actor);
    }
}
