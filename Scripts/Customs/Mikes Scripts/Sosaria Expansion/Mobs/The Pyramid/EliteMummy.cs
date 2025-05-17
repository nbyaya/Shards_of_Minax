using System;
using Server;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("an elite mummy corpse")]
    public class EliteMummy : BaseCreature
    {
        [Constructable]
        public EliteMummy()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.4, 0.8)
        {
            Name = "an Elite Mummy";
            Body = 154;
            Hue = 2407; // Ash-grey hue
            BaseSoundID = 471;

            SetStr(420, 450);
            SetDex(80, 100);
            SetInt(60, 75);

            SetHits(300, 340);
            SetDamage(18, 28);

            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Cold, 60);

            SetResistance(ResistanceType.Physical, 55, 65);
            SetResistance(ResistanceType.Fire, 15, 25);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 25, 35);

            SetSkill(SkillName.MagicResist, 50.0);
            SetSkill(SkillName.Tactics, 60.0);
            SetSkill(SkillName.Wrestling, 65.0);

            Fame = 6000;
            Karma = -6000;

            VirtualArmor = 70;

            // Custom drop
            if (Utility.RandomDouble() < 0.75)
            {
                PackItem(new MummifiedHeart());
            }
        }

        public EliteMummy(Serial serial) : base(serial) { }

        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Regular;

        public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems);
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }
}
