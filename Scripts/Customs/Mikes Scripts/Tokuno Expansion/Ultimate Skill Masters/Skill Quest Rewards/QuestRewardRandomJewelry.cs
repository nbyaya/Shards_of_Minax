using Server.Mobiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Customs.Mikes_Scripts.Skill_Masters.Ultimate_Skill_Masters.Skill_Quest_Rewards
{
    public class QuestRewardRandomJewelry : IRewardGroup
    {
        public int Weight => 20; // Set the weight for this group

        public Type[] GetRewards(PlayerMobile player, SkillName? skill)
        {
            return new[]
            {
                typeof(RandomMagicJewelry),
                 typeof(RandomSkillJewelryA),
                 typeof(RandomSkillJewelryAA),
                 typeof(RandomSkillJewelryAB),
                 typeof(RandomSkillJewelryAC),
                 typeof(RandomSkillJewelryAD),
                 typeof(RandomSkillJewelryAE),
                 typeof(RandomSkillJewelryAF),
                 typeof(RandomSkillJewelryAG),
                 typeof(RandomSkillJewelryAH),
                 typeof(RandomSkillJewelryAI),
                 typeof(RandomSkillJewelryAJ),
                 typeof(RandomSkillJewelryAK),
                 typeof(RandomSkillJewelryAL),
                 typeof(RandomSkillJewelryAM),
                 typeof(RandomSkillJewelryAN),
                 typeof(RandomSkillJewelryAO),
                 typeof(RandomSkillJewelryAP),
                 typeof(RandomSkillJewelryB),
                 typeof(RandomSkillJewelryC),
                 typeof(RandomSkillJewelryD),
                 typeof(RandomSkillJewelryE),
                 typeof(RandomSkillJewelryF),
                 typeof(RandomSkillJewelryG),
                 typeof(RandomSkillJewelryH),
                 typeof(RandomSkillJewelryI),
                 typeof(RandomSkillJewelryJ),
                 typeof(RandomSkillJewelryK),
                 typeof(RandomSkillJewelryL),
                 typeof(RandomSkillJewelryM),
                 typeof(RandomSkillJewelryN),
                 typeof(RandomSkillJewelryO),
                 typeof(RandomSkillJewelryP),
                 typeof(RandomSkillJewelryQ),
                 typeof(RandomSkillJewelryR),
                 typeof(RandomSkillJewelryS),
                 typeof(RandomSkillJewelryT),
                 typeof(RandomSkillJewelryU),
                 typeof(RandomSkillJewelryV),
                 typeof(RandomSkillJewelryW),
                 typeof(RandomSkillJewelryY),
                 typeof(RandomSkillJewelryZ),
            };
        }
    }
}
