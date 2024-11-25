using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a boss meat golem corpse")]
    public class BossMeatGolem : MeatGolem
    {
        [Constructable]
        public BossMeatGolem()
        {
            Name = "Boss Meat Golem";
            Title = "the Corrupted Flesh";
            Hue = 0x497; // Unique hue for the boss
            Body = 752; // Golem body

            SetStr(1200, 1500); // Enhanced strength
            SetDex(255, 350); // Increased dexterity
            SetInt(250, 350); // Increased intelligence

            SetHits(15000); // Increased health for the boss

            SetDamage(40, 55); // Increased damage for the boss

            SetDamageType(ResistanceType.Physical, 60);
            SetDamageType(ResistanceType.Fire, 20);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 80, 95); // Stronger resistances
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, 70, 85);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 60, 75);

            SetSkill(SkillName.Anatomy, 50.0, 75.0);
            SetSkill(SkillName.EvalInt, 100.0, 120.0);
            SetSkill(SkillName.Magery, 100.0, 120.0);
            SetSkill(SkillName.Meditation, 50.0, 75.0);
            SetSkill(SkillName.MagicResist, 150.0, 180.0); // Higher magic resist
            SetSkill(SkillName.Tactics, 120.0, 130.0);
            SetSkill(SkillName.Wrestling, 120.0, 130.0);

            Fame = 30000; // Increased fame
            Karma = -30000; // Negative karma for the boss

            VirtualArmor = 100; // Increased virtual armor

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Additional loot on defeat
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Drop 5 MaxxiaScrolls
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Include base loot
            this.Say("I am the Corrupted Flesh!");
            PackGold(2000, 3000); // Enhanced gold drops
            PackItem(new IronIngot(Utility.RandomMinMax(100, 200))); // More ingots for a boss
            this.AddLoot(LootPack.FilthyRich, 3); // Enhanced loot pack
            this.AddLoot(LootPack.Gems, 10); // Added gems
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain base behavior for abilities
        }

        public BossMeatGolem(Serial serial) : base(serial)
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
