using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class BrigandLordCloak : Cloak
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public BrigandLordCloak()
        {
            Weight = 5.0;
            Name = "Brigand Lord's Cloak";
            Hue = 2124; // A dark green/brown color, fitting a brigand theme

            // Set attributes and bonuses
            Attributes.BonusDex = 15;
            Attributes.BonusHits = 25;
            Attributes.RegenHits = 5;
            Attributes.ReflectPhysical = 10;
            Attributes.Luck = 200;

            Resistances.Physical = 12;
            Resistances.Fire = 10;
            Resistances.Cold = 8;
            Resistances.Poison = 10;
            Resistances.Energy = 6;

            SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
            SkillBonuses.SetValues(1, SkillName.Fencing, 15.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public BrigandLordCloak(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel the presence of cunning brigands at your side!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonElfBrigandTimer(pm);
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
                pm.SendMessage(37, "The brigands have faded away.");
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
            list.Add("Summons Elf Brigands");
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
                m_Timer = new SummonElfBrigandTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonElfBrigandTimer : Timer
        {
            private Mobile m_Owner;

            public SummonElfBrigandTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Cloak) is BrigandLordCloak))
                {
                    Stop();
                    return;
                }

                // Only summon if autosummon is enabled
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                // Check if the player has room for more followers
                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    ElfBrigand brigand = new ElfBrigand
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    brigand.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "An Elf Brigand appears to serve you!");
                }
            }
        }
    }
}
