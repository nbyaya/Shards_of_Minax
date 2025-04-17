using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class AmuletOfTheCrystalDepths : GoldNecklace
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public AmuletOfTheCrystalDepths()
        {
            Weight = 1.0;
            Name = "Amulet of the Crystal Depths";
            Hue = 1154; // A light crystal blue hue

            // Set attributes and bonuses
            Attributes.BonusInt = 25;
            Attributes.BonusMana = 30;
            Attributes.RegenMana = 10;
            Attributes.SpellDamage = 15;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 2;

            Resistances.Physical = 5;
            Resistances.Fire = 5;
            Resistances.Cold = 15;
            Resistances.Poison = 5;
            Resistances.Energy = 10;

            SkillBonuses.SetValues(0, SkillName.EvalInt, 15.0);
            SkillBonuses.SetValues(1, SkillName.Magery, 15.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public AmuletOfTheCrystalDepths(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "The depths of the crystal seas empower you to command more creatures!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonCrystalSeaSerpentTimer(pm);
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
                pm.SendMessage(37, "The power of the crystal seas fades, limiting your command over creatures.");
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
            list.Add("Summons Crystal Sea Serpents");
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
                m_Timer = new SummonCrystalSeaSerpentTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonCrystalSeaSerpentTimer : Timer
        {
            private Mobile m_Owner;

            public SummonCrystalSeaSerpentTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(10.0), TimeSpan.FromSeconds(10.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                // Stop if the owner is invalid or the item is not equipped
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Neck) is AmuletOfTheCrystalDepths))
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
                    CrystalSeaSerpent serpent = new CrystalSeaSerpent
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    serpent.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Crystal Sea Serpent rises from the depths to aid you!");
                }
            }
        }
    }
}
