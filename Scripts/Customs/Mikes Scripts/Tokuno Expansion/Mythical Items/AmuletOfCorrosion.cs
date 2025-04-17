using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class AmuletOfCorrosion : GoldNecklace
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public AmuletOfCorrosion()
        {
            Weight = 1.0;
            Name = "Amulet of Corrosion";
            Hue = 1369; // Acidic green color

            // Set attributes and bonuses
            Attributes.BonusInt = 15;
            Attributes.RegenMana = 5;
            Attributes.SpellDamage = 10;
            Attributes.LowerManaCost = 10;
            
            Resistances.Poison = 15;
            Resistances.Physical = 5;
            Resistances.Fire = 5;
            Resistances.Energy = 5;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public AmuletOfCorrosion(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel an acidic presence augment your control over creatures.");

                // Start summon timer
                StopSummonTimer();
                m_Timer = new SummonCorrosiveSlimeTimer(pm);
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
                pm.SendMessage(37, "The acidic presence fades, reducing your control over creatures.");
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
            list.Add("Summons Corrosive Slimes");
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
                m_Timer = new SummonCorrosiveSlimeTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonCorrosiveSlimeTimer : Timer
        {
            private Mobile m_Owner;

            public SummonCorrosiveSlimeTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(10.0), TimeSpan.FromSeconds(10.0)) // Summon every 10 seconds
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                // Stop if owner is invalid or the item is no longer equipped
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Neck) is AmuletOfCorrosion))
                {
                    Stop();
                    return;
                }

                // Check if the player has autosummon enabled
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                // Ensure there is room for new followers before summoning
                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    CorrosiveSlime slime = new CorrosiveSlime
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    slime.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Corrosive Slime slithers forth to serve you!");
                }
            }
        }
    }
}
