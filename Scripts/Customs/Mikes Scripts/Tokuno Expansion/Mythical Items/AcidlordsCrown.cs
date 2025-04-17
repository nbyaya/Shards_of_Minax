using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class AcidlordsCrown : PlateHelm
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public AcidlordsCrown()
        {
            Weight = 5.0;
            Name = "Acidlord's Crown";
            Hue = 1167; // Acidic green color

            // Set attributes and bonuses
            Attributes.BonusInt = 15;
            Attributes.RegenMana = 8;
            Attributes.DefendChance = 10;
            Attributes.SpellDamage = 12;
            Attributes.CastRecovery = 2;
            Attributes.LowerManaCost = 10;

            PoisonBonus = 20;
            EnergyBonus = 10;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public AcidlordsCrown(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel empowered to command more creatures!");

                // Start summon timer
                StopSummonTimer();
                m_Timer = new SummonAcidElementalTimer(pm);
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
                pm.SendMessage(37, "The acidic power fades, reducing your command over creatures.");
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
            list.Add("Summons Acid Elementals");
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
                m_Timer = new SummonAcidElementalTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonAcidElementalTimer : Timer
        {
            private Mobile m_Owner;

            public SummonAcidElementalTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Helm) is AcidlordsCrown))
                {
                    Stop();
                    return;
                }

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    AcidElemental elemental = new AcidElemental
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    elemental.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "An Acid Elemental emerges from the corrosive void to serve you!");
                }
            }
        }
    }
}
