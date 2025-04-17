using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class PixieWhisperRobes : FemaleElvenRobe
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public PixieWhisperRobes()
        {
            Weight = 1.0;
            Name = "Pixie Whisper Robes";
            Hue = 1153; // Light, magical blue-green color

            // Set attributes and bonuses
            ClothingAttributes.SelfRepair = 3;
            Attributes.BonusInt = 15;
            Attributes.BonusMana = 10;
            Attributes.RegenMana = 2;
            Attributes.CastSpeed = 1;
            Attributes.LowerManaCost = 10;
            Attributes.Luck = 200;

            SkillBonuses.SetValues(0, SkillName.Spellweaving, 15.0);
            SkillBonuses.SetValues(1, SkillName.Meditation, 10.0);

            Resistances.Physical = 5;
            Resistances.Fire = 5;
            Resistances.Cold = 10;
            Resistances.Poison = 5;
            Resistances.Energy = 10;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 1;
        }

        public PixieWhisperRobes(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel a gentle connection to the spirits of nature!");

                // Start summon timer
                StopSummonTimer();
                m_Timer = new SummonPixieTimer(pm);
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
                pm.SendMessage(37, "You feel the connection to the spirits weaken.");
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
            list.Add("Summons Pixies to assist you");
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
                m_Timer = new SummonPixieTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonPixieTimer : Timer
        {
            private Mobile m_Owner;

            public SummonPixieTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.OuterTorso) is PixieWhisperRobes))
                {
                    Stop();
                    return;
                }

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    Pixie pixie = new Pixie
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    pixie.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Pixie appears, fluttering to your aid!");
                }
            }
        }
    }
}
