using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a frozen corpse")]
    public class GlacialWraith : BaseCreature
    {
        [Constructable]
        public GlacialWraith()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a glacial wraith";
            Body = 26;
            Hue = 0x47F; // Frosty icy tone
            BaseSoundID = 0x482;

            SetStr(90, 110);
            SetDex(80, 100);
            SetInt(60, 85);

            SetHits(80, 100);
            SetDamage(10, 15);

            SetDamageType(ResistanceType.Cold, 75);
            SetDamageType(ResistanceType.Physical, 25);

            SetResistance(ResistanceType.Cold, 40, 60);
            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 0, 0);
            SetResistance(ResistanceType.Poison, 20, 30);

            SetSkill(SkillName.Magery, 70.0, 85.0);
            SetSkill(SkillName.EvalInt, 70.0, 85.0);
            SetSkill(SkillName.MagicResist, 65.0, 80.0);
            SetSkill(SkillName.Tactics, 60.0, 75.0);
            SetSkill(SkillName.Wrestling, 55.0, 70.0);

            Fame = 5000;
            Karma = -5000;
            VirtualArmor = 40;

            PackItem(new FrostveinCrystal());

            if (Utility.RandomDouble() < 0.005) // 1 in 200 chance
                PackItem(new IceboundAmulet()); // Optional rare drop
        }

        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Deadly;

        public GlacialWraith(Serial serial) : base(serial) { }

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
