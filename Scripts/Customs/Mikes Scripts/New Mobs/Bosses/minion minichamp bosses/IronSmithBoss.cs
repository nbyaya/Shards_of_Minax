using System;
using Server.Items;
using Server.Targeting;
using System.Collections.Generic;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the iron overlord")]
    public class IronSmithBoss : IronSmith
    {
        private TimeSpan m_BuffDelay = TimeSpan.FromSeconds(10.0); // faster buff time for the boss
        public DateTime m_NextBuffTime;

        [Constructable]
        public IronSmithBoss() : base()
        {
            Name = "Iron Overlord";
            Title = "the Supreme Smith";
            
            // Enhanced Stats to match or exceed Barracoon-style boss stats
            SetStr(1200); // Enhanced strength
            SetDex(200); // Enhanced dexterity
            SetInt(200); // Enhanced intelligence

            SetHits(15000); // Enhanced hit points
            SetDamage(30, 45); // Higher damage range

            SetResistance(ResistanceType.Physical, 75, 85); // Increased resistance
            SetResistance(ResistanceType.Fire, 60, 80); 
            SetResistance(ResistanceType.Cold, 50, 70); 
            SetResistance(ResistanceType.Poison, 70, 80); 
            SetResistance(ResistanceType.Energy, 60, 80); 

            SetSkill(SkillName.Blacksmith, 120.0, 140.0); // Enhanced blacksmithing skill
            SetSkill(SkillName.Tactics, 100.0, 120.0); 
            SetSkill(SkillName.MagicResist, 100.0, 120.0); 
            SetSkill(SkillName.Wrestling, 90.0, 110.0); 

            Fame = 10000; // Increased fame for boss tier
            Karma = -10000; // Increased karma loss for boss tier

            VirtualArmor = 100; // Increased virtual armor for toughness

            // Attach the XmlRandomAbility for extra random traits
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            m_NextBuffTime = DateTime.Now + m_BuffDelay;
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot();
            
			PackItem(new BossTreasureBox());
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void OnThink()
        {
            if (DateTime.Now >= m_NextBuffTime)
            {
                // Buffing nearby allies (if applicable)
                List<Mobile> allies = new List<Mobile>();
                foreach (Mobile m in this.GetMobilesInRange(8))
                {
                    if (m != this && m is BaseCreature && ((BaseCreature)m).ControlMaster == this.ControlMaster)
                    {
                        allies.Add(m);
                    }
                }

                foreach (Mobile ally in allies)
                {
                    if (ally != null && ally.Alive && ally.Map == this.Map && ally.InRange(this, 8))
                    {
                        this.Say(true, "Your armor is now fortified by my mastery!");
                        ally.VirtualArmorMod += 30; // Enhanced buff effect
                    }
                }

                m_NextBuffTime = DateTime.Now + m_BuffDelay;
            }

            base.OnThink();
        }

        public IronSmithBoss(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
