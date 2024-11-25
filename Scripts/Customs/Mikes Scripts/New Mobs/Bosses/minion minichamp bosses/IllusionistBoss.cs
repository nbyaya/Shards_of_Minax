using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("corpse of the master illusionist")]
    public class IllusionistBoss : Illusionist
    {
        [Constructable]
        public IllusionistBoss() : base()
        {
            Name = "Master Illusionist";
            Title = "the Grand Deceiver";

            // Update stats to match or exceed the boss tier
            SetStr(150, 250); // Increased strength
            SetDex(120, 170); // Increased dexterity
            SetInt(250, 350); // Increased intelligence

            SetHits(12000); // Higher health than regular illusionist

            SetDamage(15, 25); // Increased damage range

            SetDamageType(ResistanceType.Physical, 25);
            SetDamageType(ResistanceType.Energy, 75);

            // Improved resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 70, 80);

            // Enhanced skills
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 100.0, 120.0);
            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Wrestling, 70.0, 90.0);

            Fame = 18000; // Increased fame for a boss-tier creature
            Karma = -18000; // Negative karma for a villainous boss

            VirtualArmor = 50; // Increased virtual armor

            // Attach a random ability to the boss
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

            // Optional: Additional loot or custom sayings for the boss
            int phrase = Utility.Random(2);
            switch (phrase)
            {
                case 0: this.Say(true, "The illusion is now complete!"); break;
                case 1: this.Say(true, "You cannot escape the truth of illusion!"); break;
            }
        }

        public IllusionistBoss(Serial serial) : base(serial)
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
