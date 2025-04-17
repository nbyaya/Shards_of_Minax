using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class VenomlordsShroud : Robe
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public VenomlordsShroud()
        {
            Weight = 1.0;
            Name = "Venomlord's Shroud";
            Hue = 1175; // Toxic green hue

            // Set attributes and bonuses
            ClothingAttributes.SelfRepair = 4;
            Attributes.BonusInt = 15;
            Attributes.BonusMana = 20;
            Attributes.RegenMana = 3;
            Attributes.LowerManaCost = 10;
            Attributes.SpellDamage = 15;
            Attributes.NightSight = 1;

            SkillBonuses.SetValues(0, SkillName.Poisoning, 20.0);
            SkillBonuses.SetValues(1, SkillName.Necromancy, 15.0);

            Resistances.Physical = 10;
            Resistances.Fire = 5;
            Resistances.Cold = 10;
            Resistances.Poison = 20;
            Resistances.Energy = 10;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public VenomlordsShroud(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel the toxic power of the Venomlord coursing through you!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonGreaterPoisonElementalTimer(pm);
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
                pm.SendMessage(37, "The toxic power of the Venomlord fades away.");
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
            list.Add("Summons Greater Poison Elementals");
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
                // Start the summon timer if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonGreaterPoisonElementalTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonGreaterPoisonElementalTimer : Timer
        {
            private Mobile m_Owner;

            public SummonGreaterPoisonElementalTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                // Stop if the owner is invalid or the item is not equipped
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.OuterTorso) is VenomlordsShroud))
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
                    GreaterPoisonElemental elemental = new GreaterPoisonElemental
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    elemental.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Greater Poison Elemental rises from the ground to serve you!");
                }
            }
        }
    }
}
