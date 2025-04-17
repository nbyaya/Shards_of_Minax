using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class WhiskersCloak : Cloak
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public WhiskersCloak()
        {
            Weight = 1.0;
            Name = "Whisker's Cloak";
            Hue = 1153; // Soft grey with a slight sheen, resembling a cat's fur.

            // Set attributes and bonuses
            Attributes.BonusDex = 25; // Agility of a cat
            Attributes.BonusStam = 20; // Stamina for swift movement
            Attributes.NightSight = 1; // Cats have excellent night vision
            Attributes.Luck = 200; // Cats are often considered lucky
            Attributes.DefendChance = 15; // Reflexes to avoid attacks

            Resistances.Physical = 8; // Nimble and quick
            Resistances.Cold = 8; // Adaptable fur
            Resistances.Poison = 12; // Hardy constitution

            // Attach XmlLevelItem for integration
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2; // Increase in max followers
        }

        public WhiskersCloak(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel an affinity with feline companions!");

                // Start summon timer if auto-summon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonCatTimer(pm);
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
                pm.SendMessage(37, "Your bond with feline companions wanes.");
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
            list.Add("Summons Cats to follow you");
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
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonCatTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonCatTimer : Timer
        {
            private Mobile m_Owner;

            public SummonCatTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Cloak) is WhiskersCloak))
                {
                    Stop();
                    return;
                }

                // Check if auto-summon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                // Only summon if the player has room for more followers
                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Cat cat = new Cat
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    cat.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A cat appears and rubs against your leg before following you!");
                }
            }
        }
    }
}
