using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class ChaosDragoonArmor : PlateChest
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public ChaosDragoonArmor()
        {
            Weight = 10.0;
            Name = "Chaos Dragoon Armor";
            Hue = 1175; // A dark metallic hue representing chaos

            // Set attributes and bonuses
            ArmorAttributes.SelfRepair = 5;
            Attributes.BonusStr = 20;
            Attributes.BonusHits = 15;
            Attributes.RegenHits = 3;
            Attributes.DefendChance = 10;
            Attributes.AttackChance = 10;
            Attributes.WeaponDamage = 15;
            SkillBonuses.SetValues(0, SkillName.Tactics, 20.0);
            SkillBonuses.SetValues(1, SkillName.AnimalTaming, 15.0);

            PhysicalBonus = 15;
            FireBonus = 10;
            ColdBonus = 10;
            PoisonBonus = 15;
            EnergyBonus = 10;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public ChaosDragoonArmor(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel the chaotic power to command more creatures!");

                // Start summon timer
                StopSummonTimer();
                m_Timer = new SummonChaosDragoonTimer(pm);
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
                pm.SendMessage(37, "The chaotic power fades, and you feel less commanding.");
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
            list.Add("Summons Chaos Dragoon Elite");
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
                m_Timer = new SummonChaosDragoonTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonChaosDragoonTimer : Timer
        {
            private Mobile m_Owner;

            public SummonChaosDragoonTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.InnerTorso) is ChaosDragoonArmor))
                {
                    Stop();
                    return;
                }

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    ChaosDragoonElite dragoon = new ChaosDragoonElite
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    dragoon.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Chaos Dragoon Elite emerges to serve you!");
                }
            }
        }
    }
}
