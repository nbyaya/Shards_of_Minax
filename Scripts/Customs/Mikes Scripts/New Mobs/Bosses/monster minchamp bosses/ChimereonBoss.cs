using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a chimereon boss corpse")]
    public class ChimereonBoss : Chimereon
    {
        [Constructable]
        public ChimereonBoss() : base()
        {
            // Enhanced stats for the boss version
            Name = "Chimereon the Supreme Shapeshifter";
            Title = "the Chaos Incarnate";
            Hue = 1150; // Unique color to set it apart

            SetStr(1200, 1500); // Higher strength than normal Chimereon
            SetDex(255, 300); // Increased dexterity for more agility
            SetInt(250, 350); // Boosted intelligence for better spellcasting

            SetHits(12000, 15000); // Higher health for a tougher fight

            SetDamage(45, 55); // Increased damage output

            SetResistance(ResistanceType.Physical, 75, 90); // Stronger physical resistance
            SetResistance(ResistanceType.Fire, 80, 90); // Stronger fire resistance
            SetResistance(ResistanceType.Cold, 60, 80); // Stronger cold resistance
            SetResistance(ResistanceType.Poison, 80, 90); // Stronger poison resistance
            SetResistance(ResistanceType.Energy, 60, 80); // Stronger energy resistance

            SetSkill(SkillName.Anatomy, 50.0, 75.0);
            SetSkill(SkillName.EvalInt, 120.0, 150.0); // Increased magical skills
            SetSkill(SkillName.Magery, 120.0, 150.0); // Higher magery skill for more powerful spells
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0, 200.0); // Higher resist skill to make it tougher to affect with spells
            SetSkill(SkillName.Tactics, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 100.0, 120.0);

            Fame = 30000; // Higher fame
            Karma = -30000; // Higher karma for an evil boss

            VirtualArmor = 100; // Increased armor

            Tamable = false; // Make it untameable as a boss

            // Attach the random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void GenerateLoot()
        {
            // Base loot generation from the original Chimereon
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
            // Additional boss logic could go here, such as more frequent or stronger abilities
        }

        public ChimereonBoss(Serial serial) : base(serial)
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
