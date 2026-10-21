using System;
using System.Collections.Generic;
using System.Text;

namespace VintageQuesting.api
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TaskTypeAttribute : Attribute
    {
        public string TypeId { get; }

        public TaskTypeAttribute(string typeId)
        {
            TypeId = typeId;
        }
    }

    public abstract class TaskBase
    {
        public string Name { get; set; }
        public abstract bool Completed { get; }
    }

    public abstract class Task<T> : TaskBase
    {
        public T data { get; set; }
    }
}
