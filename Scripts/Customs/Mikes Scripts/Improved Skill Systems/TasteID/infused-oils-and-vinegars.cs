using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class InfusedOilsAndVinegars : Food
    {
        [Constructable]
        public InfusedOilsAndVinegars() : base(0x15FC)
        {
            Weight = 1.0;
            FillFactor = 1;
            Name = "Infused Oils and Vinegars";
			Hue = 2667;
        }

        public InfusedOilsAndVinegars(Serial serial) : base(serial)
        {
        }

        public override bool Eat(Mobile from)
        {
            if (from.Skills[SkillName.TasteID].Base < 75.0)
            {
                from.SendMessage("You need at least 75 Taste Identification skill to enjoy this.");
                return false;
            }

            int bonusValue = 20 + (int)((from.Skills[SkillName.TasteID].Value - 75) / 2.5);
            from.AddStatMod(new StatMod(StatType.Dex, "InfusedOilsAndVinegars", bonusValue, TimeSpan.FromMinutes(35)));
            from.SendMessage("You feel your dexterity increasing!");

            return base.Eat(from);
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
