using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class SavageMastersRobe : Robe
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public SavageMastersRobe()
        {
            Weight = 2.0;
            Name = "Savage Master's Robe";
            Hue = 1175; // Light green color to symbolize the wilderness

            // Set attributes and bonuses
            Attributes.BonusStr = 8;
            Attributes.BonusDex = 8;
            Attributes.BonusInt = 8;
            Attributes.BonusHits = 15;
            Attributes.BonusStam = 15;
            Attributes.BonusMana = 15;
            Attributes.RegenHits = 3;
            Attributes.RegenStam = 3;
            Attributes.RegenMana = 3;
            Attributes.WeaponSpeed = 5;
            Attributes.WeaponDamage = 10;
            Attributes.DefendChance = 10;
            Attributes.AttackChance = 10;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 1;
            Attributes.LowerManaCost = 5;
            Attributes.LowerRegCost = 10;
            Attributes.Luck = 75;
            Attributes.SpellDamage = 10;
            Attributes.NightSight = 1;

            // Resistances
            Resistances.Physical = 8;
            Resistances.Fire = 8;
            Resistances.Cold = 8;
            Resistances.Poison = 8;
            Resistances.Energy = 8;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 15.0);
            SkillBonuses.SetValues(1, SkillName.AnimalLore, 15.0);
            SkillBonuses.SetValues(2, SkillName.Veterinary, 15.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public SavageMastersRobe(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel a deeper connection to wild creatures!");

                // Start summon timer only if autosummon is enabled
                StopSummonTimer();

                if (AutoSummonManager.IsAutoSummonEnabled(pm)) // Check autosummon toggle
                {
                    m_Timer = new SummonSavageRidgebackTimer(pm);
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
                pm.SendMessage(37, "You feel your bond with wild creatures fade.");
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
            list.Add("Summons Savage Ridgebacks");
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

            // Reinitialize timer if equipped on restart and autosummon is enabled
            if (Parent is Mobile mob && AutoSummonManager.IsAutoSummonEnabled(mob))
            {
                m_Timer = new SummonSavageRidgebackTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonSavageRidgebackTimer : Timer
        {
            private Mobile m_Owner;

            public SummonSavageRidgebackTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(10.0), TimeSpan.FromSeconds(10.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.OuterTorso) is SavageMastersRobe))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before proceeding
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                // Only summon if the player has room for more followers
                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    SavageRidgeback ridgeback = new SavageRidgeback
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    ridgeback.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Savage Ridgeback emerges to serve you!");
                }
            }
        }
    }
}
