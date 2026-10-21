using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using VintageQuesting.api;
using Vintagestory.API.Common;

namespace VintageQuesting.json
{
    internal static class TaskTypeCollector
    {
        internal static Dictionary<string, Type> ScanForTaskTypes(ILogger logger)
        {
            var map = new Dictionary<string, Type>();
            var baseType = typeof(TaskBase);

            // Scan all loaded assemblies for types inheriting from TaskBase
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    foreach (var type in assembly.GetTypes())
                    {
                        if (baseType.IsAssignableFrom(type) && !type.IsAbstract)
                        {
                            var taskTypeAttr = type.GetCustomAttribute<TaskTypeAttribute>();
                            if (taskTypeAttr != null)
                            {
                                map[taskTypeAttr.TypeId] = type;
                            }
                            else
                            {
                                logger.Warning($"Task type `{type.FullName}` does not have a TaskTypeAttribute. This is probably a mistake.");
                            }
                        }
                    }
                }
                catch { /* Skip assemblies that can't be scanned */ }
            }

            return map;
        }
    }
}
