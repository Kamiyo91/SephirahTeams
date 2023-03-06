using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using SephirahTeams.Util;
using UI;
using UnityEngine;

namespace SephirahTeams.HarmonyPatches
{
    [HarmonyPatch]
    public class BaseHarmonyPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(BookModel), "GetThumbSprite")]
        [HarmonyPatch(typeof(BookXmlInfo), "GetThumbSprite")]
        public static void General_GetThumbSprite(object __instance, ref Sprite __result)
        {
            switch (__instance)
            {
                case BookXmlInfo bookInfo:
                    ArtUtil.GetThumbSprite(bookInfo.id, ref __result);
                    break;
                case BookModel bookModel:
                    ArtUtil.GetThumbSprite(bookModel.BookId, ref __result);
                    break;
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(UIBookStoryChapterSlot), "SetEpisodeSlots")]
        public static void UIBookStoryChapterSlot_SetEpisodeSlots(UIBookStoryChapterSlot __instance)
        {
            ArtUtil.SetEpisodeSlots(__instance, __instance.panel, __instance.EpisodeSlots);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(UISettingInvenEquipPageListSlot), "SetBooksData")]
        [HarmonyPatch(typeof(UIInvenEquipPageListSlot), "SetBooksData")]
        public static void General_SetBooksData(object __instance,
            List<BookModel> books, UIStoryKeyData storyKey)
        {
            switch (__instance)
            {
                case UISettingInvenEquipPageListSlot instance:
                    ArtUtil.SetBooksData(instance, books, storyKey);
                    break;
                case UIInvenEquipPageListSlot instance:
                    ArtUtil.SetBooksData(instance, books, storyKey);
                    break;
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(BookInventoryModel), "LoadFromSaveData")]
        public static void BookInventoryModel_LoadFromSaveData(BookInventoryModel __instance)
        {
            foreach (var keypageId in ModParameters.StartUpKeypages.Where(keypageId =>
                         !Singleton<BookInventoryModel>.Instance.GetBookListAll()
                             .Exists(x => x.GetBookClassInfoId() == keypageId)))
                __instance.CreateBook(keypageId);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(TextDataModel), "InitTextData")]
        public static void TextDataModel_InitTextData(string currentLanguage)
        {
            ModParameters.Language = currentLanguage;
            LocalizeUtil.AddGlobalLocalize();
        }
    }
}