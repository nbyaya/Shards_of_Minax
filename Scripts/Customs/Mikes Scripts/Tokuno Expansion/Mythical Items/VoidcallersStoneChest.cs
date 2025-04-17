using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class VoidcallersStoneChest : LeatherGloves
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public VoidcallersStoneChest()
        {
            Weight = 10.0;
            Name = "Voidcaller's Gloves";
            Hue = 2075; // Dark, void-like color

            // Set attributes and bonuses
            Attributes.BonusInt = 15; // Enhances magical abilities
            Attributes.SpellDamage = 20; // Increases spell damage
            Attributes.CastSpeed = 1; // Faster casting
            Attributes.CastRecovery = 2; // Faster cast recovery
            Attributes.LowerManaCost = 10; // Reduces mana cost
            Attributes.NightSight = 1; // Grants night sight

            // Resistances
            PhysicalBonus = 15;
            FireBonus = 10;
            ColdBonus = 10;
            PoisonBonus = 15;
            EnergyBonus = 20; // Strong against energy (void-themed)

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Mysticism, 15.0); // Enhances Mysticism
            SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 10.0); // Enhances Spirit Speak

            // Follower bonus
            m_BonusFollowers = 2; // Increases follower count by 2

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public VoidcallersStoneChest(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel the void's power increasing your command over creatures!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm)) // Check if autosummon is enabled
                {
                    m_Timer = new SummonVoidManifestationTimer(pm);
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
                pm.SendMessage(37, "The void's power fades, reducing your command over creatures.");
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
            list.Add("Summons Void Manifestations");
            list.Add("Increases maximum followers by 2");
            list.Add("Enhances Mysticism and Spirit Speak");
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
                // Only start the timer if auto-summon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonVoidManifestationTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonVoidManifestationTimer : Timer
        {
            private Mobile m_Owner;

            public SummonVoidManifestationTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.InnerTorso) is VoidcallersStoneChest))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before proceeding
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                // Summon if there's space for more followers
                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    VoidManifestation manifestation = new VoidManifestation
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    manifestation.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Void Manifestation emerges from the void to serve you!");
                }
            }
        }
    }
}
