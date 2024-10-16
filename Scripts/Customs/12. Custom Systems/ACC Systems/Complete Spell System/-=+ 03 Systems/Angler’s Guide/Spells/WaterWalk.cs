using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.FishingMagic
{
    public class WaterWalk : FishingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Water Walk", "Aqua Ambulare",
                                                        21004,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 40.0; } }
        public override int RequiredMana { get { return 15; } }

        public WaterWalk(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.PlaySound(0x64F); // Play water sound
                Caster.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist); // Water splash effect

                foreach (Mobile m in Caster.GetMobilesInRange(5)) // Apply effect to allies within 5 tiles
                {
                    if (m is PlayerMobile && m.Alive && m != Caster && m.InLOS(Caster))
                    {
                        m.SendMessage("You feel lighter and able to walk on water!");
                        m.Blessed = true; // Temporarily make them blessed to walk on water
                        Timer.DelayCall(TimeSpan.FromSeconds(30), () => m.Blessed = false); // Remove blessing after 30 seconds
                    }
                }

                Caster.SendMessage("You can now walk on water!");
                Caster.Blessed = true; // Temporarily make caster blessed to walk on water
                Timer.DelayCall(TimeSpan.FromSeconds(30), () => Caster.Blessed = false); // Remove blessing after 30 seconds
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(7.5);
        }
    }
}
