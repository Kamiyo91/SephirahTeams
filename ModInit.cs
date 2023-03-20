using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BigDLL4221.Enum;
using BigDLL4221.Models;
using BigDLL4221.Utils;
using SephirahTeams.HarmonyPatches;

namespace SephirahTeams
{
    public class ModInit : ModInitializer
    {
        public override void OnInitializeMod()
        {
            OnInitParameters();
            ArtUtil.GetArtWorks(new DirectoryInfo(SephirahTeamModParameters.Path + "/ArtWork"));
            CardUtil.ChangeCardItem(ItemXmlDataList.instance, SephirahTeamModParameters.PackageId);
            PassiveUtil.ChangePassiveItem(SephirahTeamModParameters.PackageId);
            LocalizeUtil.AddGlobalLocalize(SephirahTeamModParameters.PackageId);
            ArtUtil.MakeCustomBook(SephirahTeamModParameters.PackageId);
            ArtUtil.PreLoadBufIcons();
            LocalizeUtil.RemoveError();
            SephirahTeamModParameters.Harmony.CreateClassProcessor(typeof(MainHarmonyPatch)).Patch();
        }

        private static void OnInitParameters()
        {
            ModParameters.PackageIds.Add(SephirahTeamModParameters.PackageId);
            SephirahTeamModParameters.Path = Path.GetDirectoryName(
                Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
            ModParameters.Path.Add(SephirahTeamModParameters.PackageId, SephirahTeamModParameters.Path);
            OnInitSprites();
            OnInitRewards();
            OnInitCredenza();
        }

        private static void OnInitRewards()
        {
            ModParameters.StartUpRewardOptions.Add(new RewardOptions(keypages: new List<LorId>
            {
                new LorId(SephirahTeamModParameters.PackageId, 10000001),
                new LorId(SephirahTeamModParameters.PackageId, 10000002),
                new LorId(SephirahTeamModParameters.PackageId, 10000003),
                new LorId(SephirahTeamModParameters.PackageId, 10000004),
                new LorId(SephirahTeamModParameters.PackageId, 10000005),
                new LorId(SephirahTeamModParameters.PackageId, 10000006),
                new LorId(SephirahTeamModParameters.PackageId, 10000007),
                new LorId(SephirahTeamModParameters.PackageId, 10000008),
                new LorId(SephirahTeamModParameters.PackageId, 10000009),
                new LorId(SephirahTeamModParameters.PackageId, 10000010),
                new LorId(SephirahTeamModParameters.PackageId, 10000011),
                new LorId(SephirahTeamModParameters.PackageId, 10000012),
                new LorId(SephirahTeamModParameters.PackageId, 10000013),
                new LorId(SephirahTeamModParameters.PackageId, 10000014)
            }));
        }

        private static void OnInitCredenza()
        {
            ModParameters.CredenzaOptions.Add(SephirahTeamModParameters.PackageId,
                new CredenzaOptions(baseIconSpriteId: "Chapter1", credenzaName: "Sephirah Teams"));
        }

        private static void OnInitSprites()
        {
            ModParameters.SpriteOptions.Add(SephirahTeamModParameters.PackageId, new List<SpriteOptions>
            {
                new SpriteOptions(SpriteEnum.Custom, 10000001, "DefaultSprite_ST4221"),
                new SpriteOptions(SpriteEnum.Custom, 10000002, "DefaultSprite_ST4221"),
                new SpriteOptions(SpriteEnum.Custom, 10000003, "DefaultSprite_ST4221"),
                new SpriteOptions(SpriteEnum.Custom, 10000004, "DefaultSprite_ST4221"),
                new SpriteOptions(SpriteEnum.Custom, 10000005, "DefaultSprite_ST4221"),
                new SpriteOptions(SpriteEnum.Custom, 10000006, "DefaultSprite_ST4221"),
                new SpriteOptions(SpriteEnum.Custom, 10000007, "DefaultSprite_ST4221"),
                new SpriteOptions(SpriteEnum.Custom, 10000008, "DefaultSprite_ST4221"),
                new SpriteOptions(SpriteEnum.Custom, 10000009, "DefaultSprite_ST4221"),
                new SpriteOptions(SpriteEnum.Custom, 10000010, "DefaultSprite_ST4221"),
                new SpriteOptions(SpriteEnum.Custom, 10000011, "DefaultSprite_ST4221"),
                new SpriteOptions(SpriteEnum.Custom, 10000012, "DefaultSprite_ST4221"),
                new SpriteOptions(SpriteEnum.Custom, 10000013, "DefaultSprite_ST4221"),
                new SpriteOptions(SpriteEnum.Custom, 10000014, "DefaultSprite_ST4221")
            });
        }
    }
}