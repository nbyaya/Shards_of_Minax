using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    public class DreadfulBeast : BaseCreature
    {
        [Constructable]
        public DreadfulBeast() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Dreadful Beast";
            Body = 309; // Example body ID
            Hue = Utility.RandomAnimalHue();
            BaseSoundID = 0xA8;

            SetStr(60);
            SetDex(60);
            SetInt(30);

            SetDamage(6, 12);

            SetSkill(SkillName.MagicResist, 30.0);
            SetSkill(SkillName.Tactics, 50.0);
            SetSkill(SkillName.Wrestling, 50.0);

            Fame = 600;
            Karma = -600;

            VirtualArmor = 30;

            // Optionally, add loot
            PackItem(new ValorItem());
        }

        public DreadfulBeast(Serial serial) : base(serial)
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
