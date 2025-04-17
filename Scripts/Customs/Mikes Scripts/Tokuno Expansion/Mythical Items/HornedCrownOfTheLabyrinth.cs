using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class HornedCrownOfTheLabyrinth : PlateHelm
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public HornedCrownOfTheLabyrinth()
        {
            Weight = 5.0;
            Name = "Horned Crown of the Labyrinth";
            Hue = 1157; // Dark gold hue for a majestic appearance

            // Set attributes and bonuses
            ArmorAttributes.SelfRepair = 5;
            Attributes.BonusStr = 20;
            Attributes.BonusHits = 20;
            Attributes.RegenHits = 3;
            Attributes.DefendChance = 10;
            Attributes.AttackChance = 10;
            SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
            SkillBonuses.SetValues(1, SkillName.Swords, 15.0);

            PhysicalBonus = 15;
            FireBonus = 10;
            ColdBonus = 10;
            PoisonBonus = 5;
            EnergyBonus = 10;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public HornedCrownOfTheLabyrinth(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel a surge of command and authority over mighty beasts!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonMinotaurCaptainTimer(pm);
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
                pm.SendMessage(37, "You feel your command over beasts wane.");
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
            list.Add("Summons Minotaur Captains");
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
                m_Timer = new SummonMinotaurCaptainTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonMinotaurCaptainTimer : Timer
        {
            private Mobile m_Owner;

            public SummonMinotaurCaptainTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Helm) is HornedCrownOfTheLabyrinth))
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
                    MinotaurCaptain minotaur = new MinotaurCaptain
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    minotaur.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Minotaur Captain emerges to serve your command!");
                }
            }
        }
    }
}
