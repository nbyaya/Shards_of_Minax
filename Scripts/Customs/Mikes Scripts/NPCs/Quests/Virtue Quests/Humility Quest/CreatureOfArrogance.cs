using System;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    public class CreatureOfArrogance : BaseCreature
    {
        [Constructable]
        public CreatureOfArrogance() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Creature of Arrogance";
            Body = 1; // Example body ID
            Hue = 0x455; // Optional unique color
            BaseSoundID = 0x165;

            SetStr(100);
            SetDex(60);
            SetInt(30);

            SetDamage(10, 15);

            SetSkill(SkillName.MagicResist, 40.0);
            SetSkill(SkillName.Tactics, 50.0);
            SetSkill(SkillName.Wrestling, 50.0);

            Fame = 1000;
            Karma = -1000;

            VirtualArmor = 35;

            PackItem(new HumbleOffering());
        }

        public CreatureOfArrogance(Serial serial) : base(serial)
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
