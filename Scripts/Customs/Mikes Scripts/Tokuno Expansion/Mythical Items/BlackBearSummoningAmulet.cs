using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class BlackBearSummoningAmulet : BaseJewel
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public BlackBearSummoningAmulet() : base(0x1088, Layer.Neck)
        {
            Weight = 1.0;
            Name = "Amulet of the Bear Clan";
            Hue = 1109; // Dark earthy tone

            // Set attributes and bonuses
            Attributes.BonusStr = 25;
            Attributes.RegenHits = 3;
            Attributes.DefendChance = 10;
            Attributes.NightSight = 1;
            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 15.0);
            SkillBonuses.SetValues(1, SkillName.AnimalLore, 10.0);

            Resistances.Physical = 12;
            Resistances.Fire = 8;
            Resistances.Cold = 15;
            Resistances.Poison = 10;
            Resistances.Energy = 10;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public BlackBearSummoningAmulet(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel the strength of the bear coursing through you!");

                // Start summon timer if auto summon is enabled
                StopSummonTimer();

                // Check if auto-summon is enabled before starting the timer
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonBlackBearTimer(pm);
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
                pm.SendMessage(37, "The strength of the bear fades away.");
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
            list.Add("Summons Black Bears");
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
                // Start the timer only if auto-summon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonBlackBearTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonBlackBearTimer : Timer
        {
            private Mobile m_Owner;

            public SummonBlackBearTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Neck) is BlackBearSummoningAmulet))
                {
                    Stop();
                    return;
                }

                // Only summon if auto-summon is enabled
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    BlackBear bear = new BlackBear
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    bear.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Black Bear emerges to serve you!");
                }
            }
        }
    }
}
