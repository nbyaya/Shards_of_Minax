using Server.Items;
using Server.Mobiles;
using System;

namespace Server.Customs.Mikes_Scripts.Skill_Masters.Ultimate_Skill_Masters.Skill_Quest_Rewards
{
    internal class QuestRewardArmor : IRewardGroup
    {
        public int Weight => 30; // Set the weight for this group

        public Type[] GetRewards(PlayerMobile player, SkillName? skill)
        {
            return new[]
            {
                typeof(JesterHatOfCommand),
                typeof(CrownOfTheAbyss),
                typeof(HelmOfDarkness),
                typeof(MaskedAvengersVoice),
                typeof(RadiantCrown),
                typeof(GlovesOfTheSilentAssassin),
                typeof(PumpkinKingsCrown),
                typeof(ThiefsNimbleCap),
                typeof(NecromancersHood),
                typeof(DaggerSign),
                typeof(ChefsHatOfFocus),
                typeof(WarHeronsCap),
                typeof(WisdomsCirclet),
                typeof(PlateLeggingsOfCommand),
                typeof(DarkFathersHeartplate),
                typeof(BladeDancersPlateChest),
                typeof(FrostwardensPlateChest),
                typeof(MagitekInfusedPlate),
                typeof(NecromancersRobe),
                typeof(VestOfTheVeinSeeker),
                typeof(KnightsValorShield),
                typeof(FrostwardensPlateLegs),
                typeof(LeggingsOfTheRighteous),
                typeof(BladeDancersPlateLegs),
                typeof(WildwalkersGreaves),
                typeof(FortunesGorget),
                typeof(FortunesHelm),
                typeof(FortunesPlateArms),
                typeof(FortunesPlateChest),
                typeof(FortunesPlateLegs),
                typeof(GlovesOfTheSilentAssassin),
                typeof(CraftsmansProtectiveGloves),
                typeof(ArtisansCraftedGauntlets),
                typeof(HarmonyGauntlets),
                typeof(GlovesOfTransmutation),
                typeof(BootsOfCommand),
                typeof(BootsOfFleetness),
                typeof(HealersFurCape),
                typeof(NecromancersShadowBoots),
                typeof(SilentStepTabi),
                typeof(RingSlotChangeDeed),
                typeof(NecklaceOfAromaticProtection),
                typeof(SerenitysHelm),
                typeof(MysticsGuard),
                typeof(ShadowGripGloves),
                typeof(BeltSlotChangeDeed),
            };
        }
    }
}

