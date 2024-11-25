using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a frostclaw corpse")]
    public class SiberianFrostclawBoss : SiberianFrostclaw
    {
        [Constructable]
        public SiberianFrostclawBoss() : base()
        {
            Name = "Siberian Frostclaw";
            Title = "the Glacial Terror";

            // Update stats to match or exceed Barracoon's stats
            SetStr(1200); // Upper strength limit from Barracoon
            SetDex(255);  // Max dexterity to match high mobility
            SetInt(250);  // High intelligence for more varied abilities

            SetHits(12000);  // Boss-tier health
            SetDamage(29, 38);  // Increase damage to be more powerful

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 75);  // Improved resistances
            SetResistance(ResistanceType.Fire, 80);  // Higher fire resistance
            SetResistance(ResistanceType.Cold, 75);  // Stronger cold resistance
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60);

            SetSkill(SkillName.Anatomy, 50.0, 75.0);  // Slightly increased skill
            SetSkill(SkillName.EvalInt, 100.0, 150.0);  // Increased eval intelligence
            SetSkill(SkillName.Magery, 110.0, 150.0);  // Magery skill boost
            SetSkill(SkillName.Meditation, 50.0, 75.0);  
            SetSkill(SkillName.MagicResist, 150.0, 200.0);  // Strong magic resist
            SetSkill(SkillName.Tactics, 100.0, 120.0);  // Boosted tactics for combat efficiency
            SetSkill(SkillName.Wrestling, 100.0, 120.0);  // Better combat ability

            Fame = 30000;  // Increased fame for a boss-tier NPC
            Karma = -30000;  // Boss-tier negative karma

            VirtualArmor = 100;  // Stronger virtual armor

            // Attach a random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());
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
            base.OnThink();
            // Additional boss logic or cooldowns could be added here
        }

        public SiberianFrostclawBoss(Serial serial) : base(serial)
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
