using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a mystic ferret boss corpse")]
    public class MysticFerretBoss : MysticFerret
    {
        [Constructable]
        public MysticFerretBoss()
            : base()
        {
            Name = "Mystic Ferret Boss";
            Title = "the Archmage Ferret";

            // Enhance stats to match a boss-tier creature
            SetStr(1200); // Enhanced strength for boss
            SetDex(255); // Maximum dexterity
            SetInt(250); // Enhanced intelligence for boss

            SetHits(12000); // Increase health to make it a boss-tier challenge
            SetDamage(45, 60); // Increased damage range for boss-tier

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 75, 85); // Higher resistance values
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 65, 75);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 60, 75);

            SetSkill(SkillName.Anatomy, 60.0, 100.0); // Enhanced skill for boss
            SetSkill(SkillName.EvalInt, 120.0, 150.0); 
            SetSkill(SkillName.Magery, 125.0, 150.0);
            SetSkill(SkillName.Meditation, 60.0, 100.0);
            SetSkill(SkillName.MagicResist, 120.0, 150.0);
            SetSkill(SkillName.Tactics, 120.0, 150.0);
            SetSkill(SkillName.Wrestling, 120.0, 150.0);

            Fame = 30000; // Increased fame
            Karma = -30000; // Increased karma (negative for evil boss)

            VirtualArmor = 100; // Stronger armor

            PackItem(new BossTreasureBox());
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
            // Additional boss-specific logic could be added here if necessary
        }

        public MysticFerretBoss(Serial serial)
            : base(serial)
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
