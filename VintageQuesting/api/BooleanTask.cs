using System;
using System.Collections.Generic;
using System.Text;

namespace VintageQuesting.api
{
    [TaskType("Boolean")]
    public class BooleanTask: TaskBase
    {
        public bool completed { get; set; }
        public override bool Completed => completed;
    }
}
