using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class StaffOfTheArcaneMeer : BlackStaff
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public StaffOfTheArcaneMeer()
        {
            Weight = 5.0;
            Name = "Staff of the Arcane Meer";
            Hue = 1174; // Ethereal blue hue

            // Set attributes and bonuses
            Attributes.SpellDamage = 20;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 2;
            Attributes.BonusInt = 15;
            Attributes.RegenMana = 5;
            Attributes.DefendChance = 10;

            SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
            SkillBonuses.SetValues(1, SkillName.EvalInt, 20.0);

            // Follower bonus
            m_BonusFollowers = 2;

            // Attach XmlLevelItem for scaling with player power
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public StaffOfTheArcaneMeer(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel an ancient arcane presence empowering your control over followers!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm)) // Check if autosummon is enabled
                {
                    m_Timer = new SummonMeerMageTimer(pm);
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
                pm.SendMessage(37, "The arcane presence fades, reducing your control over followers.");
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
            list.Add("Summons Meer Mages");
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
                if (AutoSummonManager.IsAutoSummonEnabled(mob)) // Check if autosummon is enabled
                {
                    m_Timer = new SummonMeerMageTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonMeerMageTimer : Timer
        {
            private Mobile m_Owner;

            public SummonMeerMageTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.TwoHanded) is StaffOfTheArcaneMeer))
                {
                    Stop();
                    return;
                }

                // Only summon if the player has room for more followers
                if (m_Owner.Followers < m_Owner.FollowersMax && AutoSummonManager.IsAutoSummonEnabled(m_Owner)) // Check if autosummon is enabled
                {
                    MeerMage mage = new MeerMage
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    mage.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Meer Mage answers your arcane call!");
                }
            }
        }
    }
}
