using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class RavagersRobes : Robe
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public RavagersRobes() 
        {
            Weight = 2.0;
            Name = "Ravager's Robes";
            Hue = 45; // Dark purple, change as desired

            // Set attributes and bonuses
            Attributes.BonusStr = 12;
            Attributes.BonusDex = 8;
            Attributes.BonusInt = 15;
            Attributes.BonusHits = 25;
            Attributes.BonusMana = 30;
            Attributes.RegenHits = 6;
            Attributes.RegenMana = 6;
            Attributes.DefendChance = 10;
            Attributes.AttackChance = 10;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 2;
            Attributes.LowerManaCost = 10;
            Attributes.SpellDamage = 15;
            Attributes.NightSight = 1;

            // Resistances
            Resistances.Physical = 12;
            Resistances.Fire = 12;
            Resistances.Cold = 12;
            Resistances.Poison = 12;
            Resistances.Energy = 12;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Necromancy, 15.0);
            SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 15.0);
            SkillBonuses.SetValues(2, SkillName.EvalInt, 10.0);
            SkillBonuses.SetValues(3, SkillName.Tactics, 10.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public RavagersRobes(Serial serial) : base(serial)
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

                // Start summon timer if autosummon is enabled
                StopSummonTimer();

                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonRavagerTimer(pm);
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
            list.Add("Summons Ravagers");
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
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonRavagerTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonRavagerTimer : Timer
        {
            private Mobile m_Owner;

            public SummonRavagerTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.OuterTorso) is RavagersRobes))
                {
                    Stop();
                    return;
                }

                // Only summon if autosummon is enabled and the player has room for more followers
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner) || m_Owner.Followers >= m_Owner.FollowersMax)
                    return;

                Ravager ravager = new Ravager
                {
                    Controlled = true,
                    ControlMaster = m_Owner
                };

                ravager.MoveToWorld(m_Owner.Location, m_Owner.Map);
                m_Owner.SendMessage(38, "A Ravager emerges to serve you!");
            }
        }
    }
}
