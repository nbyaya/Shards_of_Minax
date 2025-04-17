using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class OgresGauntlets : PlateGloves
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public OgresGauntlets()
        {
            Weight = 5.0;
            Name = "Ogre's Gauntlets";
            Hue = 1175; // Ogre-themed color

            // Set attributes and bonuses
            Attributes.BonusStr = 20;
            Attributes.RegenHits = 5;
            Attributes.DefendChance = 10;
            Attributes.AttackChance = 10;
            Attributes.ReflectPhysical = 15;

            PhysicalBonus = 15;
            FireBonus = 10;
            ColdBonus = 10;
            PoisonBonus = 5;
            EnergyBonus = 5;

            SkillBonuses.SetValues(0, SkillName.Macing, 20.0);
            SkillBonuses.SetValues(1, SkillName.Parry, 15.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public OgresGauntlets(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel a primal strength course through you, allowing you to command more creatures!");

                // Start summon timer only if autosummon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm)) 
                {
                    m_Timer = new SummonOgreTimer(pm);
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
                pm.SendMessage(37, "The primal strength leaves you, reducing your ability to command creatures.");
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
            list.Add("Summons Ogres to fight for you");
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
            if (Parent is Mobile mob)
            {
                // Check autosummon setting when item is re-equipped
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonOgreTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonOgreTimer : Timer
        {
            private Mobile m_Owner;

            public SummonOgreTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Gloves) is OgresGauntlets))
                {
                    Stop();
                    return;
                }

                // Only summon if autosummon is enabled and there is room for the ogre
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Ogre ogre = new Ogre
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    ogre.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "An Ogre stomps forth to serve you!");
                }
            }
        }
    }
}
