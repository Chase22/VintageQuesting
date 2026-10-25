namespace VintageQuesting.api
{
    [TaskType("Error")]
    public class ErrorTask(string name) : TaskBase
    {
        private readonly string _name = name;

        public override string Name
        {
            get { return _name; }
            set { return; }
        }

        public override bool Completed { get { return false; } }
    }
}