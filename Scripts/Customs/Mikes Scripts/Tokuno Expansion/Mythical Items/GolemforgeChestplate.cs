using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class GolemforgeChestplate : PlateChest
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public GolemforgeChestplate()
        {
            Name = "Golemforge Chestplate";
            Hue = 2406; // Metallic hue
            Weight = 10.0;

            // Attributes and bonuses
            Attributes.BonusStr = 20;
            Attributes.BonusHits = 25;
            Attributes.RegenHits = 3;
            Attributes.DefendChance = 15;
            Attributes.ReflectPhysical = 10;

            PhysicalBonus = 20;
            FireBonus = 10;
            ColdBonus = 15;
            PoisonBonus = 5;
            EnergyBonus = 10;

            SkillBonuses.SetValues(0, SkillName.Tinkering, 25.0);

            ArmorAttributes.SelfRepair = 5;
            ArmorAttributes.DurabilityBonus = 50;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public GolemforgeChestplate(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel the power of crafted metal at your command!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm)) // Check if autosummon is enabled
                {
                    m_Timer = new SummonGolemTimer(pm);
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
                pm.SendMessage(37, "The command over metal beings wanes.");
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
            list.Add("Summons Golems");
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
                m_Timer = new SummonGolemTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonGolemTimer : Timer
        {
            private Mobile m_Owner;

            public SummonGolemTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(20.0), TimeSpan.FromSeconds(20.0)) // Summon every 20 seconds
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                // Stop if the owner is invalid or the item is not equipped
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.InnerTorso) is GolemforgeChestplate))
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
                    IronGolem golem = new IronGolem
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    golem.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "An Iron Golem assembles to protect you!");
                }
            }
        }
    }
}
