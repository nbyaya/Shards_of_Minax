using System;
using System.Collections;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Network;
using Server.Targeting;
using Server.Spells;

namespace Server.ACC.CSS.Systems.CampingMagic
{
    public class CampfireCraft : CampingSpell
    {
        private static SpellInfo m_Info = new SpellInfo(
            "Campfire Craft", "Warmth of the Wild",
            //SpellCircle.First,
            21005,
            9301,
            false
        );

        public override SpellCircle Circle
        {
            get { return SpellCircle.Fourth; } // Arbitrary choice; adjust as needed
        }

        public override double CastDelay { get { return 0.1; } }
        public override double RequiredSkill { get { return 0.0; } }  // No skill requirement
        public override int RequiredMana { get { return 20; } }  // Mana cost

        public CampfireCraft(Mobile caster, Item scroll) : base(caster, scroll, m_Info)
        {
        }

        public override void OnCast()
        {
            if (CheckSequence())
            {
                Caster.SendMessage("You craft a campfire at your location.");
                Effects.PlaySound(Caster.Location, Caster.Map, 0x225); // Campfire sound
                Effects.SendLocationParticles(EffectItem.Create(Caster.Location, Caster.Map, EffectItem.DefaultDuration), 0x36BD, 9, 32, 5024);

                Point3D loc = Caster.Location;
                Map map = Caster.Map;

                if (map != null)
                {
                    CampfireItem campfire = new CampfireItem();
                    campfire.MoveToWorld(loc, map);
                    campfire.StartEffect(Caster); // Start the healing effect
                }
            }

            FinishSequence();
        }

        public override TimeSpan GetCastDelay()
        {
            return TimeSpan.FromSeconds(5);  // Cooldown time
        }

        private class CampfireItem : Item
        {
            private Timer m_Timer;
            private DateTime m_End;

            public CampfireItem() : base(0xDE3) // Campfire item ID
            {
                Movable = false;
                Name = "Healing Campfire";
            }

            public override void OnAfterDelete()
            {
                base.OnAfterDelete();
                if (m_Timer != null)
                    m_Timer.Stop();
            }

            public void StartEffect(Mobile caster)
            {
                m_End = DateTime.Now + TimeSpan.FromSeconds(30); // Duration of campfire effect
                m_Timer = new InternalTimer(this, caster, TimeSpan.FromSeconds(1.0));
                m_Timer.Start();
            }

            public CampfireItem(Serial serial) : base(serial)
            {
            }

            public override void Serialize(GenericWriter writer)
            {
                base.Serialize(writer);
                writer.Write((int)0); // version
                writer.Write(m_End - DateTime.Now);
            }

            public override void Deserialize(GenericReader reader)
            {
                base.Deserialize(reader);
                int version = reader.ReadInt();
                TimeSpan duration = reader.ReadTimeSpan();
                m_End = DateTime.Now + duration;
                StartEffect(null); // Restart effect on server reboot
            }

            private class InternalTimer : Timer
            {
                private readonly CampfireItem m_Campfire;
                private readonly Mobile m_Caster;

                public InternalTimer(CampfireItem campfire, Mobile caster, TimeSpan interval) : base(interval, interval)
                {
                    m_Campfire = campfire;
                    m_Caster = caster;
                }

                protected override void OnTick()
                {
                    if (m_Campfire.Deleted || DateTime.Now > m_Campfire.m_End)
                    {
                        m_Campfire.Delete();
                        Stop();
                        return;
                    }

                    ArrayList list = new ArrayList();
                    foreach (Mobile m in m_Campfire.GetMobilesInRange(5)) // Range of healing effect
                    {
                        if (m.Alive && m.CanBeBeneficial(m_Caster) && m != m_Caster && m.Player)
                        {
                            list.Add(m);
                        }
                    }

                    foreach (Mobile m in list)
                    {
                        m.Heal(5); // Heal amount
                        m.Stam += 5; // Stamina restore amount
                        m.FixedParticles(0x375A, 9, 32, 5027, EffectLayer.Waist); // Healing visual effect
                        m.PlaySound(0x1F7); // Healing sound
                    }
                }
            }
        }
    }
}
