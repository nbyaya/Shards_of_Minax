using System;
using System.Collections;
using Server.Mobiles;
using Server.Network;
using Server.Targeting;
using Server.Spells;
using Server.Items;

namespace Server.ACC.CSS.Systems.NinjitsuMagic
{
    public class SmokeBomb : NinjitsuSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Smoke Bomb", "Obscuro",
            21004,
            9300
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Third; }
        }

        public override double CastDelay { get { return 0.2; } }
        public override double RequiredSkill { get { return 50.0; } }
        public override int RequiredMana { get { return 20; } }

        public SmokeBomb(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Point3D p = Caster.Location;
                Map map = Caster.Map;

                if (map == null)
                    return;

                Effects.PlaySound(p, map, 0x22F); // Smoke bomb sound effect
                Effects.SendLocationEffect(p, map, 0x3728, 16, 10, 1150, 0); // Smoke visual effect

                new SmokeCloud(p, map, Caster);
            }

            FinishSequence();
        }

        private class SmokeCloud : Item
        {
            private Timer m_Timer;
            private Mobile m_Caster;

            public override bool BlocksFit { get { return true; } }

            public SmokeCloud(Point3D loc, Map map, Mobile caster) : base(0x372A)
            {
                Visible = true;
                Movable = false;
                MoveToWorld(loc, map);

                m_Caster = caster;

                m_Timer = new InternalTimer(this, caster);
                m_Timer.Start();
            }

            public SmokeCloud(Serial serial) : base(serial)
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

            private class InternalTimer : Timer
            {
                private SmokeCloud m_Item;
                private Mobile m_Caster;
                private int m_Ticks;

                public InternalTimer(SmokeCloud item, Mobile caster) : base(TimeSpan.FromSeconds(0.5), TimeSpan.FromSeconds(1.0))
                {
                    Priority = TimerPriority.TwoFiftyMS;
                    m_Item = item;
                    m_Caster = caster;
                }

                protected override void OnTick()
                {
                    if (m_Item.Deleted)
                    {
                        Stop();
                        return;
                    }

                    m_Ticks++;

                    if (m_Ticks >= 10) // Cloud lasts for 10 seconds
                    {
                        m_Item.Delete(); // Remove the cloud
                        Stop();
                        return;
                    }

                    ArrayList list = new ArrayList();

                    foreach (Mobile m in m_Item.GetMobilesInRange(3))
                    {
                        if (m != m_Caster && m.Player && m.Alive)
                            list.Add(m);
                    }

                    foreach (Mobile m in list)
                    {
                        if (m.CanBeHarmful(m_Caster, false))
                        {
                            m.PlaySound(0x1FB); // Miss sound effect
                            m.SendMessage("Your vision is obscured by the smoke!");

                            // Apply a debuff to simulate reduced accuracy
                            new AccuracyDebuff(m).Start(); // Apply the debuff
                        }
                    }
                }
            }
        }

        private class AccuracyDebuff : Timer
        {
            private Mobile m_Target;

            public AccuracyDebuff(Mobile target) : base(TimeSpan.FromSeconds(0.0), TimeSpan.FromSeconds(1.0))
            {
                m_Target = target;
                Priority = TimerPriority.TwoFiftyMS;
            }

            protected override void OnTick()
            {
                if (m_Target != null && !m_Target.Deleted && m_Target.Alive)
                {
                    m_Target.SendMessage("Your vision clears and your accuracy returns to normal.");
                    // Logic to reset the debuff goes here
                    Stop();
                }
            }
        }
    }
}
