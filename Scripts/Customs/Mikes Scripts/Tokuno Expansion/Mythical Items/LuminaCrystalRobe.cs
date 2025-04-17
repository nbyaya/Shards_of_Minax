using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class LuminaCrystalRobe : Robe
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public LuminaCrystalRobe()
        {
            Weight = 2.0;
            Name = "Lumina Crystal Robe";
            Hue = 1153; // A crystalline blue hue

            // Set attributes and bonuses
            ClothingAttributes.SelfRepair = 5;
            Attributes.BonusMana = 20;
            Attributes.RegenMana = 5;
            Attributes.BonusInt = 15;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 2;
            Attributes.Luck = 200;
            Attributes.NightSight = 1;

            Resistances.Physical = 8;
            Resistances.Fire = 8;
            Resistances.Cold = 12;
            Resistances.Poison = 10;
            Resistances.Energy = 15;

            SkillBonuses.SetValues(0, SkillName.SpiritSpeak, 15.0);
            SkillBonuses.SetValues(1, SkillName.EvalInt, 15.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 1;
        }

        public LuminaCrystalRobe(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel a harmonious crystalline energy guiding your control over creatures!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonCrystalWispTimer(pm);
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
                pm.SendMessage(37, "The harmonious crystalline energy fades from you.");
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
            list.Add("Summons Crystal Wisps");
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
                // Start summon timer if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonCrystalWispTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonCrystalWispTimer : Timer
        {
            private Mobile m_Owner;

            public SummonCrystalWispTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(10.0), TimeSpan.FromSeconds(10.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.OuterTorso) is LuminaCrystalRobe))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before proceeding
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    CrystalWisp wisp = new CrystalWisp
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    wisp.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Crystal Wisp emerges to serve you, radiating tranquil energy!");
                }
            }
        }
    }
}
