using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class ToxicDefenderShield : HeaterShield
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public ToxicDefenderShield()
        {
            Weight = 6.0;
            Name = "Toxic Defender Shield";
            Hue = 1167; // Poison-like green color

            // Set attributes and bonuses
            Attributes.BonusStr = 8;
            Attributes.BonusDex = 8;
            Attributes.BonusHits = 15;
            Attributes.RegenHits = 3;
            Attributes.DefendChance = 20;
            Attributes.ReflectPhysical = 15;
            Attributes.SpellDamage = 8;
            Attributes.CastRecovery = 1;
            Attributes.CastSpeed = 1;
            Attributes.LowerManaCost = 10;

            // Resistances
            PoisonBonus = 20;
            PhysicalBonus = 10;

            // Skill Bonus
            SkillBonuses.SetValues(0, SkillName.Parry, 15.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public ToxicDefenderShield(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel capable of commanding more creatures!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonToxicElementalTimer(pm);
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
                pm.SendMessage(37, "You feel your control over creatures weaken.");
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
            list.Add("Summons Toxic Elementals");
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
            if (Parent is Mobile mob && AutoSummonManager.IsAutoSummonEnabled(mob))
            {
                m_Timer = new SummonToxicElementalTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonToxicElementalTimer : Timer
        {
            private Mobile m_Owner;

            public SummonToxicElementalTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.TwoHanded) is ToxicDefenderShield))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    ToxicElemental elemental = new ToxicElemental
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    elemental.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Toxic Elemental emerges to serve you!");
                }
            }
        }
    }
}
