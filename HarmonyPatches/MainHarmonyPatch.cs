using System.Linq;
using HarmonyLib;
using SephirahTeams.Passives;
using SephirahTeams.Util;
using UI;

namespace SephirahTeams.HarmonyPatches
{
    [HarmonyPatch]
    public class MainHarmonyPatch
    {
        [HarmonyPostfix]
        [HarmonyPriority(0)]
        [HarmonyPatch(typeof(StageLibraryFloorModel), "InitUnitList")]
        public static void StageLibraryFloorModel_InitUnitList(StageLibraryFloorModel __instance, StageModel stage)
        {
            var unit = __instance._unitList.FirstOrDefault(x => x.unitData.isSephirah);
            if (unit == null) return;
            var unitPassives = unit.unitData.bookItem.CreatePassiveList();
            if (!unitPassives.Exists(x => x is PassiveAbility_SephirahTeam_ST4221)) return;
            var sephirahs =
                (unitPassives.FirstOrDefault(x => x is PassiveAbility_SephirahTeam_ST4221) as
                    PassiveAbility_SephirahTeam_ST4221)?.GetSephirahs();
            __instance._unitList.Clear();
            UnitUtil.AddSephirahUnits(stage, __instance._unitList, sephirahs);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(BookModel), "CanSuccessionPassive")]
        public static void BookModel_CanSuccessionPassive(BookModel __instance, PassiveModel targetpassive,
            ref GivePassiveState haspassiveState, ref bool __result)
        {
            if (targetpassive.originData.currentpassive.id.packageId != ModParameters.PackageId) return;
            if (UI.UIController.Instance.CurrentUnit != null && !UI.UIController.Instance.CurrentUnit.isSephirah)
            {
                haspassiveState = GivePassiveState.Lock;
                __result = false;
            }

            switch (targetpassive.originData.currentpassive.id.id)
            {
                case 1 when __instance.GetPassiveModelList()
                    .Any(x => x.reservedData.currentpassive.id == new LorId(ModParameters.PackageId, 2) ||
                              x.reservedData.currentpassive.id == new LorId(ModParameters.PackageId, 4)):
                    haspassiveState = GivePassiveState.Lock;
                    __result = false;
                    break;
                case 2 when __instance.GetPassiveModelList()
                    .Any(x => x.reservedData.currentpassive.id == new LorId(ModParameters.PackageId, 1) ||
                              x.reservedData.currentpassive.id == new LorId(ModParameters.PackageId, 4)):
                    haspassiveState = GivePassiveState.Lock;
                    __result = false;
                    break;
                case 4 when __instance.GetPassiveModelList()
                    .Any(x => x.reservedData.currentpassive.id == new LorId(ModParameters.PackageId, 2) ||
                              x.reservedData.currentpassive.id == new LorId(ModParameters.PackageId, 1)):
                    haspassiveState = GivePassiveState.Lock;
                    __result = false;
                    break;
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(UICharacterListPanel), "RefreshBattleUnitDataModel")]
        public static void RefreshBattleUnitDataModel(UICharacterListPanel __instance,
            UnitDataModel data)
        {
            var slot = __instance.CharacterList;
            var stageModel = Singleton<StageController>.Instance.GetStageModel();
            var list = UnitUtil.UnitsToRecover(stageModel, data);
            foreach (var unit in list)
            {
                unit.Refreshhp();
                var uicharacterSlot = slot?.slotList.Find(x => x.unitBattleData == unit);
                if (uicharacterSlot == null || uicharacterSlot.unitBattleData == null) continue;
                uicharacterSlot.ReloadHpBattleSettingSlot();
            }
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(UISpriteDataManager), "GetStoryIcon")]
        public static void UISpriteDataManager_GetStoryIcon(ref string story)
        {
            if (story.Contains("SephirahTeamST4221"))
                story = "Chapter1";
        }
    }
}