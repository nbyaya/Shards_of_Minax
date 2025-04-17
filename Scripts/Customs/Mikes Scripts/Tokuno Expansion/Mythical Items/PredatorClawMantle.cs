using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Items
{
    public class PredatorClawMantle : Cloak
    {
        private Timer m_Timer;
        private int m_BonusFollowers = 1;

        [Constructable]
        public PredatorClawMantle()
        {
            Weight = 1.0;
            Name = "Predator Claw Mantle";
            Hue = 1157; // Fiery red-orange
            LootType = LootType.Blessed;

            // Core Attributes
            Attributes.BonusStr = 15;
            Attributes.BonusDex = 15;
            Attributes.RegenHits = 3;
            Attributes.RegenStam = 3;
            Attributes.RegenMana = 3;
            Attributes.AttackChance = 15;
            Attributes.DefendChance = 15;
            Attributes.SpellDamage = 10;
            Attributes.Luck = 150;
            Attributes.NightSight = 1;

            // Resistances
            Resistances.Fire = 20;
            Resistances.Physical = 15;
            Resistances.Energy = 10;
            Resistances.Poison = 15;

            // Skill Bonuses
            SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
            SkillBonuses.SetValues(1, SkillName.Tactics, 15.0);
            SkillBonuses.SetValues(2, SkillName.Tracking, 15.0);
            SkillBonuses.SetValues(3, SkillName.Ninjitsu, 10.0);

            // Special Properties
            WeaponAttributes.HitFireball = 40;
            WeaponAttributes.HitLeechStam = 35;

            XmlAttach.AttachTo(this, new XmlLevelItem());
        }

        public PredatorClawMantle(Serial serial) : base(serial) { }

        public override void OnAdded(object parent)
        {
            base.OnAdded(parent);

            if (parent is PlayerMobile pm)
            {
                pm.FollowersMax += m_BonusFollowers;
                pm.SendMessage(78, "Your connection to the predator realm increases your follower capacity!");

                // Check if autosummon is enabled and start the summon timer accordingly
                StopSummonTimer();
                if (AutoSummonManager.IsAutoSummonEnabled(pm))
                {
                    m_Timer = new SummonHellCatTimer(pm);
                    m_Timer.Start();
                }
            }
        }

        public override void OnRemoved(object parent)
        {
            base.OnRemoved(parent);

            if (parent is PlayerMobile pm)
            {
                pm.FollowersMax -= m_BonusFollowers;
                pm.SendMessage(37, "Your connection to the predator realm fades...");
            }

            // Stop the summon timer
            StopSummonTimer();
        }

        private void StopSummonTimer()
        {
            m_Timer?.Stop();
            m_Timer = null;
        }

        public override void AddNameProperties(ObjectPropertyList list)
        {
            base.AddNameProperties(list);
            list.Add("Summons Predator Hell Cats");
            list.Add("Increases maximum followers");
            list.Add("Burns with hellish fury");
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
            writer.Write(m_BonusFollowers);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
            m_BonusFollowers = reader.ReadInt();

            if (Parent is Mobile mob)
            {
                // Reinitialize the timer if the item is equipped upon load
                if (AutoSummonManager.IsAutoSummonEnabled(mob))
                {
                    m_Timer = new SummonHellCatTimer(mob);
                    m_Timer.Start();
                }
            }
        }

        private class SummonHellCatTimer : Timer
        {
            private readonly Mobile m_Owner;

            public SummonHellCatTimer(Mobile owner) : base(TimeSpan.FromSeconds(8), TimeSpan.FromSeconds(12))
            {
                m_Owner = owner;
                Priority = TimerPriority.OneSecond;
            }

            protected override void OnTick()
            {
                if (m_Owner?.Deleted != false || !(m_Owner.FindItemOnLayer(Layer.Cloak) is PredatorClawMantle))
                {
                    Stop();
                    return;
                }

                if (m_Owner.Followers < m_Owner.FollowersMax)
                {
                    var hellCat = new PredatorHellCat
                    {
                        Controlled = true,
                        ControlMaster = m_Owner
                    };

                    hellCat.MoveToWorld(m_Owner.Location, m_Owner.Map);
                    m_Owner.SendMessage(54, "A Predator Hell Cat answers your call!");
                }
            }
        }
    }
}
