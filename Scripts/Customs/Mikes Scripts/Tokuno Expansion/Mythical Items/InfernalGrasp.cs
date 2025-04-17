using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;
using Server.Engines.Quests.Samurai;

namespace Server.Items
{
    public class InfernalGrasp : LeatherGloves
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public InfernalGrasp()
        {
            Weight = 1.0;
            Name = "Infernal Grasp";
            Hue = 1157; // Dark fiery color

            // Set attributes and bonuses
            Attributes.BonusStr = 10;
            Attributes.BonusDex = 5;
            Attributes.BonusMana = 15;
            Attributes.RegenMana = 3;
            Attributes.SpellDamage = 10;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 2;
            
            PhysicalBonus = 5;
            FireBonus = 15;
            ColdBonus = 5;
            PoisonBonus = 10;
            EnergyBonus = 10;

            SkillBonuses.SetValues(0, SkillName.Necromancy, 20.0);
            SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 20.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public InfernalGrasp(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel empowered to control more infernal minions!");

                // Start summon timer only if autosummon is enabled
                StopSummonTimer();

                // Check if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonDeadlyImpTimer(pm);
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
                pm.SendMessage(37, "You feel your infernal control weakening.");
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
            list.Add("Summons Deadly Imps");
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
                // Check if autosummon is enabled upon restart
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonDeadlyImpTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonDeadlyImpTimer : Timer
        {
            private Mobile m_Owner;

            public SummonDeadlyImpTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Gloves) is InfernalGrasp))
                {
                    Stop();
                    return;
                }

                // Only summon if the player has room for more followers and autosummon is enabled
                if (m_Owner.Followers < m_Owner.FollowersMax && AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                {
                    DeadlyImp imp = new DeadlyImp
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    imp.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Deadly Imp emerges to serve you!");
                }
            }
        }
    }
}
