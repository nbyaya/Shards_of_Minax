using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class DragonmastersRobe : Robe
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public DragonmastersRobe()
        {
            Weight = 2.0;
            Name = "Dragonmaster's Robe";
            Hue = 1175; // Greenish dragon-like hue

            // Set attributes and bonuses
            Attributes.BonusStr = 8;
            Attributes.BonusDex = 8;
            Attributes.BonusInt = 8;
            Attributes.BonusHits = 15;
            Attributes.BonusMana = 15;
            Attributes.RegenHits = 3;
            Attributes.RegenMana = 3;
            Attributes.WeaponDamage = 15;
            Attributes.DefendChance = 10;
            Attributes.AttackChance = 10;
            Attributes.LowerManaCost = 10;
            Attributes.Luck = 150;

            // Resistances
            Resistances.Physical = 8;
            Resistances.Fire = 8;
            Resistances.Cold = 8;
            Resistances.Poison = 8;
            Resistances.Energy = 8;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 10.0);
            SkillBonuses.SetValues(1, SkillName.AnimalLore, 10.0);
            SkillBonuses.SetValues(2, SkillName.Veterinary, 10.0);
            SkillBonuses.SetValues(3, SkillName.MagicResist, 10.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public DragonmastersRobe(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel like you could command more creatures now!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonScaledSwampDragonTimer(pm);
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
                pm.SendMessage(37, "You feel like you cannot command as many creatures as before.");
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
            list.Add("Summons Scaled Swamp Dragons");
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
                m_Timer = new SummonScaledSwampDragonTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonScaledSwampDragonTimer : Timer
        {
            private Mobile m_Owner;

            public SummonScaledSwampDragonTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(10.0), TimeSpan.FromSeconds(10.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.OuterTorso) is DragonmastersRobe))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    ScaledSwampDragon dragon = new ScaledSwampDragon
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    dragon.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Scaled Swamp Dragon emerges to serve you!");
                }
            }
        }
    }
}
