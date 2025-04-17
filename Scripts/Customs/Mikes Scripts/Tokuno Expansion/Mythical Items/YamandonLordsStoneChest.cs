using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class YamandonLordsStoneChest : PlateChest
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public YamandonLordsStoneChest()
        {
            Weight = 10.0;
            Name = "Yamandon Lord's Stone Chest";
            Hue = 1161; // Dark, ominous purple

            // Set attributes and bonuses
            Attributes.BonusStr = 15;
            Attributes.BonusDex = 10;
            Attributes.BonusInt = 5;
            Attributes.RegenHits = 3;
            Attributes.RegenStam = 3;
            Attributes.RegenMana = 3;
            Attributes.WeaponDamage = 15;
            Attributes.DefendChance = 10;
            Attributes.AttackChance = 10;
            Attributes.Luck = 75;
            Attributes.SpellDamage = 10;
            Attributes.NightSight = 1;

            // Resistances
            PhysicalBonus = 15;
            FireBonus = 10;
            ColdBonus = 10;
            PoisonBonus = 15;
            EnergyBonus = 10;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 15.0);
            SkillBonuses.SetValues(1, SkillName.AnimalLore, 15.0);
            SkillBonuses.SetValues(2, SkillName.MagicResist, 10.0);

            // Follower bonus
            m_BonusFollowers = 2;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public YamandonLordsStoneChest(Serial serial) : base(serial)
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

                // Check if autosummon is enabled and start summon timer if true
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonYamandonTimer(pm);
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
            list.Add("Summons Yamandons");
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
                // Check if autosummon is enabled and start timer if true
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonYamandonTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonYamandonTimer : Timer
        {
            private Mobile m_Owner;

            public SummonYamandonTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.InnerTorso) is YamandonLordsStoneChest))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before summoning
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                // Only summon if the player has room for more followers
                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Yamandon yamandon = new Yamandon
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    yamandon.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Yamandon emerges to serve you!");
                }
            }
        }
    }
}
