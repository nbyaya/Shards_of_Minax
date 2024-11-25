using System;
using Server.Items;
using Server.Mobiles;
using Server.Engines.XmlSpawner2;

namespace Server.Mobiles
{
    [CorpseName("a boss cheese golem corpse")]
    public class BossCheeseGolem : CheeseGolem
    {
        [Constructable]
        public BossCheeseGolem()
            : base()
        {
            Name = "Cheese Golem";
            Title = "the Colossus";
            Hue = 1956; // Unique boss hue (just slightly different from original)
            Body = 752; // Retain the same body, customize if needed

            SetStr(1200, 1500); // Increased strength, as per boss-tier stats
            SetDex(255, 300); // Increased dexterity
            SetInt(250, 350); // Increased intelligence
            
            SetHits(15000); // Boss-level health
            SetDamage(40, 55); // Boss-level damage range

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 25);
            SetDamageType(ResistanceType.Energy, 25);

            SetResistance(ResistanceType.Physical, 70, 85); // Enhanced resistances
            SetResistance(ResistanceType.Fire, 70, 90);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 75, 90);
            SetResistance(ResistanceType.Energy, 50, 70);

            SetSkill(SkillName.Anatomy, 50.0, 75.0); // Improved skill levels
            SetSkill(SkillName.EvalInt, 120.0, 150.0);
            SetSkill(SkillName.Magery, 120.0, 150.0);
            SetSkill(SkillName.Meditation, 60.0, 100.0);
            SetSkill(SkillName.MagicResist, 150.0, 175.0);
            SetSkill(SkillName.Tactics, 110.0, 120.0);
            SetSkill(SkillName.Wrestling, 110.0, 130.0);

            Fame = 30000; // Increased fame
            Karma = -30000; // Boss-tier karma

            VirtualArmor = 100; // Increased virtual armor

            Tamable = false; // Bosses typically are not tamable
            ControlSlots = 0; // Not tamable, no control slots

            PackItem(new BossTreasureBox());
            XmlAttach.AttachTo(this, new XmlRandomAbility());

            // Enhanced loot drop
            for (int i = 0; i < 5; i++)
            {
                PackItem(new MaxxiaScroll()); // Drops 5 MaxxiaScrolls
            }
        }

        public override void GenerateLoot()
        {
            base.GenerateLoot(); // Retains original loot
            this.Say("The power of the earth will crush you!");
            PackGold(2000, 3000); // Increased gold drop for boss
            PackItem(new IronIngot(Utility.RandomMinMax(100, 200))); // More iron ingots
            AddLoot(LootPack.FilthyRich, 3); // Additional rich loot
            AddLoot(LootPack.Gems, 10); // More gems
        }

        public override void OnThink()
        {
            base.OnThink(); // Retain original abilities
        }

        public BossCheeseGolem(Serial serial) : base(serial)
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
