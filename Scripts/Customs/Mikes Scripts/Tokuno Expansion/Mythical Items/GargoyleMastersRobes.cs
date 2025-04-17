using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class GargoyleMastersRobes : Robe
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public GargoyleMastersRobes()
        {
            Weight = 2.0;
            Name = "Gargoyle Master's Robes";
            Hue = 1194; // Dark green hue, adjust as desired

            // Set attributes and bonuses
            Attributes.BonusStr = 8;
            Attributes.BonusDex = 8;
            Attributes.BonusInt = 12;
            Attributes.BonusMana = 15;
            Attributes.RegenMana = 4;
            Attributes.RegenHits = 3;
            Attributes.CastSpeed = 1;
            Attributes.CastRecovery = 1;
            Attributes.LowerManaCost = 10;
            Attributes.SpellDamage = 8;
            Attributes.DefendChance = 10;
            Attributes.Luck = 50;

            // Resistances
            Resistances.Physical = 8;
            Resistances.Fire = 8;
            Resistances.Cold = 8;
            Resistances.Poison = 8;
            Resistances.Energy = 8;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Necromancy, 10.0);
            SkillBonuses.SetValues(1, SkillName.SpiritSpeak, 10.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 1;
        }

        public GargoyleMastersRobes(Serial serial) : base(serial)
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

                // Only start the summon timer if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    StopSummonTimer();
                    m_Timer = new SummonGargoyleTimer(pm);
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
                pm.SendMessage(37, "You feel your command over creatures weaken.");
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
            list.Add("Summons Putrid Undead Gargoyles");
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

            // Reinitialize timer if equipped on restart and auto-summon is enabled
            if (Parent is Mobile mob && AutoSummonManager.IsAutoSummonEnabled(mob))
            {
                m_Timer = new SummonGargoyleTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonGargoyleTimer : Timer
        {
            private Mobile m_Owner;

            public SummonGargoyleTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.OuterTorso) is GargoyleMastersRobes))
                {
                    Stop();
                    return;
                }

                // Check if auto-summon is enabled before continuing
                if (!AutoSummonManager.IsAutoSummonEnabled(m_Owner))
                    return;

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    PutridUndeadGargoyle gargoyle = new PutridUndeadGargoyle
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    gargoyle.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Putrid Undead Gargoyle rises to serve you!");
                }
            }
        }
    }
}
