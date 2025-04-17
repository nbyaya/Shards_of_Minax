using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class HellfireCrown : PlateHelm
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public HellfireCrown()
        {
            Name = "Hellfire Crown";
            Hue = 1175; // Fiery hue
            Weight = 5.0;

            // Set attributes and bonuses
            Attributes.BonusStr = 20;
            Attributes.BonusInt = 15;
            Attributes.RegenHits = 5;
            Attributes.RegenMana = 5;
            Attributes.SpellDamage = 10;
            Attributes.DefendChance = 10;
            Attributes.Luck = 150;

            PhysicalBonus = 12;
            FireBonus = 15;
            ColdBonus = 8;
            PoisonBonus = 10;
            EnergyBonus = 10;

            ArmorAttributes.SelfRepair = 5;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public HellfireCrown(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "The power of the DemonKnight surges within you, commanding more servants!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonDemonKnightTimer(pm);
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
                pm.SendMessage(37, "The power of the DemonKnight fades, and your command weakens.");
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
            list.Add("Summons DemonKnights to your side");
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
                m_Timer = new SummonDemonKnightTimer(mob);
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer.Start();
                }
            }
        }

        private class SummonDemonKnightTimer : Timer
        {
            private Mobile m_Owner;

            public SummonDemonKnightTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Helm) is HellfireCrown))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    DemonKnight demonKnight = new DemonKnight
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    demonKnight.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A DemonKnight emerges from the flames to serve you!");
                }
            }
        }
    }
}
