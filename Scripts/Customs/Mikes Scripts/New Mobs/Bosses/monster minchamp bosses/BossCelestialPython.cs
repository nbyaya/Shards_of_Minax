using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a celestial python corpse")]
    public class BossCelestialPython : CelestialPython
    {
        [Constructable]
        public BossCelestialPython() : base()
        {
            Name = "Boss Celestial Python";
            Title = "the Cosmic Serpent";
            Hue = 1778; // Keep celestial blue hue for visual consistency
            Body = 0x15; // Giant Serpent body

            SetStr(1200, 1500); // Boss-level strength
            SetDex(255, 300);   // Boss-level dexterity
            SetInt(250, 350);   // Boss-level intelligence

            SetHits(12000); // Boss-level health
            SetDamage(35, 50); // Increased damage

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 30);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 75, 85);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 75, 85);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.Anatomy, 80.0, 120.0);
            SetSkill(SkillName.EvalInt, 120.0, 150.0);
            SetSkill(SkillName.Magery, 110.0, 150.0);
            SetSkill(SkillName.Meditation, 70.0, 100.0);
            SetSkill(SkillName.MagicResist, 150.0, 180.0);
            SetSkill(SkillName.Tactics, 120.0, 150.0);
            SetSkill(SkillName.Wrestling, 120.0, 150.0);

            Fame = 30000; // Higher fame to signify a boss
            Karma = -30000;

            VirtualArmor = 100; // Increased armor for the boss-level tankiness

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Additional loot on defeat
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Assuming MaxxiaScroll is a defined item
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Include base loot
            this.Say("You dare challenge the cosmic might of the Python?");
            PackGold(1500, 2000); // Enhanced gold drops
            PackItem(new IronIngot(Utility.RandomMinMax(100, 150))); // More ingots for a boss
            PackItem(new GoldIngot()); // Example of additional rare item
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain base behavior for using abilities like constriction and projection
        }

        public BossCelestialPython(Serial serial) : base(serial)
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
