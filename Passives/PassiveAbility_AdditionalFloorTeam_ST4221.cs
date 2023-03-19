using System.Linq;
using BigDLL4221.Models;
using BigDLL4221.Utils;
using UnityEngine;

namespace SephirahTeams.Passives
{
    public class PassiveAbility_AdditionalFloorTeam_ST4221 : PassiveAbilityBase
    {
        public int EmotionCards;
        public StageLibraryFloorModel StageModel;

        public virtual SephirahType GetSephirah()
        {
            return SephirahType.None;
        }

        public virtual int SelectionMaxNumber()
        {
            var level = 0;
            var floor = LibraryModel.Instance.GetFloor(GetSephirah());
            if (floor != null)
                level = Singleton<StageController>.Instance.IsRebattle ? floor.TemporaryLevel : floor.Level;
            return level;
        }

        public override void OnWaveStart()
        {
            StageModel = Singleton<StageController>.Instance.GetCurrentStageFloorModel();
            EmotionCards = Singleton<StageController>.Instance.GetStageModel()
                .GetStageStorageData<int>($"EmotionUnit{GetSephirah()}_ST4221", out var usedEmotionCards)
                ? usedEmotionCards
                : 0;
        }

        public override /*async*/ void OnRoundEndTheLast_ignoreDead()
        {
            if (BattleObjectManager.instance.GetAliveList(Faction.Player)
                .All(x => x.UnitData.unitData.OwnerSephirah != GetSephirah())) return;
            //await GenericUtil.PutTaskDelay(1000);
            owner.emotionDetail.CheckLevelUp();
            if (StageModel.team.emotionLevel > SelectionMaxNumber() + 1 ||
                EmotionCards >= StageModel.team.emotionLevel) return;
            EmotionCards++;
            var emotionList = CardUtil.CreateSephirahSelectableList(owner.emotionDetail.EmotionLevel, GetSephirah());
            StaticModsInfo.OnPlayCardEmotion = true;
            if (emotionList.Count <= 0) return;
            if (!SingletonBehavior<BattleManagerUI>.Instance.ui_levelup.IsEnabled)
                SingletonBehavior<BattleManagerUI>.Instance.ui_levelup.SetRootCanvas(true);
            SingletonBehavior<BattleManagerUI>.Instance.ui_levelup.Init(
                Mathf.Clamp(owner.emotionDetail.EmotionLevel - 1, 0, 4), emotionList);
        }

        public override void OnBattleEnd()
        {
            var stageModel = Singleton<StageController>.Instance.GetStageModel();
            stageModel.SetStageStorgeData($"EmotionUnit{GetSephirah()}_ST4221", EmotionCards);
        }
    }
}