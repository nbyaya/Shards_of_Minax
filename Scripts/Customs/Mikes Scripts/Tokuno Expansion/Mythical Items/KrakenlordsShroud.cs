using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class KrakenlordsShroud : Cloak
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public KrakenlordsShroud()
        {
            Weight = 1.0;
            Name = "Krakenlord's Shroud";
            Hue = 1365; // Deep ocean blue

            // Set attributes and bonuses
            Attributes.BonusInt = 15;
            Attributes.BonusMana = 25;
            Attributes.RegenMana = 5;
            Attributes.SpellDamage = 10;
            Attributes.DefendChance = 10;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 1;

            Resistances.Physical = 5;
            Resistances.Cold = 15;
            Resistances.Poison = 5;
            Resistances.Energy = 10;

            SkillBonuses.SetValues(0, SkillName.Fishing, 20.0);
            SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 15.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 3; // Increase max followers by 3
        }

        public KrakenlordsShroud(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "The power of the seas surges within you!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonKrakenTimer(pm);
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
                pm.SendMessage(37, "You feel less attuned to the ocean's power.");
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
            list.Add("Summons Krakens to aid you");
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
                // Start the timer if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonKrakenTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonKrakenTimer : Timer
        {
            private Mobile m_Owner;

            public SummonKrakenTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(20.0), TimeSpan.FromSeconds(20.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                // Stop if the owner is invalid or the item is not equipped
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Cloak) is KrakenlordsShroud))
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
                    Kraken kraken = new Kraken
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    kraken.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Kraken emerges from the depths to serve you!");
                }
            }
        }
    }
}
