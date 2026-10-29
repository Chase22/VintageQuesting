using System;
using System.Diagnostics.CodeAnalysis;

namespace VintageQuesting.api
{
    [AttributeUsage(AttributeTargets.Class)]
    public class TaskTypeAttribute : Attribute
    {
        new public string TypeId { get; }

        public TaskTypeAttribute(string typeId)
        {
            TypeId = typeId;
        }
    }

    public abstract class TaskBase
    {
        public abstract string Name { get; set; }
        
        public abstract bool Completed { get; }
    }

    public abstract class Task<T> : TaskBase
    {
        [MaybeNull]
        public T Data { get; set; }
    }
}
