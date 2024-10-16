using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    public class CursedBeast : BaseCreature
    {
        [Constructable]
        public CursedBeast() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Cursed Beast";
            Body = 31; // Example body ID
            Hue = Utility.RandomAnimalHue();
            BaseSoundID = 0xA8;

            SetStr(50);
            SetDex(50);
            SetInt(25);

            SetDamage(5, 10);

            SetSkill(SkillName.MagicResist, 25.0);
            SetSkill(SkillName.Tactics, 40.0);
            SetSkill(SkillName.Wrestling, 40.0);

            Fame = 500;
            Karma = -500;

            VirtualArmor = 25;

            // Optionally, add loot
            PackItem(new CursedItem());
        }

        public CursedBeast(Serial serial) : base(serial)
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
