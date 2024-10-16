using System;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using System.Collections;

namespace Server.ACC.CSS.Systems.LockpickingMagic
{
    public class SecurityVeil : LockpickingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Security Veil", "Securum Velum",
            21001, 9301
        );

        public override SpellCircle Circle { get { return SpellCircle.Second; } }
        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 30.0; } }
        public override int RequiredMana { get { return 10; } }

        public SecurityVeil(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.Target = new InternalTarget(this);
            }
        }

        private void Target(Mobile target)
        {
            if (target == null || target.Deleted || !Caster.CanBeBeneficial(target, false))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
                return;
            }

            if (CheckSequence())
            {
                Caster.DoBeneficial(target);
                Effects.PlaySound(Caster.Location, Caster.Map, 0x1ED); // Magical sound effect
                Caster.FixedParticles(0x375A, 10, 15, 5018, EffectLayer.Waist); // Visual effect

                SecurityVeilItem barrier = new SecurityVeilItem(target);
                barrier.MoveToWorld(target.Location, target.Map);

                Timer.DelayCall(TimeSpan.FromSeconds(10.0), barrier.Delete); // Barrier lasts for 10 seconds

                target.SendMessage("A magical barrier surrounds you, protecting you from harm!");

                target.BeginAction(typeof(SecurityVeil)); // Prevents multiple uses concurrently

                Timer.DelayCall(TimeSpan.FromSeconds(10.0), delegate
                {
                    target.EndAction(typeof(SecurityVeil));
                    target.SendMessage("The magical barrier dissipates.");
                });
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private SecurityVeil m_Owner;

            public InternalTarget(SecurityVeil owner) : base(12, false, TargetFlags.Beneficial)
            {
                m_Owner = owner;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (targeted is Mobile)
                {
                    m_Owner.Target((Mobile)targeted);
                }
            }

            protected override void OnTargetFinish(Mobile from)
            {
                m_Owner.FinishSequence();
            }
        }

        private class SecurityVeilItem : Item
        {
            private Mobile m_Target;

            public SecurityVeilItem(Mobile target) : base(0x1F14) // Use a suitable graphic
            {
                Movable = false;
                Visible = false;
                m_Target = target;

                Timer.DelayCall(TimeSpan.FromSeconds(1.0), MakeVisible); // Delay visibility for effect
                Timer.DelayCall(TimeSpan.FromSeconds(10.0), Expire);
            }

            private void MakeVisible()
            {
                if (Deleted) return;
                Visible = true;
                Effects.SendLocationEffect(Location, Map, 0x373A, 10, 1, 1153, 0); // Visual effect on location
            }

            private void Expire()
            {
                if (Deleted) return;
                Delete();
                m_Target.SendMessage("The barrier protecting you has expired.");
            }

            public override void OnAfterDelete()
            {
                base.OnAfterDelete();
                if (m_Target != null)
                {
                    m_Target.SendMessage("The barrier has been removed.");
                }
            }

            public SecurityVeilItem(Serial serial) : base(serial)
            {
            }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);
                writer.Write((int)0); // version
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);
                int version = reader.ReadInt();
            }
        }
    }
}
