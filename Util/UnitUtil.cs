using System.Collections.Generic;
using System.Linq;

namespace SephirahTeams.Util
{
    public static class UnitUtil
    {
        public static void AddSephirahUnits(StageModel stage,
            List<UnitBattleDataModel> unitList, List<SephirahType> sephirahs)
        {
            unitList?.AddRange(sephirahs.Select(sephirah => InitUnitDefault(stage,
                LibraryModel.Instance.GetOpenedFloorList()
                    .FirstOrDefault(x => x.Sephirah == sephirah)
                    ?.GetUnitDataList()
                    .FirstOrDefault(y => y.isSephirah))));
        }

        public static UnitBattleDataModel InitUnitDefault(StageModel stage, UnitDataModel data)
        {
            var unitBattleDataModel = new UnitBattleDataModel(stage, data);
            unitBattleDataModel.Init();
            return unitBattleDataModel;
        }

        public static List<UnitBattleDataModel> UnitsToRecover(StageModel stageModel, UnitDataModel data)
        {
            var floorList = StageController.Instance._stageModel.GetAvailableFloorList();
            var list = new List<UnitBattleDataModel>();
            foreach (var sephirah in floorList.Select(x => x.Sephirah))
                list.AddRange(stageModel.GetFloor(sephirah).GetUnitBattleDataList()
                    .Where(x => x.unitData == data));
            return list;
        }
    }
}