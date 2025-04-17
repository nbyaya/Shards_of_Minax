using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class WardensGargoyleAmulet : GoldNecklace
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public WardensGargoyleAmulet()
        {
            Weight = 1.0;
            Name = "Warden's Gargoyle Amulet";
            Hue = 1154; // Gargoyle-esque metallic hue

            // Set attributes and bonuses
            Attributes.BonusStr = 15;
            Attributes.BonusDex = 10;
            Attributes.BonusMana = 20;
            Attributes.DefendChance = 10;
            Attributes.ReflectPhysical = 15;
            Attributes.RegenHits = 3;
            Attributes.RegenMana = 5;

            Resistances.Physical = 12;
            Resistances.Fire = 8;
            Resistances.Cold = 10;
            Resistances.Poison = 10;
            Resistances.Energy = 10;

            SkillBonuses.SetValues(0, SkillName.Anatomy, 20.0);
            SkillBonuses.SetValues(1, SkillName.Parry, 15.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public WardensGargoyleAmulet(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "The Warden's power bolsters your command over followers!");

                // Start summon timer only if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    StopSummonTimer();
                    m_Timer = new SummonGargoyleEnforcerTimer(pm);
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
                pm.SendMessage(37, "The Warden's power leaves you, reducing your control over followers.");
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
            list.Add("Summons Gargoyle Enforcers");
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
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonGargoyleEnforcerTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonGargoyleEnforcerTimer : Timer
        {
            private Mobile m_Owner;

            public SummonGargoyleEnforcerTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(12.0), TimeSpan.FromSeconds(12.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Neck) is WardensGargoyleAmulet))
                {
                    Stop();
                    return;
                }

                // Only summon if autosummon is enabled
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    GargoyleEnforcer enforcer = new GargoyleEnforcer
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    enforcer.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Gargoyle Enforcer materializes, ready to serve you!");
                }
            }
        }
    }
}
