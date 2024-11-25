using System;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the Florida Overlord")]
    public class FloridaManBoss : FloridaMan
    {
        [Constructable]
        public FloridaManBoss() : base()
        {
            Name = "Florida Overlord";
            Title = "the Supreme Swamp King";

            // Enhanced Stats
            SetStr(1200); // Boosted strength
            SetDex(255);  // Maximum dexterity
            SetInt(250);  // Increased intelligence

            SetHits(12000); // Increased health

            SetDamage(29, 45); // Increased damage range

            SetResistance(ResistanceType.Physical, 80); // Increased resistances
            SetResistance(ResistanceType.Fire, 80);
            SetResistance(ResistanceType.Cold, 70);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 60);

            SetSkill(SkillName.Magery, 100.0); // Increased skill levels
            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 22500;  // Increased fame
            Karma = -22500; // Increased karma

            VirtualArmor = 75;  // Increased virtual armor

            // Attach a random ability
            XmlAttach.AttachTo(this, new XmlRandomAbility());
        }

        public override void OnThink()
        {
            base.OnThink();

            // Additional boss logic could be added here, such as using more advanced spells, etc.
        }

        // Override loot generation to include 5 MaxxiaScrolls
        public override void GenerateLoot()
        {
            base.GenerateLoot();

            // Add 5 MaxxiaScrolls
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public FloridaManBoss(Serial serial) : base(serial)
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
