using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class CapybaraWhispererSash : BodySash
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public CapybaraWhispererSash()
        {
            Weight = 1.0;
            Name = "Capybara Whisperer's Sash";
            Hue = 2125; // A natural green hue

            // Set attributes and bonuses
            Attributes.BonusHits = 15;
            Attributes.RegenHits = 3;
            Attributes.RegenStam = 2;
            Attributes.BonusStam = 10;
            Attributes.DefendChance = 10;
            Attributes.NightSight = 1;

            Resistances.Physical = 8;
            Resistances.Fire = 5;
            Resistances.Cold = 8;
            Resistances.Poison = 12;
            Resistances.Energy = 5;

            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 20.0);
            SkillBonuses.SetValues(1, SkillName.Veterinary, 20.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public CapybaraWhispererSash(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel a connection to peaceful creatures!");

                // Start summon timer
                StopSummonTimer();
                m_Timer = new SummonCapybaraTimer(pm);
                m_Timer.Start();
            }
        }

        public override void OnRemoved(object parent)
        {
            base.OnRemoved(parent);

            if (parent is PlayerMobile pm)
            {
                // Remove follower bonus
                pm.FollowersMax -= m_BonusFollowers;
                pm.SendMessage(37, "The peaceful aura fades away.");
            }

            // Stop the summon timer
            StopSummonTimer();
        }

        private void StopSummonTimer()
        {
            if (m_Timer != null)
            {
                m_Timer.Stop();
                m_Timer = null;
            }
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Summons Capybaras");
            list.Add("Increases maximum followers");
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_BonusFollowers);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_BonusFollowers = reader.ReadInt();

            // Reinitialize timer if equipped on restart
            if (Parent is Mobile mob)
            {
                m_Timer = new SummonCapybaraTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonCapybaraTimer : Timer
        {
            private Mobile m_Owner;

            public SummonCapybaraTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.MiddleTorso) is CapybaraWhispererSash))
                {
                    Stop();
                    return;
                }

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Capybara capybara = new Capybara
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    capybara.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Capybara appears to accompany you!");
                }
            }
        }
    }

    public class Capybara : BaseCreature
    {
        public Capybara() : base(AIType.AI_Animal, FightMode.None, 10, 1, 0.3, 0.6)
        {
            Name = "a capybara";
            Body = 0x115; // Use an appropriate body ID for a capybara
            BaseSoundID = 0x85;

            SetStr(40, 50);
            SetDex(30, 40);
            SetInt(15, 20);

            SetHits(25, 30);
            SetStam(50, 60);

            SetDamage(3, 5);

            SetSkill(SkillName.Wrestling, 25.0, 30.0);
            SetSkill(SkillName.MagicResist, 15.0, 20.0);

            Fame = 0;
            Karma = 2000;

            VirtualArmor = 10;

            Tamable = false;
        }

        public Capybara(Serial serial) : base(serial)
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
