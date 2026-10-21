using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Nodes;
using VintageQuesting.api;
using VintageQuesting.json;
using Vintagestory.API.Common;

namespace VintageQuestingTest
{
    internal class TestLogger : LoggerBase
    {
        protected override void LogImpl(EnumLogType logType, [StringSyntax("CompositeFormat")] string format, params object[] args)
        {
            Console.WriteLine($"[{logType}] {string.Format(format, args)}");
        }
    }

    [TaskType("TestTask")]
    internal class TestTask : VintageQuesting.api.Task<String>
    {
        public string Name
        {
            get { return "TestTask"; }
        }
        public String data { get; set; }
    }

    [TestClass]
    public sealed class Test1
    {
        [TestMethod]
        public void CanDeserializeQuest()
        {
            var json = """
            {
                "Id": "TestQuest",
                "Name": "Test Quest",
                "Description": "This is a test quest.",
                "Tasks": [
                    {
                        "Type": "TestTask",
                        "Name": "Test Task",
                        "data": "Hello World"
                    }
                ]
            }
            """;

            var taskTypes = TaskTypeCollector.ScanForTaskTypes(new TestLogger());
            var converter = new TaskJsonConverter(taskTypes);
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new TaskContractResolver(converter)
            };

            var quest = JsonConvert.DeserializeObject<QuestDefinition>(json, settings);

            Assert.IsNotNull(quest);
            Assert.AreEqual("Test Quest", quest.Name);
            Assert.AreEqual("Hello World", ((TestTask)quest.Tasks[0]).data);
        }
    }
}
