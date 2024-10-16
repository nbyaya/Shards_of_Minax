using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.EvalIntMagic
{
    public class MysticMissileSpell : EvalIntSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Mystic Missile", "Vez Yera",
            // SpellCircle.Fifth,
            21005, // Animation ID for spell
            9401   // Effect ID for spell
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fifth; }
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 60.0; } }
        public override int RequiredMana { get { return 15; } }

        public MysticMissileSpell(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                // Target the enemy
                Caster.Target = new InternalTarget(this);
            }
            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private MysticMissileSpell m_Owner;

            public InternalTarget(MysticMissileSpell owner) : base(12, false, TargetFlags.Harmful)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object o)
            {
                if (o is Mobile target)
                {
                    if (target != null && target != from && from.CanSee(target) && from.InLOS(target))
                    {
                        // Launch the missiles
                        LaunchMissiles(from, target);
                    }
                }
                else
                {
                    from.SendMessage("You cannot target that.");
                }
            }

            private void LaunchMissiles(Mobile caster, Mobile target)
            {
                // Get the caster's Intelligence to scale damage
                double intelligenceBonus = caster.Int / 10.0; // For scaling damage (e.g., 1 damage per 10 Int)

                for (int i = 0; i < 5; i++) // Launch 5 missiles
                {
                    Timer.DelayCall(TimeSpan.FromMilliseconds(100 * i), () =>
                    {
                        // Create and send the missile
                        Missile missile = new Missile(caster, target, intelligenceBonus);
                        missile.MoveToWorld(caster.Location, caster.Map);

                        Effects.PlaySound(caster.Location, caster.Map, 0x1F5); // Play a magical sound
                    });
                }
            }
        }

        private class Missile : Item
        {
            private Mobile m_Caster;
            private Mobile m_Target;
            private double m_Damage;

            public Missile(Mobile caster, Mobile target, double damage) : base(0x1F4)
            {
                m_Caster = caster;
                m_Target = target;
                m_Damage = damage;
                Visible = false;
                Movable = false;

                Timer.DelayCall(TimeSpan.FromSeconds(1.0), () => HitTarget());
            }

            private void HitTarget()
            {
                if (m_Target != null && !m_Target.Deleted && m_Caster.InLOS(m_Target))
                {
                    m_Target.FixedEffect(0x1F4, 10, 16); // Visual effect for the missile hitting the target
                    AOS.Damage(m_Target, m_Caster, (int)m_Damage, 0, 0, 0, 0, 0); // Apply damage

                    m_Target.SendMessage($"You were hit by a Mystic Missile for {m_Damage} damage!");

                    // Optionally, you can add additional effects or sounds here
                }
                Delete(); // Remove the missile item after use
            }

            public Missile(Serial serial) : base(serial)
            {
            }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);
                writer.Write(0); // version
                writer.Write(m_Caster);
                writer.Write(m_Target);
                writer.Write(m_Damage);
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);
                int version = reader.ReadInt();
                m_Caster = reader.ReadMobile();
                m_Target = reader.ReadMobile();
                m_Damage = reader.ReadDouble();
            }
        }
    }
}
