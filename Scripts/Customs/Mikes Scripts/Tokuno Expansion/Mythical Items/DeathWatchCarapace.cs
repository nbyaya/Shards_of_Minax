using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class DeathWatchCarapace : BoneChest
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public DeathWatchCarapace()
        {
            Weight = 6.0;
            Name = "DeathWatch Carapace";
            Hue = 1175; // Dark Beetle-like hue

            // Set attributes and bonuses
            ArmorAttributes.SelfRepair = 3;
            Attributes.BonusStr = 20;
            Attributes.RegenHits = 3;
            Attributes.DefendChance = 15;
            Attributes.ReflectPhysical = 10;
            Attributes.SpellDamage = 5;

            PhysicalBonus = 15;
            FireBonus = 10;
            ColdBonus = 10;
            PoisonBonus = 20;
            EnergyBonus = 10;

            SkillBonuses.SetValues(0, SkillName.AnimalTaming, 20.0);
            SkillBonuses.SetValues(1, SkillName.AnimalLore, 20.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public DeathWatchCarapace(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel like you can command more creatures now!");

                // Start summon timer only if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonDeathwatchBeetleTimer(pm);
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
                pm.SendMessage(37, "You feel like you cannot command as many creatures as before.");
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
            list.Add("Summons DeathWatch Beetles");
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
                    m_Timer = new SummonDeathwatchBeetleTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonDeathwatchBeetleTimer : Timer
        {
            private Mobile m_Owner;

            public SummonDeathwatchBeetleTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                // Stop if the owner is invalid or the item is not equipped
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.InnerTorso) is DeathWatchCarapace))
                {
                    Stop();
                    return;
                }

                // Only summon if autosummon is enabled and the player has room for more followers
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    DeathwatchBeetle beetle = new DeathwatchBeetle
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    beetle.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A DeathWatch Beetle emerges to serve you!");
                }
            }
        }
    }
}
