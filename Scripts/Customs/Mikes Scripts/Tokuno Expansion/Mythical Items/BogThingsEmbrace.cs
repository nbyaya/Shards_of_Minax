using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class BogThingsEmbrace : LeatherChest
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public BogThingsEmbrace()
        {
            Weight = 6.0;
            Name = "BogThing's Embrace";
            Hue = 1267; // Swamp-like green

            // Set attributes and bonuses
            Attributes.BonusStr = 15;
            Attributes.RegenHits = 3;
            Attributes.RegenStam = 3;
            Attributes.DefendChance = 10;
            Attributes.Luck = 100;

            PhysicalBonus = 12;
            FireBonus = 8;
            ColdBonus = 10;
            PoisonBonus = 15;
            EnergyBonus = 8;

            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 10.0);
            SkillBonuses.SetValues(1, SkillName.Veterinary, 10.0);
            SkillBonuses.SetValues(2, SkillName.MagicResist, 15.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public BogThingsEmbrace(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel a connection to the swamp's creatures!");

                // Start summon timer only if auto-summon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonBogThingTimer(pm);
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
                pm.SendMessage(37, "The swamp's connection fades.");
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
            list.Add("Summons Bog Things");
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
                    m_Timer = new SummonBogThingTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonBogThingTimer : Timer
        {
            private Mobile m_Owner;

            public SummonBogThingTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(12.0), TimeSpan.FromSeconds(12.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.InnerTorso) is BogThingsEmbrace))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    BogThing bogThing = new BogThing
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    bogThing.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Bog Thing rises from the swamp to aid you!");
                }
            }
        }
    }
}
