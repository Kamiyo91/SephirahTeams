using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace SephirahTeams
{
    public class ModParameters
    {
        public const string PackageId = "SephirahTeamST4221.Mod";
        public static Harmony Harmony = new Harmony("LOR.SephirahTeamST4221_MOD");
        public static string Language = GlobalGameManager.Instance.CurrentOption.language;
        public static string Path = string.Empty;
        public static Dictionary<string, Sprite> ArtWorks = new Dictionary<string, Sprite>();

        public static List<LorId> StartUpKeypages = new List<LorId>
            { new LorId(PackageId, 10000001), new LorId(PackageId, 10000002), new LorId(PackageId, 10000003) };

        public static Dictionary<int, string> Sprites = new Dictionary<int, string>
        {
            { 10000001, "DefaultSprite_ST4221" },
            { 10000002, "DefaultSprite_ST4221" },
            { 10000003, "DefaultSprite_ST4221" }
        };
    }
}