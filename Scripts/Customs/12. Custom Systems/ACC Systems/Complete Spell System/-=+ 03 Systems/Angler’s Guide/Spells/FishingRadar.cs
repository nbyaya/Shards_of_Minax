using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.FishingMagic
{
    public class FishingRadar : FishingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Fishing Radar",
            "Reveal Aquatic Bounty",
            266,
            9300
        );

        public override SpellCircle Circle => SpellCircle.First;
        public override double CastDelay => 0.1;
        public override double RequiredSkill => 30.0;
        public override int RequiredMana => 15;

        public FishingRadar(Mobile caster, Item scroll)
            : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.PlaySound(0x1ED); // Sonar ping sound
                Caster.FixedParticles(0x3779, 1, 30, 9502, 43, 3, EffectLayer.Waist); // Blue sparkles

                RevealFishAndResources();
            }

            FinishSequence();
        }

        private void RevealFishAndResources()
        {
            int range = (int)(Caster.Skills[CastSkill].Value / 10) + 5; // Range increases with skill
            foreach (IEntity entity in Caster.Map.GetObjectsInRange(Caster.Location, range))
            {
                if (entity is BaseFish fish)
                {
                    RevealFish(fish);
                }
            }

            Caster.SendLocalizedMessage(1010574, "", 0x35); // You sense aquatic life nearby!
        }

        private void RevealFish(BaseFish fish)
        {
            fish.PrivateOverheadMessage(MessageType.Regular, 0x14A, false, "Fish!", Caster.NetState);
            Effects.SendLocationParticles(EffectItem.Create(fish.Location, fish.Map, EffectItem.DefaultDuration),
                0x3789, 1, 40, 0x47D, 3, 9907, 0); // Watery bubble effect
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(2.5);
        }
    }
}