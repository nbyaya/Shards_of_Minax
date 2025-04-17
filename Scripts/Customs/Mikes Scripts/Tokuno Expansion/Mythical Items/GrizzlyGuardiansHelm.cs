using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class GrizzlyGuardiansHelm : BearMask
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public GrizzlyGuardiansHelm()
        {
            Weight = 5.0;
            Name = "Grizzly Guardian's Helm";
            Hue = 2413; // Dark brown, like a bear's fur

            // Set attributes and bonuses
            Attributes.BonusStr = 20;
            Attributes.RegenHits = 5;
            Attributes.DefendChance = 15;
            Attributes.LowerManaCost = 10;
            Attributes.ReflectPhysical = 10;

            Resistances.Physical = 15;
            Resistances.Cold = 10;
            Resistances.Poison = 5;
            Resistances.Fire = 5;
            Resistances.Energy = 10;

            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 20.0);
            SkillBonuses.SetValues(1, SkillName.AnimalLore, 20.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public GrizzlyGuardiansHelm(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel the spirit of the Grizzly empowering you to command more creatures!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonGrizzlyBearTimer(pm);
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
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
                pm.SendMessage(37, "The spirit of the Grizzly fades, and you feel less capable of commanding creatures.");
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
            list.Add("Summons Grizzly Bears");
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
                m_Timer = new SummonGrizzlyBearTimer(mob);
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer.Start();
                }
            }
        }

        private class SummonGrizzlyBearTimer : Timer
        {
            private Mobile m_Owner;

            public SummonGrizzlyBearTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Helm) is GrizzlyGuardiansHelm))
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
                    GrizzlyBear grizzly = new GrizzlyBear
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    grizzly.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Grizzly Bear emerges to serve you!");
                }
            }
        }
    }
}
