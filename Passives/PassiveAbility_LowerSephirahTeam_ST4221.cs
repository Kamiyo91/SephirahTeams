using System.Collections.Generic;

namespace SephirahTeams.Passives
{
    public class PassiveAbility_LowerSephirahTeam_ST4221 : PassiveAbility_SephirahTeam_ST4221
    {
        public override List<SephirahType> GetSephirahs()
        {
            return new List<SephirahType>
            {
                SephirahType.Hod, SephirahType.Malkuth, SephirahType.Yesod, SephirahType.Netzach, SephirahType.Tiphereth
            };
        }
    }
}