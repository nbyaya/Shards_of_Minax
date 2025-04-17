using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class FrostbindAmulet : GoldNecklace
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public FrostbindAmulet()
        {
            Weight = 1.0;
            Name = "Frostbind Amulet";
            Hue = 1152; // Icy blue color

            // Set attributes and bonuses
            Attributes.BonusMana = 20;
            Attributes.RegenMana = 5;
            Attributes.CastSpeed = 1;
            Attributes.SpellDamage = 15;
            Attributes.DefendChance = 10;

            Resistances.Physical = 8;
            Resistances.Fire = 5;
            Resistances.Cold = 20;
            Resistances.Poison = 5;
            Resistances.Energy = 10;

            SkillBonuses.SetValues(0, SkillName.Magery, 15.0);
            SkillBonuses.SetValues(1, SkillName.EvalInt, 15.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public FrostbindAmulet(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel the icy power of the ColdDrake enhancing your command over creatures!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonColdDrakeTimer(pm);
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
                pm.SendMessage(37, "The power of the ColdDrake fades, reducing your control over creatures.");
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
            list.Add("Summons ColdDrakes");
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
                if (AutoSummonManager.IsAutoSummonEnabled(mob))  // Check if autosummon is enabled when reloaded
                {
                    m_Timer = new SummonColdDrakeTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonColdDrakeTimer : Timer
        {
            private Mobile m_Owner;

            public SummonColdDrakeTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Neck) is FrostbindAmulet))
                {
                    Stop();
                    return;
                }

                // Only summon if autosummon is enabled
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    ColdDrake drake = new ColdDrake
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    drake.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "An icy ColdDrake emerges to serve you!");
                }
            }
        }
    }
}
