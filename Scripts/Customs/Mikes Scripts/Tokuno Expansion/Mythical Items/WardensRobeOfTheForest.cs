using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class WardensRobeOfTheForest : Robe
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public WardensRobeOfTheForest()
        {
            Weight = 2.0;
            Name = "Warden's Robe of the Forest";
            Hue = 2124; // Forest green color

            // Set attributes and bonuses
            ClothingAttributes.SelfRepair = 5;
            Attributes.BonusDex = 20;
            Attributes.RegenStam = 3;
            Attributes.RegenMana = 2;
            Attributes.DefendChance = 10;
            Attributes.Luck = 100;
            Attributes.NightSight = 1;

            Resistances.Physical = 8;
            Resistances.Fire = 5;
            Resistances.Cold = 10;
            Resistances.Poison = 15;
            Resistances.Energy = 8;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public WardensRobeOfTheForest(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel the forest granting you control over its creatures!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonForestOstardTimer(pm);
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
                pm.SendMessage(37, "The forest's connection fades, and your command weakens.");
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
            list.Add("Summons Forest Ostards");
            list.Add("Increases maximum followers");
            list.Add("Grants stamina and mana regeneration");
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
                // Check if autosummon is enabled when the item is equipped
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonForestOstardTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonForestOstardTimer : Timer
        {
            private Mobile m_Owner;

            public SummonForestOstardTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.OuterTorso) is WardensRobeOfTheForest))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                // Only summon if the player has room for more followers
                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    ForestOstard ostard = new ForestOstard
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    ostard.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Forest Ostard emerges from the wilderness to serve you!");
                }
            }
        }
    }
}
