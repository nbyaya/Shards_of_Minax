using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class FrostbindersCloak : Cloak
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public FrostbindersCloak()
        {
            Weight = 2.0;
            Name = "Frostbinder's Cloak";
            Hue = 1152; // Frosty blue color

            // Set attributes and bonuses
            ClothingAttributes.SelfRepair = 3;
            Attributes.BonusInt = 15;
            Attributes.RegenMana = 5;
            Attributes.DefendChance = 10;
            Attributes.CastSpeed = 1;
            Attributes.SpellDamage = 10;

            Resistances.Cold = 15;
            Resistances.Physical = 5;
            Resistances.Fire = -5; // Vulnerable to fire due to icy theme

            SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
            SkillBonuses.SetValues(1, SkillName.EvalInt, 20.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public FrostbindersCloak(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel a frosty aura granting you control over more creatures.");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))  // Check if autosummon is enabled
                {
                    m_Timer = new SummonIceSerpentTimer(pm);
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
                pm.SendMessage(37, "The frosty aura fades, and your control over creatures diminishes.");
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
            list.Add("Summons Ice Serpents");
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
                // Start summon timer if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonIceSerpentTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonIceSerpentTimer : Timer
        {
            private Mobile m_Owner;

            public SummonIceSerpentTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(10.0), TimeSpan.FromSeconds(10.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Cloak) is FrostbindersCloak))
                {
                    Stop();
                    return;
                }

                // Stop if autosummon is disabled
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    IceSerpent serpent = new IceSerpent
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    serpent.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "An Ice Serpent emerges from the frost to serve you!");
                }
            }
        }
    }
}
