using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class SpectralGuardPlate : PlateChest
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public SpectralGuardPlate()
        {
            Weight = 10.0;
            Name = "Spectral Guard Plate";
            Hue = 1150; // Ghostly white

            // Set attributes and bonuses
            Attributes.BonusHits = 25;
            Attributes.BonusStr = 15;
            Attributes.DefendChance = 20;
            Attributes.RegenHits = 3;
            Attributes.RegenMana = 3;
            Attributes.ReflectPhysical = 15;
            Attributes.CastSpeed = 1;
            Attributes.Luck = 150;

            // Resistances
            PhysicalBonus = 12;
            FireBonus = 8;
            ColdBonus = 12;
            PoisonBonus = 8;
            EnergyBonus = 10;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Parry, 10.0);
            SkillBonuses.SetValues(1, SkillName.Tactics, 10.0);

            // Follower bonus
            m_BonusFollowers = 1;
        }

        public SpectralGuardPlate(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "The Spectral Guard enhances your ability to command followers!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonSpectralArmorTimer(pm);
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
                pm.SendMessage(37, "The Spectral Guard's influence fades, reducing your follower capacity.");
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
            list.Add("Summons Spectral Armor");
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
                    m_Timer = new SummonSpectralArmorTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonSpectralArmorTimer : Timer
        {
            private Mobile m_Owner;

            public SummonSpectralArmorTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(12.0), TimeSpan.FromSeconds(12.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.InnerTorso) is SpectralGuardPlate))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before proceeding
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                // Only summon if the player has room for more followers
                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    SpectralArmour spectral = new SpectralArmour
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    spectral.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Spectral Armor emerges to guard you!");
                }
            }
        }
    }
}
