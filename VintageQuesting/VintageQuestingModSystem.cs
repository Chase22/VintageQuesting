using Newtonsoft.Json;
using VintageQuesting.api;
using VintageQuesting.json;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace VintageQuesting
{
    public class VintageQuestingModSystem : ModSystem
    {
        public static string MOD_ID = "vintagequesting";
        public static AssetCategory questCategory;

        public override void StartPre(ICoreAPI api)
        {
            questCategory = new AssetCategory("quests", true, EnumAppSide.Server);
        }

        public override void AssetsLoaded(ICoreAPI api)
        {
            if (api.Side != EnumAppSide.Server)
            {
                return;
            }
            ILogger logger = api.Logger;

            var taskTypes = TaskTypeCollector.ScanForTaskTypes(logger);
            var converter = new TaskJsonConverter(taskTypes);
            var settings = new JsonSerializerSettings
            {
                ContractResolver = new TaskContractResolver(converter)
            };
            api.Assets.GetManyInCategory("quests", "").ForEach((asset) =>
            {
                if (asset.Location.Path.EndsWith(".json"))
                {
                    logger.Notification($"Found quest definition: {asset.Name}");
                }
            });
            var asset = api.Assets.Get(new AssetLocation(MOD_ID, "quests/quests.json"));
            if (asset == null)
            {
                logger.Error("Could not find quests.json");
                return;
            }

            logger.Notification($"Loading quest definitions from {asset.Name}");
            QuestManager.Initialize(asset.ToText(), JsonSerializer.Create(settings));
            logger.Notification($"QuestManager initialized successfully");
            logger.Notification($"Loaded {QuestManager.Instance.Quests.Count} quests");
        }

        public override void StartServerSide(ICoreServerAPI sapi)
        {
            Commands.RegisterCommands(sapi);
        }
    }
}
