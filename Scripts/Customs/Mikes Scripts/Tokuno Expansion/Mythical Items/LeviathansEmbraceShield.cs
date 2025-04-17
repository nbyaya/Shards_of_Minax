using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class LeviathansEmbraceShield : MetalShield
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public LeviathansEmbraceShield()
        {
            Weight = 6.0;
            Name = "Leviathan's Embrace";
            Hue = 1365; // Deep blue to represent the ocean

            // Set attributes and bonuses
            Attributes.BonusHits = 25;
            Attributes.BonusMana = 15;
            Attributes.DefendChance = 20;
            Attributes.RegenHits = 5;
            Attributes.RegenMana = 3;
            Attributes.SpellDamage = 10;

            PhysicalBonus = 15;
            FireBonus = 5;
            ColdBonus = 20;
            PoisonBonus = 10;
            EnergyBonus = 15;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public LeviathansEmbraceShield(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel the ocean's might empowering your command!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonLeviathanTimer(pm);
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
                pm.SendMessage(37, "The ocean's might recedes, and your command weakens.");
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
            list.Add("Summons Leviathans");
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
            if (Parent is Mobile mob && AutoSummonManager.IsAutoSummonEnabled(mob))
            {
                m_Timer = new SummonLeviathanTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonLeviathanTimer : Timer
        {
            private Mobile m_Owner;

            public SummonLeviathanTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(20.0), TimeSpan.FromSeconds(20.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.TwoHanded) is LeviathansEmbraceShield))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                // Only summon if the player has room for more followers
                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Leviathan leviathan = new Leviathan
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    leviathan.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Leviathan emerges from the depths to serve you!");
                }
            }
        }
    }
}
