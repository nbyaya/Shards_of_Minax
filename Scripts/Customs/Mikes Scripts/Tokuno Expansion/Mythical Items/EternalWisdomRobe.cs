using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class EternalWisdomRobe : Robe
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public EternalWisdomRobe()
        {
            Weight = 2.0;
            Name = "Eternal Wisdom Robe";
            Hue = 1260; // Mystical light blue

            // Set attributes and bonuses
            Attributes.BonusInt = 15;
            Attributes.RegenMana = 5;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 2;
            Attributes.SpellDamage = 15;
            Attributes.LowerManaCost = 10;
            Attributes.Luck = 150;

            Resistances.Physical = 8;
            Resistances.Fire = 8;
            Resistances.Cold = 10;
            Resistances.Poison = 10;
            Resistances.Energy = 12;

            SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
            SkillBonuses.SetValues(1, SkillName.EvalInt, 15.0);
            SkillBonuses.SetValues(2, SkillName.Meditation, 10.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public EternalWisdomRobe(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "The wisdom of the Meer guides you to command more followers!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonMeerEternalTimer(pm);
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
                pm.SendMessage(37, "The presence of the Meer fades, reducing your command potential.");
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
            list.Add("Summons Meer Eternal");
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
                m_Timer = new SummonMeerEternalTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonMeerEternalTimer : Timer
        {
            private Mobile m_Owner;

            public SummonMeerEternalTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(10.0), TimeSpan.FromSeconds(10.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                // Stop if the owner is invalid or the item is not equipped
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.OuterTorso) is EternalWisdomRobe))
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
                    MeerEternal meer = new MeerEternal
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    meer.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Meer Eternal appears, guided by your wisdom!");
                }
            }
        }
    }
}
