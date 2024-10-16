using System;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Misc;
using Server.Targeting;
using Server.Items;

namespace Server.ACC.CSS.Systems.CampingMagic
{
    public class NightVision : CampingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Night Vision", "In Lumina",
            21005,
            9301
        );

        public override SpellCircle Circle { get { return SpellCircle.Second; } }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 20; } }

        public NightVision(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            // Get the target location of the caster
            Caster.Target = new InternalTarget(this);
        }

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target cannot be seen.
            }
            else if (CheckSequence())
            {
                SpellHelper.Turn(Caster, p);

                // Apply Night Vision effect to the caster and allies
                Effects.PlaySound(p, Caster.Map, 0x29);
                Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x376A, 9, 32, 5008);
                
                // Apply night vision effect to all allies within 5 tile radius
                foreach (Mobile m in Caster.GetMobilesInRange(5))
                {
                    if (m is PlayerMobile && m.Alive && m.Karma >= 0) // Only affect players who are alive and non-negative karma
                    {
                        m.SendMessage("Your vision sharpens, allowing you to see clearly in the dark.");
                        m.LightLevel = 21; // Sets vision level to daylight

                        Timer.DelayCall(TimeSpan.FromMinutes(1), () => // Effect duration is 1 minute
                        {
                            if (m != null && !m.Deleted && m.Alive)
                            {
                                m.LightLevel = 0; // Reset vision level to normal
                                m.SendMessage("Your night vision fades.");
                            }
                        });

                        // Add visual and sound effects
                        m.FixedParticles(0x376A, 9, 32, 5008, EffectLayer.Waist);
                        m.PlaySound(0x1E4);
                    }
                }
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private NightVision m_Owner;

            public InternalTarget(NightVision owner) : base(12, false, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is IPoint3D)
                    m_Owner.Target((IPoint3D)o);
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }
    }
}
