using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    public class FierceDragon : BaseCreature
    {
        [Constructable]
        public FierceDragon() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Fierce Dragon";
            Body = 59; // Dragon body
            Hue = Utility.RandomAnimalHue();
            BaseSoundID = 0xA8;

            SetStr(500);
            SetDex(150);
            SetInt(250);

            SetDamage(20, 30);

            SetSkill(SkillName.MagicResist, 100.0);
            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);

            Fame = 2000;
            Karma = -2000;

            VirtualArmor = 50;

            // Optionally, add loot
            PackItem(new AncientArtifact());
        }

        public FierceDragon(Serial serial) : base(serial)
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
