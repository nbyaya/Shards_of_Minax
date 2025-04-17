using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class NecromancerKiteShield : BaseShield
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public NecromancerKiteShield() : base(0x1B74) // Kite Shield Item ID
        {
            Weight = 7.0;
            Name = "Necromancer's Kite Shield";
            Hue = 1175; // Dark necrotic hue

            // Set attributes and bonuses
            Attributes.BonusInt = 8;
            Attributes.BonusMana = 15;
            Attributes.RegenMana = 3;
            Attributes.SpellDamage = 10;
            Attributes.DefendChance = 10;
            Attributes.LowerManaCost = 8;
            Attributes.NightSight = 1;

            // Resistances
            PhysicalBonus = 8;
            ColdBonus = 12;
            PoisonBonus = 8;

            // Follower bonus
            m_BonusFollowers = 2;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public NecromancerKiteShield(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "Dark power fills you, allowing you to command more minions!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonSkeletalMageTimer(pm);
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
                pm.SendMessage(37, "The dark power fades, limiting your ability to command minions.");
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
            list.Add("Summons Skeletal Mages");
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
                // Ensure the summon timer is started only if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonSkeletalMageTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonSkeletalMageTimer : Timer
        {
            private Mobile m_Owner;

            public SummonSkeletalMageTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.TwoHanded) is NecromancerKiteShield))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    SkeletalMage skeletalMage = new SkeletalMage
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    skeletalMage.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Skeletal Mage rises to serve you!");
                }
            }
        }
    }
}
