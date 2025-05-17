using System;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a fungal beast corpse")]
    public class FungalBeast : BaseCreature
    {
        [Constructable]
        public FungalBeast() : base(AIType.AI_Berserk, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a fungal beast";
            Body = 187;
            Hue = 1417; // Sickly green-purple
            BaseSoundID = 0x3F3;

            SetStr(120, 150);
            SetDex(60, 85);
            SetInt(40, 60);

            SetHits(160, 200);
            SetDamage(8, 12);

            SetDamageType(ResistanceType.Poison, 40);
            SetDamageType(ResistanceType.Physical, 60);

            SetResistance(ResistanceType.Physical, 30);
            SetResistance(ResistanceType.Poison, 80);
            SetResistance(ResistanceType.Fire, 10);
            SetResistance(ResistanceType.Cold, 20);
            SetResistance(ResistanceType.Energy, 10);

            SetSkill(SkillName.MagicResist, 40.0, 60.0);
            SetSkill(SkillName.Tactics, 60.0, 80.0);
            SetSkill(SkillName.Wrestling, 70.0, 85.0);

            Fame = 4500;
            Karma = -4500;

            VirtualArmor = 28;
        }

        public override void GenerateLoot()
        {
            if (Utility.RandomDouble() < 0.5) // 50% chance
                PackItem(new AmbercapSpores());
        }

        public FungalBeast(Serial serial) : base(serial) { }

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
