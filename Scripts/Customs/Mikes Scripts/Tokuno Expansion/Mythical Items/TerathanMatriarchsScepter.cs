using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class TerathanMatriarchsScepter : Scepter
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public TerathanMatriarchsScepter()
        {
            Weight = 5.0;
            Name = "Terathan Matriarch's Scepter";
            Hue = 1175; // A greenish hue to represent poison/terathan colors

            // Set attributes and bonuses
            Attributes.BonusStr = 5;
            Attributes.BonusDex = 5;
            Attributes.BonusInt = 10;
            Attributes.BonusHits = 15;
            Attributes.RegenMana = 3;
            Attributes.DefendChance = 10;
            Attributes.AttackChance = 10;
            Attributes.CastSpeed = 1;
            Attributes.SpellDamage = 10;
            Attributes.Luck = 50;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Mysticism, 10.0);
            SkillBonuses.SetValues(1, SkillName.Spellweaving, 10.0);
            SkillBonuses.SetValues(2, SkillName.AnimalTaming, 5.0);

            // Weapon Attributes
            WeaponAttributes.HitPoisonArea = 30;
            WeaponAttributes.HitMagicArrow = 25;
            WeaponAttributes.HitLeechMana = 15;

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 1;
        }

        public TerathanMatriarchsScepter(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "The power of the Terathan Matriarch flows through you!");

                // Start summon timer if autosummon is enabled
                StopSummonTimer();
                m_Timer = new SummonTerathanMatriarchTimer(pm);
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
                pm.SendMessage(37, "The power of the Terathan Matriarch fades away...");
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
            list.Add("Summons Terathan Matriarchs");
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
                m_Timer = new SummonTerathanMatriarchTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonTerathanMatriarchTimer : Timer
        {
            private Mobile m_Owner;

            public SummonTerathanMatriarchTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0)) // Adjust time as desired
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.OneHanded) is TerathanMatriarchsScepter))
                {
                    Stop();
                    return;
                }

                // Check if autosummon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    TerathanMatriarch matriarch = new TerathanMatriarch
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    matriarch.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Terathan Matriarch emerges to aid you!");
                }
            }
        }
    }
}
