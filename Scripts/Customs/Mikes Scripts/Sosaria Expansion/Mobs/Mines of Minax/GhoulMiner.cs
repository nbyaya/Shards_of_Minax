using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a ghoul miner corpse")]
    public class GhoulMiner : BaseCreature
    {
        [Constructable]
        public GhoulMiner()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a ghoul miner";
            Body = 3; // Unique hue or skeletal miner look
            Hue = 0x835; // Ghastly bluish-green tint

            SetStr(90, 110);
            SetDex(50, 70);
            SetInt(30, 45);

            SetHits(120, 150);

            SetDamage(10, 14);

            SetDamageType(ResistanceType.Physical, 80);
            SetDamageType(ResistanceType.Cold, 20);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 10, 20);

            SetSkill(SkillName.Tactics, 70.0, 90.0);
            SetSkill(SkillName.MagicResist, 50.0, 65.0);
            SetSkill(SkillName.Wrestling, 75.0, 90.0);

            Fame = 1500;
            Karma = -1500;

            VirtualArmor = 30;

            PackItem(new IronOre(Utility.RandomMinMax(1, 3)));

            if (Utility.RandomDouble() < 0.005) // 0.5% chance
                PackItem(new BrothersMiningBand());
        }

        public override bool BleedImmune => true;

        public override Poison PoisonImmune => Poison.Regular;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
        }

        public GhoulMiner(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
