using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class FrostlaceCloak : Cloak
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public FrostlaceCloak()
        {
            Weight = 1.0;
            Name = "Frostlace Cloak";
            Hue = 1152; // Icy blue color

            // Set attributes and bonuses
            ClothingAttributes.SelfRepair = 5;
            Attributes.BonusInt = 15;
            Attributes.RegenMana = 3;
            Attributes.LowerManaCost = 10;
            Attributes.SpellDamage = 10;
            Attributes.DefendChance = 10;

            Resistances.Cold = 20;
            Resistances.Physical = 10;
            Resistances.Fire = 5;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public FrostlaceCloak(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel the icy power of control over more creatures!");

                // Start summon timer only if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonIceElementalTimer(pm);
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
                pm.SendMessage(37, "The icy power fades, and you feel less control over creatures.");
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
            list.Add("Summons Ice Elementals");
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
                    m_Timer = new SummonIceElementalTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonIceElementalTimer : Timer
        {
            private Mobile m_Owner;

            public SummonIceElementalTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0)) // Summon every 15 seconds
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Cloak) is FrostlaceCloak))
                {
                    Stop();
                    return;
                }

                // Only summon if autosummon is enabled
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                // Summon only if the player has room for more followers
                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    IceElemental elemental = new IceElemental
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    elemental.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "An Ice Elemental rises to serve you!");
                }
            }
        }
    }
}
