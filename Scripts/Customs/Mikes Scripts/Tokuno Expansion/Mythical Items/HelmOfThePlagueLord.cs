using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class HelmOfThePlagueLord : BoneHelm
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public HelmOfThePlagueLord()
        {
            Weight = 5.0;
            Name = "Helm of the Plague Lord";
            Hue = 1365; // Sickly green color

            // Set attributes and bonuses
            Attributes.BonusStr = 15;
            Attributes.RegenHits = 3;
            Attributes.RegenMana = 3;
            Attributes.DefendChance = 10;
            Attributes.LowerManaCost = 10;
            Attributes.ReflectPhysical = 15;

            SkillBonuses.SetValues(0, SkillName.Necromancy, 25.0);
            SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 25.0);

            PhysicalBonus = 15;
            FireBonus = 5;
            ColdBonus = 10;
            PoisonBonus = 25; // High poison resistance fits the theme
            EnergyBonus = 10;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public HelmOfThePlagueLord(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "A dark power stirs within you, granting the ability to command more creatures.");

                // Start summon timer
                StopSummonTimer();
                m_Timer = new SummonPlagueBeastLordTimer(pm);
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
                pm.SendMessage(37, "The dark power fades, and your control over creatures diminishes.");
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
            list.Add("Summons PlagueBeastLords");
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
                m_Timer = new SummonPlagueBeastLordTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonPlagueBeastLordTimer : Timer
        {
            private Mobile m_Owner;

            public SummonPlagueBeastLordTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Helm) is HelmOfThePlagueLord))
                {
                    Stop();
                    return;
                }

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    PlagueBeastLord plagueBeast = new PlagueBeastLord
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    plagueBeast.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Plague Beast Lord emerges from the shadows to serve you!");
                }
            }
        }
    }
}
