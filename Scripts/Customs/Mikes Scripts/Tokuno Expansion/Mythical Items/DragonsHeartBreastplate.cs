using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class DragonsHeartBreastplate : PlateChest
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public DragonsHeartBreastplate()
        {
            Name = "Dragon's Heart Breastplate";
            Hue = 1157; // Fiery red, can be adjusted for effect
            Weight = 8.0;

            // Set attributes and bonuses
            Attributes.BonusStr = 20;
            Attributes.BonusHits = 15;
            Attributes.RegenHits = 5;
            Attributes.RegenStam = 3;
            Attributes.RegenMana = 3;
            Attributes.DefendChance = 15;
            Attributes.SpellDamage = 10;

            PhysicalBonus = 15;
            FireBonus = 20;
            ColdBonus = 10;
            PoisonBonus = 5;
            EnergyBonus = 10;

            ArmorAttributes.SelfRepair = 5;
            ArmorAttributes.MageArmor = 1;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public DragonsHeartBreastplate(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel the commanding presence of a dragon's heart!");

                // Start summon timer only if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonGreaterDragonTimer(pm);
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
                pm.SendMessage(37, "The dragon's commanding presence fades from you.");
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
            list.Add("Summons Greater Dragons");
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
                    m_Timer = new SummonGreaterDragonTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonGreaterDragonTimer : Timer
        {
            private Mobile m_Owner;

            public SummonGreaterDragonTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(30.0), TimeSpan.FromSeconds(30.0)) // Summons every 30 seconds
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.InnerTorso) is DragonsHeartBreastplate))
                {
                    Stop();
                    return;
                }

                // Only summon if autosummon is enabled
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    GreaterDragon dragon = new GreaterDragon
                    {
                        Controlled = true,
                        ControlMaster = m_Owner,
                        Loyalty = 100
                    };

                    dragon.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Greater Dragon emerges to serve you!");
                }
            }
        }
    }
}
