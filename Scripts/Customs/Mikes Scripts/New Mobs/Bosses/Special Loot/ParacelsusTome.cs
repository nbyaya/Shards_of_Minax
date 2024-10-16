using System;
using Server.Network;

namespace Server.Items
{
    public class ParacelsusTome : Item
    {
        [Constructable]
        public ParacelsusTome() : base(0xFBE)
        {
            Weight = 1.0;
            Hue = 0x489;
            Name = "Paracelsus' Tome";
        }

        public ParacelsusTome(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            from.SendMessage("You study the ancient alchemical knowledge within the tome.");
            from.PlaySound(0x249);

            if (from.Skills.Alchemy.Base < 100.0)
            {
                from.CheckSkill(SkillName.Alchemy, 0, 1000);
            }
            else
            {
                from.SendMessage("You already know everything this tome can teach you.");
            }
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