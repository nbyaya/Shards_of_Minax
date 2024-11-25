using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a purse spider corpse")]
    public class BossPurseSpider : PurseSpider
    {
        [Constructable]
        public BossPurseSpider()
        {
            Name = "Purse Spider";
            Title = "the Matriarch";
            Hue = 1787; // Same hue as original, but can be changed for uniqueness
            Body = 28; // Same as original Purse Spider

            SetStr(1500, 2000); // Increased strength for boss level
            SetDex(255, 300); // Increased dexterity
            SetInt(300, 400); // Higher intelligence

            SetHits(15000, 20000); // Boss-level health

            SetDamage(40, 50); // Increased damage

            SetDamageType(ResistanceType.Physical, 60); // More physical damage
            SetDamageType(ResistanceType.Fire, 30); // More fire damage
            SetDamageType(ResistanceType.Energy, 30); // More energy damage

            SetResistance(ResistanceType.Physical, 80, 90); // Higher physical resistance
            SetResistance(ResistanceType.Fire, 80, 90); // Higher fire resistance
            SetResistance(ResistanceType.Cold, 60, 70); // More cold resistance
            SetResistance(ResistanceType.Poison, 80, 90); // Higher poison resistance
            SetResistance(ResistanceType.Energy, 50, 60); // Higher energy resistance

            SetSkill(SkillName.Anatomy, 100.0, 120.0); // Enhanced anatomy skill
            SetSkill(SkillName.EvalInt, 120.0, 140.0); // Enhanced EvalInt skill
            SetSkill(SkillName.Magery, 110.0, 130.0); // Enhanced Magery skill
            SetSkill(SkillName.Meditation, 100.0, 120.0); // Enhanced Meditation skill
            SetSkill(SkillName.MagicResist, 150.0, 180.0); // Higher magic resist
            SetSkill(SkillName.Tactics, 100.0, 120.0); // Enhanced Tactics skill
            SetSkill(SkillName.Wrestling, 100.0, 120.0); // Enhanced Wrestling skill

            Fame = 35000; // Increased fame
            Karma = -35000; // Increased negative karma

            VirtualArmor = 100; // Increased virtual armor

            Tamable = false; // Cannot be tamed
            ControlSlots = 0; // Not tamable

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Additional loot on defeat
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Adding 5 MaxxiaScrolls
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Include the base loot generation
            this.Say("You dare challenge the Matriarch?");
            PackGold(1500, 2000); // Enhanced gold drop
            PackItem(new IronIngot(Utility.RandomMinMax(100, 200))); // More ingots for a boss
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain base behavior for special abilities
        }

        public BossPurseSpider(Serial serial) : base(serial)
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
