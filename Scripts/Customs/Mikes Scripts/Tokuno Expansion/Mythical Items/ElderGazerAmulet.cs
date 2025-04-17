using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class ElderGazerAmulet : GoldNecklace
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public ElderGazerAmulet()
        {
            Weight = 1.0;
            Name = "Amulet of the Elder Gazer";
            Hue = 1153; // A mystical purple hue

            // Set attributes and bonuses
            Attributes.BonusInt = 25;
            Attributes.RegenMana = 10;
            Attributes.SpellDamage = 20;
            Attributes.CastSpeed = 2;
            Attributes.CastRecovery = 3;
            Attributes.LowerManaCost = 10;

            Resistances.Physical = 12;
            Resistances.Fire = 10;
            Resistances.Cold = 8;
            Resistances.Poison = 10;
            Resistances.Energy = 15;

            SkillBonuses.SetValues(0, SkillName.EvalInt, 20.0);
            SkillBonuses.SetValues(1, SkillName.Magery, 20.0);
            SkillBonuses.SetValues(2, SkillName.Meditation, 15.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public ElderGazerAmulet(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "The power of the Elder Gazer flows through you, increasing your command over followers!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonElderGazerTimer(pm);
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
                pm.SendMessage(37, "The connection to the Elder Gazer fades, reducing your command over followers.");
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
            list.Add("Summons Elder Gazers");
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
            if (Parent is Mobile mob && AutoSummonManager.IsAutoSummonEnabled(mob))
            {
                m_Timer = new SummonElderGazerTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonElderGazerTimer : Timer
        {
            private Mobile m_Owner;

            public SummonElderGazerTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Neck) is ElderGazerAmulet))
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
                    ElderGazer elderGazer = new ElderGazer
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    elderGazer.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "An Elder Gazer manifests to serve your will!");
                }
            }
        }
    }
}
