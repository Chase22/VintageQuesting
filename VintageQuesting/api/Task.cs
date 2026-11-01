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

    /// <summary>
    /// Base class of all TaskTypes. Should only be used if a Task requires no configuration. Otherwise use <see cref="Task{T}"/>
    /// </summary>
    public abstract class TaskBase
    {
        public abstract string Name { get; set; }
        
        public abstract bool Completed { get; }
        
        /// <summary>
        /// Formats the Task into a single line text used for quest logs, commands etc. The version with not arguments should return a version assuming a player just accepted the quest
        /// </summary>
        /// <returns></returns>
        public abstract string Format();

        /// <summary>
        /// Formats the Task into a single line text used for quest logs, commands etc. The version with the player arguments should include the current state of the quest for the given player.
        /// </summary>
        /// <returns></returns>
        public abstract string Format(IPlayer player);
    }

    /// <summary>
    /// Implementation of <see cref="TaskBase"/> that includes a typed data parameter. Should be used over <c>TaskBase</c>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Task<T> : TaskBase
    {
        [MaybeNull]
        public T Data { get; set; }
    }

    /// <summary>
    /// Utility subclass of <see cref="Task{T}"/> that requires the data parameter to be non-null.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class NonNullableTask<T> : Task<T> where T : notnull;
}
