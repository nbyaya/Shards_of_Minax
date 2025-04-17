using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class FrostcallersShard : Longsword
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public FrostcallersShard()
        {
            Weight = 5.0;
            Name = "Frostcaller's Shard";
            Hue = 1152; // Icy blue hue
            ItemID = 3718; // Ice Staff graphic

            // Set attributes and bonuses
            Attributes.BonusInt = 20;
            Attributes.BonusMana = 25;
            Attributes.RegenMana = 5;
            Attributes.SpellDamage = 15;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 2;
            Attributes.Luck = 150;

            // Skill bonuses
            SkillBonuses.SetValues(0, SkillName.Mysticism, 20.0);
            SkillBonuses.SetValues(1, SkillName.EvalInt, 15.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public FrostcallersShard(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "The shard empowers you to summon Frost Dragons!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonFrostDragonTimer(pm);
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
                pm.SendMessage(37, "You feel less attuned to frost magic.");
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
            list.Add("Summons Frost Dragons");
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
                    m_Timer = new SummonFrostDragonTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonFrostDragonTimer : Timer
        {
            private Mobile m_Owner;

            public SummonFrostDragonTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0)) // Adjust interval as needed
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.OneHanded) is FrostcallersShard))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    FrostDragon dragon = new FrostDragon
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    dragon.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Frost Dragon emerges from the icy void to serve you!");
                }
            }
        }
    }
}
