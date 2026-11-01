using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VintageQuesting.api;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace VintageQuesting
{
    internal class Commands
    {
        internal static void RegisterCommands(ICoreServerAPI sapi)
        {
            var p = sapi.ChatCommands.Parsers;

            sapi.ChatCommands.Create()
                .WithName("quests")
                .RequiresPrivilege(Privilege.controlserver)
                .WithDescription("List available quests")
                .HandleWith(args =>
                {
                    var quests = QuestManager.Instance.Quests;

                    if (quests.Count == 0)
                    {
                        return TextCommandResult.Error("No quests available.");
                    }

                    var sb = new StringBuilder();

                    sb.AppendLine("Available Quests:");
                    foreach (var (id, quest ) in quests)
                    {
                        sb.AppendLine($"- {quest.Name} (ID: {id})");
                    }
                    return TextCommandResult.Success(sb.ToString());
                });

            sapi.ChatCommands.Create()
                .WithName("quest")
                .RequiresPrivilege(Privilege.controlserver)
                .WithDescription("Get details of a specific quest by ID")
                .WithArgs(p.WordRange("questId", QuestManager.Instance.Quests.Keys.ToArray()))
                .HandleWith(args =>
                {
                    var questId = (string)args[0];
                    if (string.IsNullOrEmpty(questId))
                    {
                        return TextCommandResult.Error("You must provide a quest ID.");
                    }
                    if (!QuestManager.Instance.Quests.TryGetValue(questId, out var quest))
                    {
                        return TextCommandResult.Error($"No quest found with ID '{questId}'.");
                    }
                    var sb = new StringBuilder();
                    sb.AppendLine($"Quest Details for '{quest.Name}':");
                    sb.AppendLine($"ID: {questId}");
                    sb.AppendLine($"Description: {quest.Description}");
                    sb.AppendLine($"Tasks:");
                    foreach (var task in quest.Tasks)
                    {
                        if (null == task)
                        {
                            sb.AppendLine("     - Failed to load task, please review the error log");
                        }
                        else
                        {
                            sb.AppendLine($"    - {task.Name}");
                        }
                    }
                    return TextCommandResult.Success(sb.ToString());
                });
        }
    }
}
