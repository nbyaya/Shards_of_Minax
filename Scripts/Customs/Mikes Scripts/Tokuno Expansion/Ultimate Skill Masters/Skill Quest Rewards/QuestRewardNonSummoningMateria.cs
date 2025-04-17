using Server.Items;
using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Customs.Mikes_Scripts.Skill_Masters.Ultimate_Skill_Masters.Skill_Quest_Rewards
{
    public class QuestRewardNonSummoningMateria : IRewardGroup
    {
        public int Weight => 10; // Set the weight for this group

        public Type[] GetRewards(PlayerMobile player, SkillName? skill)
        {
            return new[]
            {
                typeof(AxeBreathMateria),
                typeof(AxeCircleMateria),
                typeof(AxeLineMateria),
                typeof(BeeBreathMateria),
                typeof(BeeCircleMateria),
                typeof(BeeLineMateria),
                typeof(BlackSolenInfiltratorWarriorMateria),
                typeof(BladesBreathMateria),
                typeof(BladesCircleMateria),
                typeof(BladesLineMateria),
                typeof(BoulderBreathMateria),
                typeof(BoulderCircleMateria),
                typeof(BoulderLineMateria),
                typeof(CrankBreathMateria),
                typeof(CrankCircleMateria),
                typeof(CrankLineMateria),
                typeof(CurtainBreathMateria),
                typeof(CurtainCircleMateria),
                typeof(CurtainLineMateria),
                typeof(DeerBreathMateria),
                typeof(DeerCircleMateria),
                typeof(DeerLineMateria),
                typeof(DVortexBreathMateria),
                typeof(DVortexCircleMateria),
                typeof(DVortexLineMateria),
                typeof(FlaskBreathMateria),
                typeof(FlaskCircleMateria),
                typeof(FlaskLineMateria),
                typeof(FTreeCircleMateria),
                typeof(FTreeLineMateria),
                typeof(GasBreathMateria),
                typeof(GasCircleMateria),
                typeof(GasLineMateria),
                typeof(GateBreathMateria),
                typeof(GateCircleMateria),
                typeof(GateLineMateria),
                typeof(GlowBreathMateria),
                typeof(GlowCircleMateria),
                typeof(GlowLineMateria),
                typeof(GuillotineBreathMateria),
                typeof(GuillotineCircleMateria),
                typeof(GuillotineLineMateria),
                typeof(HeadBreathMateria),
                typeof(HeadCircleMateria),
                typeof(HeadLineMateria),
                typeof(HeartBreathMateria),
                typeof(HeartCircleMateria),
                typeof(HeartLineMateria),
                typeof(MaidenBreathMateria),
                typeof(MaidenCircleMateria),
                typeof(MaidenLineMateria),
                typeof(MushroomBreathMateria),
                typeof(MushroomCircleMateria),
                typeof(MushroomLineMateria),
                typeof(NutcrackerBreathMateria),
                typeof(NutcrackerCircleMateria),
                typeof(NutcrackerLineMateria),
                typeof(ParaBreathMateria),
                typeof(ParaCircleMateria),
                typeof(ParaLineMateria),
                typeof(RuneBreathMateria),
                typeof(RuneCircleMateria),
                typeof(RuneLineMateria),
                typeof(SawBreathMateria),
                typeof(SawCircleMateria),
                typeof(SawLineMateria),
                typeof(SkeletonBreathMateria),
                typeof(SkeletonCircleMateria),
                typeof(SkeletonLineMateria),
                typeof(SkullBreathMateria),
                typeof(SkullCircleMateria),
                typeof(SkullLineMateria),
                typeof(SmokeBreathMateria),
                typeof(SmokeCircleMateria),
                typeof(SmokeLineMateria),
                typeof(WaterBreathMateria),
                typeof(WaterCircleMateria),
                typeof(WaterLineMateria),
            };
        }
    }
}
