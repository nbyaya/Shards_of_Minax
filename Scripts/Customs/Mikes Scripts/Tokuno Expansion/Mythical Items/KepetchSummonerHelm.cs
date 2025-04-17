using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class KepetchSummonerHelm : PlateHelm
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public KepetchSummonerHelm()
        {
            Weight = 5.0;
            Name = "Helm of the Kepetch Summoner";
            Hue = 1153; // A shimmering greenish hue

            // Set attributes and bonuses
            ArmorAttributes.SelfRepair = 3;
            Attributes.BonusInt = 15;
            Attributes.RegenMana = 5;
            Attributes.DefendChance = 10;
            Attributes.SpellDamage = 10;

            PhysicalBonus = 12;
            FireBonus = 8;
            ColdBonus = 10;
            PoisonBonus = 15;
            EnergyBonus = 8;

            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 20.0);
            SkillBonuses.SetValues(1, SkillName.Mysticism, 15.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public KepetchSummonerHelm(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel an arcane bond with the creatures of the earth!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonKepetchTimer(pm);
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
                pm.SendMessage(37, "The arcane bond fades as you remove the helm.");
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
            list.Add("Summons Kepetch guardians");
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
                m_Timer = new SummonKepetchTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonKepetchTimer : Timer
        {
            private Mobile m_Owner;

            public SummonKepetchTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                // Stop if the owner is invalid or the item is not equipped
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Helm) is KepetchSummonerHelm))
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
                    Kepetch kepetch = new Kepetch
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    kepetch.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Kepetch emerges from the earth to serve you!");
                }
            }
        }
    }
}
