using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class DrakeCommandersBreastplate : PlateChest
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public DrakeCommandersBreastplate()
        {
            Weight = 10.0;
            Name = "Drake Commander's Breastplate";
            Hue = 1157; // A metallic golden hue to represent majesty and strength

            // Set attributes and bonuses
            Attributes.BonusStr = 25;
            Attributes.BonusHits = 15;
            Attributes.BonusMana = 10;
            Attributes.RegenHits = 3;
            Attributes.DefendChance = 10;
            Attributes.ReflectPhysical = 10;
            Attributes.LowerManaCost = 10;
            PhysicalBonus = 15;
            FireBonus = 20;
            ColdBonus = 10;
            PoisonBonus = 5;
            EnergyBonus = 10;

            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 20.0);
            SkillBonuses.SetValues(1, SkillName.AnimalLore, 15.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public DrakeCommandersBreastplate(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel the power to command mighty Drakes!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonDrakeTimer(pm);
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
                pm.SendMessage(37, "The command of Drakes fades from your grasp.");
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
            list.Add("Summons Drakes");
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
                m_Timer = new SummonDrakeTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonDrakeTimer : Timer
        {
            private Mobile m_Owner;

            public SummonDrakeTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                // Stop if the owner is invalid or the item is not equipped
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.InnerTorso) is DrakeCommandersBreastplate))
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
                    Drake drake = new Drake
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    drake.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A majestic Drake descends to serve you!");
                }
            }
        }
    }
}
