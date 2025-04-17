using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class SkreeSummonersHat : FeatheredHat
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public SkreeSummonersHat()
        {
            Weight = 1.0;
            Name = "Skree Summoner's Hat";
            Hue = 1150; // Ethereal blue hue for a mystical feel

            // Set attributes and bonuses
            Attributes.BonusInt = 10;
            Attributes.BonusMana = 15;
            Attributes.RegenMana = 3;
            Attributes.DefendChance = 10;
            Attributes.SpellDamage = 8;
            Attributes.LowerManaCost = 5;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Mysticism, 10.0);
            SkillBonuses.SetValues(1, SkillName.AnimalTaming, 10.0);
            SkillBonuses.SetValues(2, SkillName.AnimalLore, 10.0);

            // Resistances
            Resistances.Physical = 5;
            Resistances.Energy = 8;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public SkreeSummonersHat(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "The Skree recognize you as their master!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm)) // Check if autosummon is enabled
                {
                    m_Timer = new SummonSkreeTimer(pm);
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
                pm.SendMessage(37, "You feel less in tune with the Skree.");

                // Stop the summon timer
                StopSummonTimer();
            }
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
            list.Add("Summons Skree");
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
                if (AutoSummonManager.IsAutoSummonEnabled(mob)) // Respect the toggle when re-equip
                {
                    m_Timer = new SummonSkreeTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonSkreeTimer : Timer
        {
            private Mobile m_Owner;

            public SummonSkreeTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(10.0), TimeSpan.FromSeconds(10.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Helm) is SkreeSummonersHat))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                // Summon if the player has room for more followers
                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Skree skree = new Skree
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    skree.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Skree emerges from the shadows to serve you!");
                }
            }
        }
    }
}
