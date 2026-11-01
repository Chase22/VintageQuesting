using Newtonsoft.Json;
using Shouldly;
using VintageQuesting.api;
using VintageQuesting.json;
using VintageQuestingTest.util;
using Vintagestory.API.Common;

namespace VintageQuestingTest
{

    [TaskType("TestTask")]
    internal class TestTask : VintageQuesting.api.Task<string>
    {
        public string Name
        {
            get { return "TestTask"; }
        }

        public string data { get; set; }

        public override bool Completed => false;

        public override string Format()
        {
            return $"TestTask: {data}";
        }

        public override string Format(IPlayer player)
        {
            return Format();
        }
    }

    [TestClass]
    public class TaskJsonConverterTests
    {
        private TaskJsonConverter _converter = null!;
        private JsonSerializerSettings _settings = null!;

        [TestInitialize]
        public void Setup()
        {
            var taskTypes = TaskTypeCollector.ScanForTaskTypes(new TestLogger());
            _converter = new TaskJsonConverter(taskTypes);
            _settings = new JsonSerializerSettings
            {
                ContractResolver = new TaskContractResolver(_converter)
            };
        }

        #region CanConvert Tests

        [TestMethod]
        public void CanConvert_WithTaskBaseType_ReturnsTrue()
        {
            _converter.CanConvert(typeof(TaskBase)).ShouldBeTrue();
        }

        [TestMethod]
        public void CanConvert_WithTaskImplementationType_ReturnsTrue()
        {
            _converter.CanConvert(typeof(TestTask)).ShouldBeTrue();
        }

        [TestMethod]
        public void CanConvert_WithGenericTaskType_ReturnsTrue()
        {
            _converter.CanConvert(typeof(VintageQuesting.api.Task<string>)).ShouldBeTrue();
        }

        [TestMethod]
        public void CanConvert_WithUnrelatedType_ReturnsFalse()
        {
            _converter.CanConvert(typeof(string)).ShouldBeFalse();
            _converter.CanConvert(typeof(int)).ShouldBeFalse();
        }

        #endregion

        #region Successful Deserialization Tests

        [TestMethod]
        public void ReadJson_WithValidTaskType_DeserializesCorrectly()
        {
            var json = """
            {
                "Type": "TestTask",
                "data": "Test data"
            }
            """;

            var result = JsonConvert.DeserializeObject<TaskBase>(json, _settings);

            result.ShouldNotBeNull();
            result.ShouldBeOfType<TestTask>();
            var testTask = (TestTask)result;
            testTask.data.ShouldBe("Test data");
        }

        [TestMethod]
        public void ReadJson_WithBooleanTask_DeserializesCorrectly()
        {
            var json = """
            {
                "Type": "Boolean"
            }
            """;

            var result = JsonConvert.DeserializeObject<TaskBase>(json, _settings);
          
            var boolTask = result.ShouldBeOfType<BooleanTask>();

            boolTask.Completed.ShouldBeFalse();
        }

        [TestMethod]
        public void ReadJson_WithMultipleFields_PreservesAllData()
        {
            var json = """
            {
                "Type": "TestTask",
                "data": "Complex data with special chars: !@#$%"
            }
            """;

            var result = JsonConvert.DeserializeObject<TaskBase>(json, _settings);
            var testTask = result.ShouldBeOfType<TestTask>();

            testTask.data.ShouldBe("Complex data with special chars: !@#$%");
        }

        [TestMethod]
        public void ReadJson_WithEmptyStringData_DeserializesCorrectly()
        {
            var json = """
            {
                "Type": "TestTask",
                "data": ""
            }
            """;

            var result = JsonConvert.DeserializeObject<TaskBase>(json, _settings);
            var testTask = result.ShouldBeOfType<TestTask>();

            testTask.data.ShouldBeEmpty();
        }

        [TestMethod]
        public void ReadJson_WithNullData_DeserializesCorrectly()
        {
            var json = """
            {
                "Type": "TestTask",
                "data": null
            }
            """;

            var result = JsonConvert.DeserializeObject<TaskBase>(json, _settings);
            var testTask = result.ShouldBeOfType<TestTask>();

            testTask.data.ShouldBeNull();
        }

        #endregion

        #region Error Cases

        [TestMethod]
        public void ReadJson_WithoutTypeField_ThrowsJsonException()
        {
            var json = """
            {
                "data": "No type field"
            }
            """;

            var action = () => JsonConvert.DeserializeObject<TaskBase>(json, _settings);

            action.ShouldThrow<JsonException>()
                .Message.ShouldContain("Task must have a 'Type' field");
        }

        [TestMethod]
        [DataRow("", "Empty type", DisplayName = "Empty Type Field")]
        [DataRow("NonexistentTask", "Unknown type", DisplayName = "Unknown Task Type")]
        public void ReadJson_WithInvalidTaskType_ThrowsJsonException(string taskType, string data)
        {
            var json = $@"
            {{
                ""Type"": ""{taskType}"",
                ""data"": ""{data}""
            }}";

            var action = () => JsonConvert.DeserializeObject<TaskBase>(json, _settings);

            action.ShouldThrow<JsonException>();
        }

        [TestMethod]
        public void ReadJson_WithNullTypeField_ThrowsArgumentNullException()
        {
            var json = """
            {
                "Type": null,
                "data": "Null type"
            }
            """;

            var action = () => JsonConvert.DeserializeObject<TaskBase>(json, _settings);

            action.ShouldThrow<ArgumentNullException>();
        }

        [TestMethod]
        public void ReadJson_WithInvalidJson_ThrowsJsonException()
        {
            var json = "{ invalid json";

            var action = () => JsonConvert.DeserializeObject<TaskBase>(json, _settings);

            action.ShouldThrow<JsonException>();
        }

        [TestMethod]
        public void ReadJson_WithNullDataOnNonNullableTask_ThrowsJsonException()
        {
            var json = """
            {
                "Type": "Boolean",
                "data": null
            }
            """;
            var action = () => JsonConvert.DeserializeObject<TaskBase>(json, _settings);
            action.ShouldThrow<JsonException>();
        }

        #endregion

        #region WriteJson Tests

        [TestMethod]
        public void WriteJson_WithNullValue_SerializesSuccessfully()
        {
            var action = () => JsonConvert.SerializeObject((TestTask)null, _settings);

            action.ShouldNotThrow();
        }

        #endregion

        #region Full Quest Deserialization Tests

        [TestMethod]
        public void CanDeserializeQuestWithSingleTask()
        {
            var json = """
            {
                "Id": "TestQuest",
                "Name": "Test Quest",
                "Description": "This is a test quest.",
                "Tasks": [
                    {
                        "Type": "TestTask",
                        "data": "Hello World"
                    }
                ]
            }
            """;

            var quest = JsonConvert.DeserializeObject<QuestDefinition>(json, _settings);

            quest.ShouldNotBeNull();
            quest.Id.ShouldBe("TestQuest");
            quest.Name.ShouldBe("Test Quest");

            var testTask = quest.Tasks.ShouldHaveSingleItem().ShouldBeOfType<TestTask>();
            testTask.data.ShouldBe("Hello World");
        }

        [TestMethod]
        public void CanDeserializeQuestWithMultipleTasks()
        {
            var json = """
            {
                "Id": "ComplexQuest",
                "Name": "Complex Quest",
                "Description": "Multiple tasks",
                "Tasks": [
                    {
                        "Type": "TestTask",
                        "data": "First task"
                    },
                    {
                        "Type": "Boolean",
                        "Name": "Second task",
                    },
                    {
                        "Type": "TestTask",
                        "data": "Third task"
                    }
                ]
            }
            """;

            var quest = JsonConvert.DeserializeObject<QuestDefinition>(json, _settings);

            quest.Tasks.Count.ShouldBe(3);
            quest.Tasks[0].ShouldBeOfType<TestTask>();
            quest.Tasks[1].ShouldBeOfType<BooleanTask>();
            quest.Tasks[2].ShouldBeOfType<TestTask>();
        }

        [TestMethod]
        public void CanDeserializeQuestWithNoTasks()
        {
            var json = """
            {
                "Id": "EmptyQuest",
                "Name": "Empty Quest",
                "Description": "No tasks",
                "Tasks": []
            }
            """;

            var quest = JsonConvert.DeserializeObject<QuestDefinition>(json, _settings);

            quest.ShouldNotBeNull();
            quest.Tasks.ShouldBeEmpty();
        }

        [TestMethod]
        public void QuestDeserializationWithInvalidTask_ThrowsJsonException()
        {
            var json = """
            {
                "Id": "BadQuest",
                "Name": "Bad Quest",
                "Tasks": [
                    {
                        "Type": "InvalidTaskType",
                        "data": "This will fail"
                    }
                ]
            }
            """;

            var action = () => JsonConvert.DeserializeObject<QuestDefinition>(json, _settings);

            action.ShouldThrow<JsonException>();
        }

        #endregion

        #region Edge Cases

        [TestMethod]
        public void ReadJson_WithExtraUnknownFields_IgnoresThemCorrectly()
        {
            var json = """
            {
                "Type": "TestTask",
                "data": "Test",
                "unknownField": "This should be ignored",
                "anotherUnknown": 12345
            }
            """;

            var result = JsonConvert.DeserializeObject<TaskBase>(json, _settings);

            result.ShouldNotBeNull();
            result.ShouldBeOfType<TestTask>();
            var testTask = (TestTask)result;
            testTask.data.ShouldBe("Test");
        }

        [TestMethod]
        public void ReadJson_WithSpecialCharactersInType_HandlesCorrectly()
        {
            var json = """
            {
                "Type": "TestTask",
                "data": "Special chars: quotes, newlines, tabs"
            }
            """;

            var result = JsonConvert.DeserializeObject<TaskBase>(json, _settings);

            result.ShouldNotBeNull();
            result.ShouldBeOfType<TestTask>();
            var testTask = (TestTask)result;
            testTask.data.ShouldBe("Special chars: quotes, newlines, tabs");
        }

        [TestMethod]
        public void ReadJson_WithWhitespaceInTypeField_TreatsAsUnknownType()
        {
            var json = """
            {
                "Type": " TestTask ",
                "data": "Whitespace padded"
            }
            """;

            var action = () => JsonConvert.DeserializeObject<TaskBase>(json, _settings);

            action.ShouldThrow<JsonException>();
        }

        #endregion
    }
}
