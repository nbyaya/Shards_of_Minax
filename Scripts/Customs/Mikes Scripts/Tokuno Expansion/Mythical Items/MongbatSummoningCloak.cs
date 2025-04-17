using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class MongbatSummoningCloak : Cloak
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public MongbatSummoningCloak()
        {
            Weight = 1.0;
            Name = "Cloak of Mischief";
            Hue = 1153; // Dark mischievous color, adjust as desired

            // Set attributes and bonuses
            Attributes.BonusDex = 20;
            Attributes.BonusMana = 15;
            Attributes.RegenMana = 3;
            Attributes.DefendChance = 10;
            Attributes.Luck = 150;
            Attributes.NightSight = 1;

            SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
            SkillBonuses.SetValues(1, SkillName.AnimalTaming, 15.0);

            Resistances.Physical = 8;
            Resistances.Fire = 6;
            Resistances.Cold = 6;
            Resistances.Poison = 12;
            Resistances.Energy = 8;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public MongbatSummoningCloak(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel a mischievous energy surround you, enhancing your control over creatures!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonMongbatTimer(pm);
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
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
                pm.SendMessage(37, "The mischievous energy fades, and you feel less control over creatures.");
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
            list.Add("Summons Greater Mongbats");
            list.Add("Increases maximum followers by 2");
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
                m_Timer = new SummonMongbatTimer(mob);
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer.Start();
                }
            }
        }

        private class SummonMongbatTimer : Timer
        {
            private Mobile m_Owner;

            public SummonMongbatTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                // Stop if the owner is invalid or the item is not equipped
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Cloak) is MongbatSummoningCloak))
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
                    GreaterMongbat mongbat = new GreaterMongbat
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    mongbat.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Greater Mongbat flutters to your aid, chittering mischievously!");
                }
            }
        }
    }
}
