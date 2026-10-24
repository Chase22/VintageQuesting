using System;
using System.Collections.Generic;
using System.Reflection;
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
                                var typeId = taskTypeAttr.TypeId;

                                // Check for duplicate type IDs
                                if (map.TryGetValue(typeId, out var conflictingType))
                                {
                                    logger.Error($"Task type `{type.FullName}` conflicts with `{conflictingType.FullName}` (both have ID `{typeId}`).");
                                }

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
