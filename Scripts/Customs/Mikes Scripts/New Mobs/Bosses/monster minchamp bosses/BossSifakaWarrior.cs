using System;
using Server;
using Server.Mobiles;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a boss sifaka warrior corpse")]
    public class BossSifakaWarrior : SifakaWarrior
    {
        [Constructable]
        public BossSifakaWarrior()
        {
            Name = "Boss Sifaka Warrior";
            Title = "the Mighty";
            Hue = 1965; // Unique hue for the boss
            Body = 0x1D; // Gorilla body, same as the original

            SetStr(1200, 1500); // Increased strength
            SetDex(255, 300); // Enhanced dexterity
            SetInt(250, 350); // Higher intelligence

            SetHits(12000); // Boss-level health
            SetDamage(40, 50); // Increased damage

            SetResistance(ResistanceType.Physical, 80, 90); // Increased resistances
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 150.0); // Enhanced magic resist
            SetSkill(SkillName.Tactics, 120.0, 140.0); // Increased tactics skill
            SetSkill(SkillName.Wrestling, 120.0, 140.0); // Enhanced wrestling
            SetSkill(SkillName.EvalInt, 100.0); // Better evaluation for magic
            SetSkill(SkillName.Magery, 100.0); // Increased magery for variety in abilities

            Fame = 24000; // Boss-level fame
            Karma = -24000;

            VirtualArmor = 90; // Standard virtual armor, can be adjusted

            Tamable = false; // Cannot be tamed

            // Attach the random ability XML attachment
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Add the special loot (5 MaxxiaScrolls)
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Assuming MaxxiaScroll is a defined item
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Include base loot
            this.Say("You cannot escape my wrath!");
            PackGold(1000, 1500); // Enhanced gold drop
            PackItem(new IronIngot(Utility.RandomMinMax(50, 100))); // Increased ingot drop
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain base behavior for abilities like leap, camouflage, etc.
        }

        public BossSifakaWarrior(Serial serial) : base(serial)
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
