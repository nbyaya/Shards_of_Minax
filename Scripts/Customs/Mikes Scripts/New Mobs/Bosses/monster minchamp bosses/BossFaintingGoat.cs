using System;
using Server;
using Server.Mobiles;
using Server.Network;
using Server.Items;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a boss fainting goat corpse")]
    public class BossFaintingGoat : FaintingGoat
    {
        [Constructable]
        public BossFaintingGoat()
            : base()
        {
            Name = "Fainting Goat";
            Title = "the Terrifying";
            Hue = 0x497; // Unique hue for the boss version
            Body = 0xD1; // Goat body

            SetStr(1200, 1500); // Enhanced strength
            SetDex(255, 300); // Enhanced dexterity
            SetInt(250, 350); // Enhanced intelligence

            SetHits(15000); // Boss-level health
            SetDamage(45, 55); // Enhanced damage

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 30);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 80, 90); // Enhanced resistances
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 60, 70);

            SetSkill(SkillName.MagicResist, 150.0); // Enhanced magic resistance
            SetSkill(SkillName.Tactics, 100.0, 120.0); // Enhanced tactics
            SetSkill(SkillName.Wrestling, 100.0, 120.0); // Enhanced wrestling skill
            SetSkill(SkillName.EvalInt, 100.0, 120.0); // Enhanced EvalInt for magical abilities
            SetSkill(SkillName.Magery, 100.0, 120.0); // Enhanced Magery for spells

            Fame = 30000; // Enhanced fame
            Karma = -30000; // Negative karma for boss

            VirtualArmor = 120; // Increased armor for the boss

            Tamable = false; // Boss is untamable
            ControlSlots = 0; // No control slots for this boss

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Drop 5 MaxxiaScrolls upon death
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Include base loot
            this.Say("You dare challenge me, mortal?");
            PackGold(1500, 2000); // Enhanced gold drop
            PackItem(new IronIngot(Utility.RandomMinMax(100, 150))); // Enhanced ingot drop
            AddLoot(LootPack.Rich, 3); // Increased loot count for riches
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain base behavior for abilities

            // Custom abilities for the boss go here (same as original)
        }

        public BossFaintingGoat(Serial serial)
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
