using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class NecromancersMantle : Cloak
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public NecromancersMantle()
        {
            Weight = 1.0;
            Name = "Necromancer's Mantle";
            Hue = 1175; // A dark, necrotic shade

            // Set attributes and bonuses
            Attributes.BonusInt = 15;
            Attributes.RegenMana = 3;
            Attributes.SpellDamage = 10;
            Attributes.LowerManaCost = 10;
            Attributes.NightSight = 1;

            Resistances.Physical = 8;
            Resistances.Fire = 6;
            Resistances.Cold = 12;
            Resistances.Poison = 15;
            Resistances.Energy = 10;

            SkillBonuses.SetValues(0, SkillName.Necromancy, 20.0);
            SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 15.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public NecromancersMantle(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel the power to control more creatures coursing through you!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonFleshGolemTimer(pm);
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
                pm.SendMessage(37, "You feel the necrotic power wane as you lose control over creatures.");
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
            list.Add("Summons Flesh Golems");
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
                    m_Timer = new SummonFleshGolemTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonFleshGolemTimer : Timer
        {
            private Mobile m_Owner;

            public SummonFleshGolemTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(12.0), TimeSpan.FromSeconds(12.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Cloak) is NecromancersMantle))
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
                    FleshGolem golem = new FleshGolem
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    golem.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Flesh Golem rises from the ground to serve you!");
                }
            }
        }
    }
}
