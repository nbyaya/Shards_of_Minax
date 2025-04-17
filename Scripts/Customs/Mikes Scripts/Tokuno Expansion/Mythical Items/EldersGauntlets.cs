using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class EldersGauntlets : PlateGloves
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public EldersGauntlets()
        {
            Weight = 2.0;
            Name = "Elder's Gauntlets";
            Hue = 1157; // Unique metallic hue

            // Set attributes and bonuses
            Attributes.BonusStr = 8;
            Attributes.BonusDex = 8;
            Attributes.BonusMana = 15;
            Attributes.RegenMana = 3;
            Attributes.DefendChance = 10;
            Attributes.AttackChance = 10;
            Attributes.Luck = 75;
            Attributes.ReflectPhysical = 8;

            // Resistances
            PhysicalBonus = 8;
            FireBonus = 6;
            ColdBonus = 6;
            PoisonBonus = 6;
            EnergyBonus = 6;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Bushido, 10.0);
            SkillBonuses.SetValues(1, SkillName.Parry, 10.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public EldersGauntlets(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel empowered to command more creatures!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonYomotsuElderTimer(pm);
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
            list.Add("Summons Yomotsu Elders");
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

            // Reinitialize timer if equipped on restart and autosummon is enabled
            if (Parent is Mobile mob && AutoSummonManager.IsAutoSummonEnabled(mob))
            {
                m_Timer = new SummonYomotsuElderTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonYomotsuElderTimer : Timer
        {
            private Mobile m_Owner;

            public SummonYomotsuElderTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Gloves) is EldersGauntlets))
                {
                    Stop();
                    return;
                }

                // Only summon if autosummon is enabled and the player has room for more followers
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner) || m_Owner.Followers >= m_Owner.FollowersMax)
                    return;

                YomotsuElder elder = new YomotsuElder
                {
                    Controlled = true,
                    ControlMaster = m_Owner
                };

                elder.MoveToWorld(m_Owner.Location, m_Owner.Map);
                m_Owner.SendMessage(38, "A Yomotsu Elder emerges to serve you!");
            }
        }
    }
}
