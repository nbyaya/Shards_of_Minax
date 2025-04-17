using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class HeartOfTheSlith : PlateChest
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public HeartOfTheSlith()
        {
            Weight = 10.0;
            Name = "Heart of the Slith";
            Hue = 1367; // Earthy stone-like hue

            // Set attributes and bonuses
            Attributes.BonusHits = 15;
            Attributes.BonusStam = 10;
            Attributes.RegenHits = 3;
            Attributes.RegenStam = 2;
            Attributes.DefendChance = 10;
            Attributes.AttackChance = 10;
            Attributes.Luck = 150;

            // Resistances
            PhysicalBonus = 12;
            FireBonus = 8;
            ColdBonus = 10;
            PoisonBonus = 15;
            EnergyBonus = 10;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 10.0);
            SkillBonuses.SetValues(1, SkillName.AnimalLore, 10.0);

            // Follower bonus
            m_BonusFollowers = 2;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public HeartOfTheSlith(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel a primal connection with stone-born creatures!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonStoneSlithTimer(pm);
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
                pm.SendMessage(37, "You feel your connection with stone-born creatures weaken.");
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
            list.Add("Summons Stone Sliths");
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
                m_Timer = new SummonStoneSlithTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonStoneSlithTimer : Timer
        {
            private Mobile m_Owner;

            public SummonStoneSlithTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0)) // Summons every 15 seconds
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                // Stop if the owner is invalid or the item is not equipped
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.InnerTorso) is HeartOfTheSlith))
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
                    StoneSlith slith = new StoneSlith
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    slith.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Stone Slith emerges from the ground to aid you!");
                }
            }
        }
    }
}
