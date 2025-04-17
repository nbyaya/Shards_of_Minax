using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class DevourerCloak : Cloak
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public DevourerCloak()
        {
            Weight = 2.0;
            Name = "Cloak of the Devourer";
            Hue = 1175; // A dark, ominous hue

            // Set attributes and bonuses
            ClothingAttributes.SelfRepair = 3;
            Attributes.BonusStr = 20;
            Attributes.BonusHits = 30;
            Attributes.RegenHits = 5;
            Attributes.NightSight = 1;
            Attributes.Luck = 150;

            Resistances.Physical = 12;
            Resistances.Fire = 10;
            Resistances.Cold = 15;
            Resistances.Poison = 10;
            Resistances.Energy = 10;

            SkillBonuses.SetValues(0, SkillName.SpiritSpeak, 25.0);
            SkillBonuses.SetValues(1, SkillName.Necromancy, 25.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public DevourerCloak(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel empowered to summon greater forces of darkness!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonDevourerTimer(pm);
                    m_Timer.Start();
                }
            }
        }

        public override void OnRemoved(object parent)
        {
            base.OnRemoved(parent);

            if (parent is PlayerMobile pm)
            {
                // Remove follower bonus
                pm.FollowersMax -= m_BonusFollowers;
                pm.SendMessage(37, "The dark forces withdraw their favor.");
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
            list.Add("Summons Devourers");
            list.Add("Increases maximum followers by 2");
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
            if (Parent is Mobile mob && AutoSummonManager.IsAutoSummonEnabled(mob))
            {
                m_Timer = new SummonDevourerTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonDevourerTimer : Timer
        {
            private Mobile m_Owner;

            public SummonDevourerTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Cloak) is DevourerCloak))
                {
                    Stop();
                    return;
                }

                // Only summon if autosummon is enabled
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Devourer devourer = new Devourer
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    devourer.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Devourer emerges to do your bidding!");
                }
            }
        }
    }
}
