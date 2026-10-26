using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;
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
        public override string Name {
            get;
            set;
        } = "Whoops! This task has no name!";

        public override bool Completed => throw new NotImplementedException();
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
            Assert.AreEqual("Hello World", ((TestTask)quest.Tasks[0]).Data);
        }
    }
}
