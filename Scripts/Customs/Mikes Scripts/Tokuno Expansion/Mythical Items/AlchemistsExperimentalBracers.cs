using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class AlchemistsExperimentalBracers : LeatherGloves
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public AlchemistsExperimentalBracers()
        {
            Weight = 2.0;
            Name = "Alchemist's Experimental Bracers";
            Hue = 1167; // A greenish hue for alchemy theme

            // Set attributes and bonuses
            ArmorAttributes.SelfRepair = 4;
            Attributes.BonusInt = 20;
            Attributes.RegenMana = 5;
            Attributes.EnhancePotions = 25;
            Attributes.SpellDamage = 10;

            PhysicalBonus = 5;
            FireBonus = 5;
            ColdBonus = 5;
            PoisonBonus = 15;
            EnergyBonus = 5;

            SkillBonuses.SetValues(0, SkillName.Alchemy, 30.0);
            SkillBonuses.SetValues(1, SkillName.MagicResist, 15.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public AlchemistsExperimentalBracers(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel empowered to command additional creations!");

                // Start summon timer
                StopSummonTimer();
                m_Timer = new SummonGreenGoblinTimer(pm);
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
                pm.SendMessage(37, "You feel less capable of commanding additional creations.");
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
            list.Add("Summons Green Goblin Alchemists");
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
                m_Timer = new SummonGreenGoblinTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonGreenGoblinTimer : Timer
        {
            private Mobile m_Owner;

            public SummonGreenGoblinTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Gloves) is AlchemistsExperimentalBracers))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before summoning
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    GreenGoblinAlchemist goblin = new GreenGoblinAlchemist
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    goblin.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Green Goblin Alchemist joins your cause!");
                }
            }
        }
    }
}
