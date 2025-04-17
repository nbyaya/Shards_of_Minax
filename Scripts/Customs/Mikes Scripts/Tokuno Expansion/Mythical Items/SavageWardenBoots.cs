using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class SavageWardenBoots : Boots
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public SavageWardenBoots()
        {
            Weight = 1.0;
            Name = "Savage Warden's Boots";
            Hue = 1157; // A wild green hue for a nature theme

            // Set attributes and bonuses
            Attributes.BonusDex = 20;
            Attributes.BonusStam = 15;
            Attributes.RegenStam = 3;
            Attributes.DefendChance = 10;
            Attributes.Luck = 150;

            Resistances.Physical = 8;
            Resistances.Fire = 5;
            Resistances.Cold = 12;
            Resistances.Poison = 10;
            Resistances.Energy = 5;

            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 20.0);
            SkillBonuses.SetValues(1, SkillName.AnimalLore, 15.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public SavageWardenBoots(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel a surge of wild energy empowering your command over creatures!");

                // Check if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    // Start summon timer if autosummon is enabled
                    StopSummonTimer();
                    m_Timer = new SummonFrenziedOstardTimer(pm);
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
                pm.SendMessage(37, "The wild energy fades, reducing your control over creatures.");
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
            list.Add("Summons Frenzied Ostards");
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
                // Start summon timer only if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonFrenziedOstardTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonFrenziedOstardTimer : Timer
        {
            private Mobile m_Owner;

            public SummonFrenziedOstardTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Shoes) is SavageWardenBoots))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    FrenziedOstard ostard = new FrenziedOstard
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    ostard.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Frenzied Ostard charges into the fray to serve you!");
                }
            }
        }
    }
}
