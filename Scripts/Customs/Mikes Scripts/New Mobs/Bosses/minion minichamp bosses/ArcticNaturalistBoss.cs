using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the arctic overlord")]
    public class ArcticNaturalistBoss : ArcticNaturalist
    {
        [Constructable]
        public ArcticNaturalistBoss() : base()
        {
            Name = "Arctic Overlord";
            Title = "the Supreme Naturalist";

            // Enhance stats to match or exceed a boss-level enemy
            SetStr(1200); // Upper range of Barracoon or better
            SetDex(255);  // Max dexterity
            SetInt(250);  // High intelligence

            SetHits(12000); // Boss-tier health
            SetDamage(25, 40); // Enhanced damage range

            SetResistance(ResistanceType.Physical, 75, 85); // Boss-level resistances
            SetResistance(ResistanceType.Cold, 100, 120);   // Higher cold resistance (theme-appropriate)
            SetResistance(ResistanceType.Fire, 50, 70);     // Boss-level resistances
            SetResistance(ResistanceType.Poison, 50, 70);   // Increased poison resistance
            SetResistance(ResistanceType.Energy, 50, 70);   // Increased energy resistance

            SetSkill(SkillName.EvalInt, 100.0, 120.0);   // Higher skill values
            SetSkill(SkillName.Magery, 100.0, 120.0);    // Higher skill values
            SetSkill(SkillName.MagicResist, 130.0, 140.0); // Increased magic resist
            SetSkill(SkillName.Tactics, 100.0, 120.0);   // Increased tactics skill
            SetSkill(SkillName.Wrestling, 100.0, 120.0); // Increased wrestling skill

            Fame = 10000;  // High fame for a boss-tier enemy
            Karma = -10000; // Negative karma for an enemy of this caliber

            VirtualArmor = 100; // Higher virtual armor for more toughness

            // Attach a random ability for added difficulty
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

            // Optionally, add more boss-specific behavior or abilities
        }

        public ArcticNaturalistBoss(Serial serial) : base(serial)
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
