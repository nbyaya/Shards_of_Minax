using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Spells;
using Server.Targeting;

namespace Server.ACC.CSS.Systems.CarpentryMagic
{
    public class SpikeTrap : CarpentrySpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Spike Trap",
            "Lignum Aculeus",
            -1,
            9300

        );

        public override SpellCircle Circle => SpellCircle.Third;
        public override double RequiredSkill => 60.0;
        public override int RequiredMana => 30;

        public SpikeTrap(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            Caster.Target = new InternalTarget(this);
        }

        public void Target(IPoint3D p)
        {
            if (!Caster.CanSee(p))
            {
                Caster.SendLocalizedMessage(500237); // Target can not be seen.
            }
            else if (SpellHelper.CheckTown(p, Caster) && CheckSequence())
            {
                SpellHelper.Turn(Caster, p);

                Point3D loc = new Point3D(p);

                Effects.SendLocationParticles(EffectItem.Create(loc, Caster.Map, EffectItem.DefaultDuration), 
                    0x376A, 9, 10, 5025);

                new WoodSpikeTrap(loc, Caster.Map, Caster);

                Caster.SendLocalizedMessage(1062278); // You set the trap.
            }

            FinishSequence();
        }

        private class InternalTarget : Target
        {
            private SpikeTrap m_Owner;

            public InternalTarget(SpikeTrap owner) : base(12, true, TargetFlags.None)
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

    public class WoodSpikeTrap : Item
    {
        private Mobile m_Owner;
        private DateTime m_ExpireTime;


        [Constructable]
        public WoodSpikeTrap(Point3D loc, Map map, Mobile owner) : base(0x11A0)
        {
            Movable = false;
            m_Owner = owner;
            MoveToWorld(loc, map);
            m_ExpireTime = DateTime.UtcNow + TimeSpan.FromMinutes(10);

            Timer.DelayCall(TimeSpan.FromMinutes(10), new TimerCallback(Delete));
        }

        public WoodSpikeTrap(Serial serial) : base(serial)
        {
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m.AccessLevel > AccessLevel.Player)
                return;

            if (m_Owner == null || m == m_Owner)
                return;

            if (m.Location == this.Location && m.AccessLevel == AccessLevel.Player && DateTime.UtcNow < m_ExpireTime)
            {
                m.Damage(Utility.RandomMinMax(20, 40), m_Owner);
                m.SendLocalizedMessage(1010524); // Ouch! You stepped on a spike!

                Effects.PlaySound(m.Location, m.Map, 0x22D);
                Effects.SendLocationParticles(EffectItem.Create(m.Location, m.Map, EffectItem.DefaultDuration), 
                    0x37CC, 10, 10, 2023);

                Delete();
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version

            writer.Write(m_Owner);
            writer.Write(m_ExpireTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            m_Owner = reader.ReadMobile();
            m_ExpireTime = reader.ReadDateTime();

            if (DateTime.UtcNow >= m_ExpireTime)
                Delete();
            else
                Timer.DelayCall(m_ExpireTime - DateTime.UtcNow, new TimerCallback(Delete));
        }
    }
}