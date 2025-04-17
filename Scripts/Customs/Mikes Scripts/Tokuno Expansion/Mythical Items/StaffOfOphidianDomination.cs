using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class StaffOfOphidianDomination : GnarledStaff
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public StaffOfOphidianDomination()
        {
            Weight = 5.0;
            Name = "Staff of Ophidian Domination";
            Hue = 1266; // Ophidian-themed hue

            // Set attributes and bonuses
            Attributes.BonusInt = 15;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 2;
            Attributes.SpellDamage = 10;
            Attributes.LowerManaCost = 10;
            Attributes.LowerRegCost = 10;
            Attributes.NightSight = 1;
            Attributes.RegenMana = 3;

            SkillBonuses.SetValues(0, SkillName.Magery, 15.0);
            SkillBonuses.SetValues(1, SkillName.EvalInt, 10.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public StaffOfOphidianDomination(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "The staff empowers you to command more minions!");

                // Start summon timer if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    StopSummonTimer();
                    m_Timer = new SummonOphidianArchmageTimer(pm);
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
                pm.SendMessage(37, "The staff's influence fades, reducing your command over minions.");
            }

            // Stop the summon timer if it's running
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
            list.Add("Summons Ophidian Archmages");
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
                // Check if autosummon is enabled and restart the summon timer accordingly
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonOphidianArchmageTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonOphidianArchmageTimer : Timer
        {
            private Mobile m_Owner;

            public SummonOphidianArchmageTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                // Stop if the owner is invalid, the item is not equipped, or autosummon is disabled
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.OneHanded) is StaffOfOphidianDomination) || !AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                {
                    Stop();
                    return;
                }

                // Only summon if the player has room for more followers
                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    OphidianArchmage archmage = new OphidianArchmage
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    archmage.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "An Ophidian Archmage emerges to serve you!");
                }
            }
        }
    }
}
