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

            int idCollisionCount = 0;

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

                                // Check for conflicting type IDs
                                if (map.TryGetValue(typeId, out var conflictingType))
                                {
                                    if (map[typeId] != typeof(ErrorTask))
                                    {
                                        logger.Warning($"Removing task type `{conflictingType.FullName}`, type ID `{typeId}` has conflicts.");
                                        map[typeId] = typeof(ErrorTask);
                                        idCollisionCount++;
                                    }

                                    logger.Warning($"Failed to collect task `{type.FullName}`, type ID `{typeId}` has already been used.");
                                    idCollisionCount++;
                                }
                                else
                                {
                                    map[typeId] = type;
                                }
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

            if (idCollisionCount > 0)
            {
                logger.Error($"Task type collection identified {idCollisionCount} collisions between Task IDs. The conflicting task types have been removed to prevent further errors.");
            }

            return map;
        }
    }
}
