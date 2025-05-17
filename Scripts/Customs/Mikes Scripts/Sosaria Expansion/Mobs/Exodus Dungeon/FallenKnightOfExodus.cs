using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a cursed knight’s bones")]
    public class FallenKnightOfExodus : BaseCreature
    {
        [Constructable]
        public FallenKnightOfExodus()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Fallen Knight of Exodus";
            Body = 57;
            Hue = 2406;
            BaseSoundID = 451;

            SetStr(220, 280);
            SetDex(80, 100);
            SetInt(50, 70);

            SetHits(180, 220);

            SetDamage(10, 20);
            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Cold, 30);
            SetDamageType(ResistanceType.Energy, 40);

            SetResistance(ResistanceType.Physical, 40, 55);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 25, 35);
            SetResistance(ResistanceType.Energy, 35, 45);

            SetSkill(SkillName.MagicResist, 70.0, 90.0);
            SetSkill(SkillName.Tactics, 90.0, 100.0);
            SetSkill(SkillName.Wrestling, 90.0, 105.0);

            Fame = 4000;
            Karma = -4000;

            VirtualArmor = 50;

            AddItem(new ChainChest { Hue = 2406 });
            AddItem(new VikingSword { Hue = 2406, Name = "Spectral Blade" });
            AddItem(new HeaterShield { Hue = 2406 });

            PackItem(new CursedDust()); // Always drops one
        }

        public override bool BleedImmune => true;
        public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average, 2);
        }

        public FallenKnightOfExodus(Serial serial) : base(serial) { }

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
