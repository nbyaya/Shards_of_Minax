using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class RingOfTheJukanCommander : GoldRing
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public RingOfTheJukanCommander()
        {
            Weight = 1.0;
            Name = "Ring of the Jukan Commander";
            Hue = 1157; // Dark metallic color

            // Set attributes and bonuses
            Attributes.BonusStr = 20;
            Attributes.BonusDex = 15;
            Attributes.AttackChance = 10;
            Attributes.DefendChance = 10;
            Attributes.RegenHits = 5;
            Attributes.RegenStam = 5;
            Attributes.LowerManaCost = 10;

            Resistances.Physical = 12;
            Resistances.Fire = 8;
            Resistances.Cold = 8;
            Resistances.Poison = 10;
            Resistances.Energy = 8;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public RingOfTheJukanCommander(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel empowered to command the Juka forces!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonJukaLordTimer(pm);
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
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
                pm.SendMessage(37, "You feel less capable of commanding the Juka forces.");
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
            list.Add("Summons Juka Lords");
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
                // Check if autosummon is enabled when the item is re-equipped
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonJukaLordTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonJukaLordTimer : Timer
        {
            private Mobile m_Owner;

            public SummonJukaLordTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(10.0), TimeSpan.FromSeconds(10.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Ring) is RingOfTheJukanCommander))
                {
                    Stop();
                    return;
                }

                // Only summon if the player has room for more followers and autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(m_Owner) && m_Owner.Followers < m_Owner.FollowersMax)
                {
                    JukaLord juka = new JukaLord
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    juka.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Juka Lord emerges to serve you!");
                }
            }
        }
    }
}
