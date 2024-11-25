using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a boss bearded goat corpse")]
    public class BossBeardedGoat : BeardedGoat
    {
        [Constructable]
        public BossBeardedGoat()
        {
            Name = "Bearded Goat";
            Title = "the Colossus";
            Hue = 1175; // Unique hue for a boss (purple hue)
            Body = 0xD1; // Default goat body

            SetStr(1200, 1500); // Enhanced strength for a boss
            SetDex(255, 300); // Enhanced dexterity for a boss
            SetInt(250, 300); // Enhanced intelligence for a boss

            SetHits(12000, 15000); // High boss health
            SetDamage(50, 60); // Increased damage

            SetResistance(ResistanceType.Physical, 80, 90); // Enhanced resistances
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 60, 75);

            SetSkill(SkillName.MagicResist, 150.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Meditation, 100.0);
            SetSkill(SkillName.Magery, 120.0); // Higher magery skill for a boss

            Fame = 30000; // Increased fame
            Karma = -30000; // Negative karma for the boss

            VirtualArmor = 100; // Boss-level armor

            Tamable = false; // Not tamable
            ControlSlots = 0; // Cannot be controlled

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Add extra drops when defeated
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Add 5 MaxxiaScroll items
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Include the base loot

            this.Say("You cannot defeat me!");
            PackGold(2000, 3000); // Increased gold drops for the boss
            PackItem(new IronIngot(Utility.RandomMinMax(100, 200))); // Increased ingot drops
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain the original behavior
        }

        public BossBeardedGoat(Serial serial)
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
