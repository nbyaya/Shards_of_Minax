using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class LabyrinthMastersHelm : PlateHelm
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public LabyrinthMastersHelm()
        {
            Weight = 5.0;
            Name = "Labyrinth Master's Helm";
            Hue = 2213; // A bronze/maze-like hue

            // Set attributes and bonuses
            ArmorAttributes.SelfRepair = 5;
            Attributes.BonusStr = 20;
            Attributes.BonusStam = 15;
            Attributes.DefendChance = 15;
            Attributes.ReflectPhysical = 10;
            Attributes.RegenStam = 5;

            // Resistance bonuses
            PhysicalBonus = 15;
            FireBonus = 10;
            ColdBonus = 10;
            PoisonBonus = 5;
            EnergyBonus = 10;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public LabyrinthMastersHelm(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel like you can command more creatures within the labyrinth!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonMinotaurGeneralTimer(pm);
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
                pm.SendMessage(37, "You feel less capable of commanding creatures.");
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
            list.Add("Summons Minotaur Generals");
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
                m_Timer = new SummonMinotaurGeneralTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonMinotaurGeneralTimer : Timer
        {
            private Mobile m_Owner;

            public SummonMinotaurGeneralTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0)) // Spawns every 15 seconds
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                // Stop if the owner is invalid or the item is not equipped
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Helm) is LabyrinthMastersHelm))
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
                    MinotaurGeneral minotaur = new MinotaurGeneral
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    minotaur.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Minotaur General emerges from the labyrinth to serve you!");
                }
            }
        }
    }
}
