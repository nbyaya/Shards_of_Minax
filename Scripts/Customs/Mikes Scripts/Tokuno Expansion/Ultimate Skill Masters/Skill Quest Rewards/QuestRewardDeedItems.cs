using Server.Items;
using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Customs.Mikes_Scripts.Skill_Masters.Ultimate_Skill_Masters.Skill_Quest_Rewards
{
    internal class QuestRewardDeedItems : IRewardGroup
    {
        public int Weight => 30; // Set the weight for this group

        public Type[] GetRewards(PlayerMobile player, SkillName? skill)
        {
            return new[]
            {
                typeof(ArmSlotChangeDeed),
                typeof(BeltSlotChangeDeed),
                typeof(BraceletSlotChangeDeed),
                typeof(CapacityIncreaseDeed),
                typeof(ChestSlotChangeDeed),
                typeof(EarringSlotChangeDeed),
                typeof(FootwearSlotChangeDeed),
                typeof(HeadSlotChangeDeed),
                typeof(LegsSlotChangeDeed),
                typeof(NeckSlotChangeDeed),
                typeof(OneHandedTransformDeed),
                typeof(RingSlotChangeDeed),
                typeof(ShirtSlotChangeDeed),
                typeof(SocketDeed),
                typeof(SocketDeed1),
                typeof(SocketDeed2),
                typeof(SocketDeed3),
                typeof(SocketDeed4),
                typeof(SocketDeed5),
                typeof(TalismanSlotChangeDeed),
            };
        }
    }
}
