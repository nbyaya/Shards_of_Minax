using System;
using Server;
using Server.Mobiles;
using Server.Targeting;
using Server.Network;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.EvalIntMagic
{
    public class ArcaneBlast : EvalIntSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Arcane Blast", "Vera Tareon",
            // Spell Circle
            266,
            9040
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; } // Adjust circle if needed
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 20; } }

        public ArcaneBlast(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        private class InternalTarget : Target
        {
            private ArcaneBlast m_Owner;

            public InternalTarget(ArcaneBlast owner) : base(12, true, TargetFlags.None)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is IPoint3D point && from.CanSee(point))
                {
                    if (m_Owner.CheckSequence())
                    {
                        Point3D targetLocation = new Point3D(point);
                        m_Owner.Caster.Location = targetLocation; // Adjust caster location for visual effect

                        // Deal damage based on caster's Intelligence
                        int damage = (int)(m_Owner.Caster.Skills[SkillName.Magery].Value * 0.5 + m_Owner.Caster.Int * 0.2);
                        from.SendMessage("You have been hit by an Arcane Blast!");
                        AOS.Damage(from, m_Owner.Caster, damage, 0, 0, 0, 0, 0);

                        // Play sound effect
                        Effects.PlaySound(targetLocation, from.Map, 0x20D);

                        // Visual effect (arcane explosion)
                        Effects.SendLocationEffect(targetLocation, from.Map, 0x36D4, 16, 5, 0x00FF, 0x0000);

                        // Optional: Add arcane particles or other effects
                        Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
                        {
                            Effects.SendLocationEffect(targetLocation, from.Map, 0x3709, 16, 5, 0x00FF, 0x0000);
                        });
                    }
                }
                else
                {
                    from.SendLocalizedMessage(500237); // Target can not be seen.
                }

                m_Owner.FinishSequence();
            }
        }
    }
}
