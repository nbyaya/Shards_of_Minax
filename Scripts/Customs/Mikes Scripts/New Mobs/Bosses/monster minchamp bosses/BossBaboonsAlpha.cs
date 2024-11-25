using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a boss baboon's alpha corpse")]
    public class BossBaboonsAlpha : BaboonsAlpha
    {
        [Constructable]
        public BossBaboonsAlpha() : base()
        {
            Name = "Baboons' Alpha";
            Title = "the Beastmaster";
            Hue = 0x497; // Unique hue for the boss
            Body = 0x1D; // Gorilla body

            SetStr(1500, 1700); // Increased strength
            SetDex(255, 300);  // Increased dexterity
            SetInt(350, 450);  // Increased intelligence

            SetHits(15000);    // Increased health for the boss
            SetDamage(50, 70); // Increased damage

            SetResistance(ResistanceType.Physical, 80, 90);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 50, 60);

            SetSkill(SkillName.MagicResist, 120.0, 150.0); // Increased resist skill
            SetSkill(SkillName.Tactics, 120.0, 130.0); // Increased tactics skill
            SetSkill(SkillName.Wrestling, 120.0, 130.0); // Increased wrestling skill
            SetSkill(SkillName.Meditation, 100.0, 120.0); // Added meditation for enhanced mana regeneration
            SetSkill(SkillName.EvalInt, 90.0, 110.0); // Enhanced magic skill

            Fame = 35000;   // Boss-level fame
            Karma = -35000; // Boss-level karma

            VirtualArmor = 120; // Enhanced armor value

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Drop 5 MaxxiaScrolls along with other loot
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Keep base loot
            this.Say("You are about to face the might of the Alpha!");
            PackGold(2000, 3000); // Enhanced gold drop
            PackItem(new IronIngot(Utility.RandomMinMax(50, 100))); // More ingots
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain base behavior for abilities and logic
        }

        public BossBaboonsAlpha(Serial serial) : base(serial)
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
