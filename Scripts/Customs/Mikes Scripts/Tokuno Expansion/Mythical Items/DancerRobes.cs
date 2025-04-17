using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class DancerRobes : Robe
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public DancerRobes()
        {
            Weight = 1.0;
            Name = "Dancer's Robes";
            Hue = 1153; // Ethereal blue/purple color

            // Set attributes and bonuses
            ClothingAttributes.SelfRepair = 3;
            Attributes.BonusDex = 20;
            Attributes.BonusMana = 10;
            Attributes.RegenMana = 5;
            Attributes.DefendChance = 10;
            Attributes.CastSpeed = 1;
            Attributes.LowerManaCost = 10;

            Resistances.Physical = 8;
            Resistances.Fire = 8;
            Resistances.Cold = 10;
            Resistances.Poison = 5;
            Resistances.Energy = 10;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public DancerRobes(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel empowered to lead more creatures!");

                // Start summon timer
                StopSummonTimer();
                m_Timer = new SummonFanDancerTimer(pm);
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
                pm.SendMessage(37, "You feel your commanding power diminish.");
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
            list.Add("Summons Fan Dancers");
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
                m_Timer = new SummonFanDancerTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonFanDancerTimer : Timer
        {
            private Mobile m_Owner;

            public SummonFanDancerTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.OuterTorso) is DancerRobes))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    FanDancer dancer = new FanDancer
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    dancer.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Fan Dancer gracefully joins your cause!");
                }
            }
        }
    }
}
