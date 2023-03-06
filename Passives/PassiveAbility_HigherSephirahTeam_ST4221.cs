using System.Collections.Generic;

namespace SephirahTeams.Passives
{
    public class PassiveAbility_HigherSephirahTeam_ST4221 : PassiveAbility_SephirahTeam_ST4221
    {
        public override List<SephirahType> GetSephirahs()
        {
            return new List<SephirahType>
            {
                SephirahType.Keter, SephirahType.Binah, SephirahType.Gebura, SephirahType.Chesed, SephirahType.Hokma
            };
        }
    }
}