using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class SavageShamansRegalia : Robe
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public SavageShamansRegalia()
        {
            Weight = 2.0;
            Name = "Savage Shaman's Regalia";
            Hue = 1161; // A tribal green color, change as desired

            // Set attributes and bonuses
            Attributes.BonusStr = 10;
            Attributes.BonusDex = 10;
            Attributes.BonusInt = 10;
            Attributes.BonusHits = 20;
            Attributes.BonusStam = 20;
            Attributes.BonusMana = 20;
            Attributes.RegenHits = 5;
            Attributes.RegenStam = 5;
            Attributes.RegenMana = 5;
            Attributes.WeaponDamage = 15;
            Attributes.DefendChance = 10;
            Attributes.AttackChance = 10;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 2;
            Attributes.LowerManaCost = 10;
            Attributes.LowerRegCost = 15;
            Attributes.SpellDamage = 10;
            Attributes.NightSight = 1;

            // Resistances
            Resistances.Physical = 10;
            Resistances.Fire = 10;
            Resistances.Cold = 10;
            Resistances.Poison = 10;
            Resistances.Energy = 10;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Magery, 15.0);
            SkillBonuses.SetValues(1, SkillName.EvalInt, 15.0);
            SkillBonuses.SetValues(2, SkillName.SpiritSpeak, 15.0);
            SkillBonuses.SetValues(3, SkillName.Focus, 15.0);
            SkillBonuses.SetValues(4, SkillName.Meditation, 15.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 1;
        }

        public SavageShamansRegalia(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel like you could command more creatures now!");

                // Start summon timer if auto summon is enabled
                StopSummonTimer();
                m_Timer = new SummonSavageShamanTimer(pm);
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
            list.Add("Summons Savage Shamans");
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
                m_Timer = new SummonSavageShamanTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonSavageShamanTimer : Timer
        {
            private Mobile m_Owner;

            public SummonSavageShamanTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(10.0), TimeSpan.FromSeconds(10.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                // Stop if the owner is invalid or the item is not equipped
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.OuterTorso) is SavageShamansRegalia))
                {
                    Stop();
                    return;
                }

                // Check if auto-summon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                // Only summon if the player has room for more followers
                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    SavageShaman shaman = new SavageShaman
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    shaman.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Savage Shaman emerges to serve you!");
                }
            }
        }
    }
}
