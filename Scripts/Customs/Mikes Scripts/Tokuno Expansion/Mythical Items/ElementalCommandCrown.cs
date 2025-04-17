using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class ElementalCommandCrown : PlateHelm
    {
        private Timer m_Timer;
        private int m_BonusFollowers;

        [Constructable]
        public ElementalCommandCrown()
        {
            Weight = 5.0;
            Name = "Elemental Command Crown";
            Hue = 1153; // Valorite hue

            // Set attributes and bonuses
            Attributes.BonusInt = 15;
            Attributes.BonusMana = 25;
            Attributes.RegenMana = 8;
            Attributes.DefendChance = 10;
            Attributes.SpellDamage = 15;
            Attributes.CastRecovery = 2;
            Attributes.CastSpeed = 1;
            Attributes.LowerManaCost = 10;

            // Resistances
            PhysicalBonus = 8;
            FireBonus = 8;
            ColdBonus = 8;
            PoisonBonus = 8;
            EnergyBonus = 8;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Mysticism, 15.0);
            SkillBonuses.SetValues(1, SkillName.EvalInt, 10.0);

            // Attach XmlLevelItem
            XmlAttach.AttachTo(this, new XmlLevelItem());

            // Follower bonus
            m_BonusFollowers = 2;
        }

        public ElementalCommandCrown(Serial serial) : base(serial)
        {
        }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                // Add follower bonus
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "You feel empowered to command more followers!");

                // Start summon timer if autosummon is enabled
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    StopSummonTimer();
                    m_Timer = new SummonValoriteElementalTimer(pm);
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
                pm.SendMessage(37, "You feel your command over followers wane.");
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
            list.Add("Summons Valorite Elementals");
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
            if (Parent is Mobile mob && AutoSummonManager.IsAutoSummonEnabled(mob))
            {
                m_Timer = new SummonValoriteElementalTimer(mob);
                m_Timer.Start();
            }
        }

        private class SummonValoriteElementalTimer : Timer
        {
            private Mobile m_Owner;

            public SummonValoriteElementalTimer(Mobile owner)
                : base(TimeSpan.FromSeconds(15.0), TimeSpan.FromSeconds(15.0))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                // Stop if the owner is invalid or the item is not equipped
                if (m_Owner == null || m_Owner.Deleted || !(m_Owner.FindItemOnLayer(Layer.Helm) is ElementalCommandCrown))
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
                    ValoriteElemental elemental = new ValoriteElemental
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    elemental.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(38, "A Valorite Elemental emerges to serve you!");
                }
            }
        }
    }
}
