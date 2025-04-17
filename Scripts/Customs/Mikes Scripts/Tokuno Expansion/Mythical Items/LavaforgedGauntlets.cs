using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class LavaforgedGauntlets : PlateGloves
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public LavaforgedGauntlets()
        {
            Name = "Lavaforged Gauntlets";
            Hue = 1359; // Red-Orange hue to represent molten lava
            Weight = 5.0;

            // Attributes
            Attributes.BonusStr = 10;
            Attributes.BonusDex = 5;
            Attributes.RegenHits = 3;
            Attributes.RegenStam = 2;
            Attributes.WeaponDamage = 15;
            Attributes.DefendChance = 10;

            // Resistances
            PhysicalBonus = 8;
            FireBonus = 15;
            ColdBonus = 5;
            PoisonBonus = 5;
            EnergyBonus = 5;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Tactics, 15.0);
            SkillBonuses.SetValues(1, SkillName.Swords, 10.0);

            // Follower bonus
            m_BonusFollowers = 2;

            // Attach XmlLevelItem for leveling
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public LavaforgedGauntlets(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel empowered to control fiery beasts!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonLavaLizardTimer(pm);
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
                pm.SendMessage(37, "The fiery power leaves you, reducing your command over creatures.");
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
            list.Add("Summons Lava Lizards");
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
                m_Timer = new SummonLavaLizardTimer(mob);
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer.Start();
                }
            }
        }

        private class SummonLavaLizardTimer : Timer
        {
            private Mobile m_Owner;

            public SummonLavaLizardTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                // Stop if the owner is invalid or the item is not equipped
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Gloves) is LavaforgedGauntlets))
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
                    LavaLizard lizard = new LavaLizard
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    lizard.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Lava Lizard emerges from the ground to serve you!");
                }
            }
        }
    }
}
