using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class RotwormMastersRobes : Robe
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public RotwormMastersRobes()
        {
            Weight = 2.0;
            Name = "Rotworm Master's Robes";
            Hue = 2969; // A rotworm-like color, adjust as desired

            // Set attributes and bonuses
            Attributes.BonusStr = 8;
            Attributes.BonusDex = 8;
            Attributes.BonusInt = 12;
            Attributes.BonusHits = 15;
            Attributes.BonusMana = 25;
            Attributes.RegenHits = 3;
            Attributes.RegenMana = 5;
            Attributes.DefendChance = 10;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 2;
            Attributes.LowerManaCost = 10;
            Attributes.SpellDamage = 15;
            Attributes.Luck = 50;

            // Resistances
            Resistances.Physical = 12;
            Resistances.Fire = 8;
            Resistances.Cold = 10;
            Resistances.Poison = 15;
            Resistances.Energy = 8;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Necromancy, 10.0);
            SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 10.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public RotwormMastersRobes(Serial serial) : base(serial)
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

                // Start summon timer only if auto-summon is enabled
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonRotwormTimer(pm);
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
                pm.SendMessage(37, "You feel your control over creatures diminish.");
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
            list.Add("Summons Rotworms");
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
                // Start summon timer only if auto-summon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonRotwormTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonRotwormTimer : Timer
        {
            private Mobile m_Owner;

            public SummonRotwormTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.OuterTorso) is RotwormMastersRobes))
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
                    Rotworm rotworm = new Rotworm
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    rotworm.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Rotworm emerges from the ground to serve you!");
                }
            }
        }
    }
}
