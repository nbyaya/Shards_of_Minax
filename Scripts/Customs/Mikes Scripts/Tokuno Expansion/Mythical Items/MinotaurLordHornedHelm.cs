using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class MinotaurLordHornedHelm : NorseHelm
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public MinotaurLordHornedHelm()
        {
            Weight = 5.0;
            Name = "Minotaur Lord's Horned Helm";
            Hue = 1109; // Dark brown hue, fitting for Minotaurs

            // Set attributes and bonuses
            Attributes.BonusStr = 25;
            Attributes.BonusHits = 15;
            Attributes.RegenHits = 3;
            Attributes.AttackChance = 10;
            Attributes.DefendChance = 10;

            PhysicalBonus = 15;
            FireBonus = 5;
            ColdBonus = 5;
            PoisonBonus = 10;
            EnergyBonus = 5;

            SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
            SkillBonuses.SetValues(1, SkillName.Tactics, 15.0);

            ArmorAttributes.DurabilityBonus = 50;
            ArmorAttributes.SelfRepair = 5;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public MinotaurLordHornedHelm(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel the commanding power of the Minotaur!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();

                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonMinotaurTimer(pm);
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
                pm.SendMessage(37, "The power of the Minotaur fades.");
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
            list.Add("Summons Minotaurs");
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
                // Check if autosummon is enabled on restart
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonMinotaurTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonMinotaurTimer : Timer
        {
            private Mobile m_Owner;

            public SummonMinotaurTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Helm) is MinotaurLordHornedHelm))
                {
                    Stop();
                    return;
                }

                // Ensure autosummon is enabled
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Minotaur minotaur = new Minotaur
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    minotaur.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Minotaur emerges to serve you!");
                }
            }
        }
    }
}
