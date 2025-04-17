using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class GargoylesEmbrace : LeatherGloves
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public GargoylesEmbrace()
        {
            Weight = 2.0;
            Name = "Gargoyle's Embrace";
            Hue = 1175; // Deep stone color

            // Set attributes and bonuses
            ArmorAttributes.SelfRepair = 4;
            Attributes.BonusStr = 10;
            Attributes.BonusHits = 20;
            Attributes.DefendChance = 15;
            Attributes.RegenHits = 5;
            Attributes.RegenMana = 3;
            Attributes.Luck = 150;

            PhysicalBonus = 15;
            FireBonus = 10;
            ColdBonus = 10;
            PoisonBonus = 5;
            EnergyBonus = 10;

            SkillBonuses.SetValues(0, SkillName.Fencing, 20.0);
            SkillBonuses.SetValues(1, SkillName.Mysticism, 15.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public GargoylesEmbrace(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel a mystical connection to gargoyle spirits!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonGargishOutcastTimer(pm);
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
                pm.SendMessage(37, "The connection to the gargoyle spirits fades.");
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
            list.Add("Summons Gargish Outcasts");
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
                m_Timer = new SummonGargishOutcastTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonGargishOutcastTimer : Timer
        {
            private Mobile m_Owner;

            public SummonGargishOutcastTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Waist) is GargoylesEmbrace))
                {
                    Stop();
                    return;
                }

                // Check if auto-summon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    GargishOutcast outcast = new GargishOutcast
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    outcast.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Gargish Outcast appears to aid you!");
                }
            }
        }
    }
}
