namespace VintageQuesting.api
{
    [TaskType("Boolean")]
    public class BooleanTask: TaskBase
    {
        public override string Name {
            get;
            set;
        } = "Incomplete";

        public bool completed { get; set; }
        public override bool Completed => completed;
    }
}
