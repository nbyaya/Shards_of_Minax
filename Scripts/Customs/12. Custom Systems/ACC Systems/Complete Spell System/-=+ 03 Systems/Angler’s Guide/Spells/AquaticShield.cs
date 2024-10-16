using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.FishingMagic
{
    public class AquaticShield : FishingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Aquatic Shield", "Aqua Protego",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Second; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 15; } }

        public AquaticShield(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private AquaticShield m_Spell;

            public InternalTarget(AquaticShield spell) : base(12, false, TargetFlags.Beneficial)
            {
                m_Spell = spell;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                {
                    Mobile target = (Mobile)targeted;

                    if (m_Spell.CheckSequence())
                    {
                        Effects.PlaySound(target.Location, target.Map, 0x026); // Water sound effect

                        target.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist); // Water shield effect
                        target.PlaySound(0x026); // Water sound effect

                        // Apply shield effect
                        target.SendMessage("You are surrounded by a protective barrier of water!");
                        target.VirtualArmorMod += 20; // Increase armor temporarily

                        Timer.DelayCall(TimeSpan.FromSeconds(10), () =>
                        {
                            target.VirtualArmorMod -= 20; // Remove armor increase after 10 seconds
                            target.SendMessage("The protective barrier of water fades away.");
                        });
                    }
                }

                m_Spell.FinishSequence();
            }
        }
    }
}
