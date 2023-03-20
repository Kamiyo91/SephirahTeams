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
            if (unitPassives.Exists(x => x is PassiveAbility_AdditionalFloorTeam_ST4221))
                foreach (var passive in unitPassives.Where(x => x is PassiveAbility_AdditionalFloorTeam_ST4221))
                {
                    var typedPassive = passive as PassiveAbility_AdditionalFloorTeam_ST4221;
                    foreach (var unitDataModel in LibraryModel.Instance.GetFloor(typedPassive.GetSephirah())
                                 .GetUnitDataList())
                        __instance._unitList.Add(UnitUtil.InitUnitDefault(stage, unitDataModel));
                }

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
            if (targetpassive.originData.currentpassive.id.packageId != SephirahTeamModParameters.PackageId) return;
            if (UI.UIController.Instance.CurrentUnit != null && !UI.UIController.Instance.CurrentUnit.isSephirah)
            {
                haspassiveState = GivePassiveState.Lock;
                __result = false;
            }

            var passive =
                Singleton<AssemblyManager>.Instance.CreateInstance_PassiveAbility(targetpassive.originData
                    .currentpassive.script);
            if (passive is PassiveAbility_AdditionalFloorTeam_ST4221 pas &&
                UI.UIController.Instance.CurrentUnit?.OwnerSephirah == pas.GetSephirah())
            {
                haspassiveState = GivePassiveState.Lock;
                __result = false;
            }

            if (targetpassive.originData.currentpassive.id.packageId != SephirahTeamModParameters.PackageId ||
                __instance.GetPassiveModelList().All(x =>
                    x.reservedData.currentpassive.id.packageId != SephirahTeamModParameters.PackageId))
                return;
            haspassiveState = GivePassiveState.Lock;
            __result = false;
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