using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("corpse of a lost miner")]
    public class LostMiner : BaseCreature
    {
        [Constructable]
		public LostMiner() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.25, 0.5)
        {
            Name = "a Lost Miner";
            Title = "of Minax";
            Body = Utility.RandomBool() ? 0x190 : 0x191;
            Hue = 0x455;

            AddItem(new Robe { Hue = 1109 });
            AddItem(new Pickaxe { Name = "Echoing Pickaxe", Hue = 1154 });

            SetStr(350, 400);
            SetDex(120, 140);
            SetInt(250, 300);

            SetHits(500, 650);
            SetDamage(12, 18);

            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Cold, 30);

            SetResistance(ResistanceType.Physical, 35, 45);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 25, 35);

            SetSkill(SkillName.Magery, 90.0, 100.0);
            SetSkill(SkillName.MagicResist, 85.0, 95.0);
            SetSkill(SkillName.Tactics, 80.0, 90.0);
            SetSkill(SkillName.Wrestling, 85.0, 95.0);

            Fame = 6500;
            Karma = -6500;

            VirtualArmor = 40;
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            if (Utility.RandomDouble() < 0.4) // 40% drop chance
                PackItem(new EchoStone());
        }

        public override bool AlwaysMurderer => true;

        public LostMiner(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
