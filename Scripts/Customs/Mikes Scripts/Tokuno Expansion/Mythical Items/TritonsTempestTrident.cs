using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class TritonsTempestTrident : Pitchfork
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public TritonsTempestTrident()
        {
            Weight = 8.0;
            Name = "Triton's Tempest Trident";
            Hue = 1266; // Deep ocean blue

            // Weapon attributes
            WeaponAttributes.HitLightning = 50;
            WeaponAttributes.HitLeechStam = 25;
            WeaponAttributes.HitLowerDefend = 25;
            Attributes.WeaponDamage = 25;
            Attributes.WeaponSpeed = 15;
            Attributes.AttackChance = 10;

            // Bonuses
            Attributes.BonusStr = 10;
            Attributes.BonusDex = 10;
            Attributes.RegenStam = 5;
            Attributes.Luck = 50;
            Attributes.SpellChanneling = 1; // Allows spellcasting while equipped

            // Skill bonuses
            SkillBonuses.SetValues(0, SkillName.Fencing, 15.0);
            SkillBonuses.SetValues(1, SkillName.Tactics, 15.0);

            // Follower bonus
            m_BonusFollowers = 2;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public TritonsTempestTrident(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel the call of the deep, allowing you to command more creatures!");

                // Start summon timer
                StopSummonTimer();
                m_Timer = new SummonTritonTimer(pm);
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
                pm.SendMessage(37, "The call of the deep fades, and your command over creatures weakens.");
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
            list.Add("Summons Tritons");
            list.Add("Increases maximum followers by 2");
            list.Add("Channels the power of the ocean");
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
                m_Timer = new SummonTritonTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonTritonTimer : Timer
        {
            private Mobile m_Owner;

            public SummonTritonTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.OneHanded) is TritonsTempestTrident))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before proceeding
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                // Check if the player has room for more followers
                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Triton triton = new Triton
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    triton.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Triton emerges from the depths to serve you!");
                }
            }
        }
    }
}
