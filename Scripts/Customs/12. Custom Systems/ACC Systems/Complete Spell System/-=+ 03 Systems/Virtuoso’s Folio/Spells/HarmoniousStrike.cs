using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.MusicianshipMagic
{
    public class HarmoniousStrike : MusicianshipSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
                                                        "Harmonious Strike", "Lyrica Tonitrua",
                                                        // SpellCircle.First,
                                                        21004,
                                                        9300
                                                       );

        public override SpellCircle Circle
        {
            get { return SpellCircle.First; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 20; } }

        public HarmoniousStrike(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(Mobile m)
        {
            if (!Caster.CanSee(m))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (CheckHSequence(m))
            {
                if (m is Mobile target)
                {
                    // Deal damage and apply stun
                    double damage = Utility.RandomMinMax(10, 30); // Adjust damage range as needed
                    SpellHelper.Damage(TimeSpan.FromSeconds(0.5), target, Caster, damage, 0, 100, 0, 0, 0); // Pure energy damage

                    // Stun effect
                    target.Freeze(TimeSpan.FromSeconds(2.0)); // Stun duration

                    // Visual and sound effects
                    target.PlaySound(0x5C3); // Sound of musical note
                    target.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist); // Visual effect

                    // Additional visual flair
                    Effects.SendMovingParticles(new Entity(Serial.Zero, new Point3D(Caster.Location), Caster.Map), target, 0x36E4, 1, 0, false, false, 0x0, 0, 9501, 0, 0, 0); // Musical note moving towards the target
                    Caster.PlaySound(0x58E); // Caster's sound effect
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private HarmoniousStrike m_Owner;

            public InternalTarget(HarmoniousStrike owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile)
                    m_Owner.Target((Mobile)o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
