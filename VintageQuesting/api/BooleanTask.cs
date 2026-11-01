


namespace VintageQuesting.api
{
    /// <summary>
    /// A simple task type with a fixed boolean. Will never complete by itself and must be triggered by external code.
    /// 
    /// Data represents the text shown in the task list
    /// </summary>
    [TaskType("Boolean")]
        public override string Name {
            get;
            set;
        } = "Incomplete";

        public bool completed { get; set; }
        public bool completed { get; set; } = false;
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
