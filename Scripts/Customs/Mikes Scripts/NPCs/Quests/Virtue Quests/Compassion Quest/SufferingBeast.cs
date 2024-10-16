using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    public class SufferingBeast : BaseCreature
    {
        [Constructable]
        public SufferingBeast() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Suffering Beast";
            Body = 233; // Example body ID
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
            PackItem(new HealingHerb());
        }

        public SufferingBeast(Serial serial) : base(serial)
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
