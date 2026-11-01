using Newtonsoft.Json;
using Vintagestory.API.Common;

namespace VintageQuesting.api
{
    /// <summary>
    /// A simple task type with a fixed boolean. Will never complete by itself and must be triggered by external code.
    /// 
    /// Data represents the text shown in the task list
    /// </summary>
    [TaskType("Boolean")]
    public class BooleanTask : NonNullableTask<string>
    {
        public bool completed { get; set; } = false;
        public override bool Completed => completed;

        public override string Format()
        {
            return data;
        }

        public override string Format(IPlayer player)
        {
            return Format();
        }
    }
}
