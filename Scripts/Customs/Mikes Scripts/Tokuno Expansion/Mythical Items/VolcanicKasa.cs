using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class VolcanicKasa : Kasa
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public VolcanicKasa()
        {
            Weight = 2.0;
            Name = "Volcanic Kasa";
            Hue = 1359; // Fiery orange hue

            // Set attributes and bonuses
            Attributes.BonusStr = 15;
            Attributes.BonusHits = 25;
            Attributes.RegenHits = 5;
            Attributes.RegenMana = 5;
            Attributes.DefendChance = 15;
            Attributes.ReflectPhysical = 10;
            Resistances.Fire = 20;
            Resistances.Physical = 10;
            Resistances.Energy = 5;

            SkillBonuses.SetValues(0, SkillName.Tactics, 15.0);
            SkillBonuses.SetValues(1, SkillName.AnimalTaming, 15.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public VolcanicKasa(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel empowered to control more fiery creatures!");

                // Start summon timer only if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    StopSummonTimer();
                    m_Timer = new SummonFireBeetleTimer(pm);
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
                pm.SendMessage(37, "Your connection to the volcanic plane weakens.");
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
            list.Add("Summons Fire Beetles");
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
                // Only start the timer if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonFireBeetleTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonFireBeetleTimer : Timer
        {
            private Mobile m_Owner;

            public SummonFireBeetleTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(10.0), TimeSpan.FromSeconds(10.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                // Stop if the owner is invalid or the item is not equipped
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Helm) is VolcanicKasa))
                {
                    Stop();
                    return;
                }

                // Only summon if autosummon is enabled
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                // Only summon if the player has room for more followers
                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    FireBeetle beetle = new FireBeetle
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    beetle.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Fire Beetle emerges to serve you!");
                }
            }
        }
    }
}
