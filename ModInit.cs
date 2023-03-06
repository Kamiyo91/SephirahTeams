using System;
using System.IO;
using System.Reflection;
using SephirahTeams.HarmonyPatches;
using SephirahTeams.Util;

namespace SephirahTeams
{
    public class ModInit : ModInitializer
    {
        public override void OnInitializeMod()
        {
            ModParameters.Path =
                Path.GetDirectoryName(
                    Uri.UnescapeDataString(new UriBuilder(Assembly.GetExecutingAssembly().CodeBase).Path));
            ArtUtil.GetArtWorks(new DirectoryInfo(ModParameters.Path + "/ArtWork"));
            ModParameters.Harmony.CreateClassProcessor(typeof(MainHarmonyPatch)).Patch();
            ModParameters.Harmony.CreateClassProcessor(typeof(BaseHarmonyPatch)).Patch();
            LocalizeUtil.AddGlobalLocalize();
            LocalizeUtil.RemoveError();
        }
    }
}