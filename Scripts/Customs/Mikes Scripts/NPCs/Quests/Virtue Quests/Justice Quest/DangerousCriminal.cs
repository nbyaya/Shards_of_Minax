using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    public class DangerousCriminal : BaseCreature
    {
        [Constructable]
        public DangerousCriminal() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Dangerous Criminal";
            Body = 400; // Human Male
            Hue = Utility.RandomSkinHue();
            BaseSoundID = 0x45A;

            SetStr(75);
            SetDex(75);
            SetInt(50);

            SetDamage(10, 15);

            SetSkill(SkillName.MagicResist, 50.0);
            SetSkill(SkillName.Tactics, 60.0);
            SetSkill(SkillName.Wrestling, 60.0);

            Fame = 1000;
            Karma = -1000;

            VirtualArmor = 30;
			
            AddItem(new FancyShirt(Utility.RandomNeutralHue()));
            AddItem(new LongPants(Utility.RandomNeutralHue()));
            AddItem(new Boots(Utility.RandomNeutralHue()));

            // Optionally, add loot
            PackItem(new StolenGoods());
        }

        public DangerousCriminal(Serial serial) : base(serial)
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
