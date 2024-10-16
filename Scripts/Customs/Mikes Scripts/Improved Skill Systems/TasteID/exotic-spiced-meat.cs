using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class ExoticSpicedMeat : Food
    {
        [Constructable]
        public ExoticSpicedMeat() : base(0x15FB)
        {
            Weight = 1.0;
            FillFactor = 5;
            Name = "Exotic Spiced Meat";
			Hue = 2114;
        }

        public ExoticSpicedMeat(Serial serial) : base(serial)
        {
        }

        public override bool Eat(Mobile from)
        {
            if (from.Skills[SkillName.TasteID].Base < 80.0)
            {
                from.SendMessage("You need at least 80 Taste Identification skill to enjoy this.");
                return false;
            }

            int bonusValue = 10 + (int)((from.Skills[SkillName.TasteID].Value - 80) / 4);
            from.AddStatMod(new StatMod(StatType.Str, "ExoticSpicedMeat_Str", bonusValue, TimeSpan.FromMinutes(40)));
            from.AddResistanceMod(new ResistanceMod(ResistanceType.Poison, bonusValue));
            from.SendMessage("You feel stronger and more resistant to poison!");

            Timer.DelayCall(TimeSpan.FromMinutes(40), () => RemovePoisonResistance(from, bonusValue));

            return base.Eat(from);
        }

        private static void RemovePoisonResistance(Mobile from, int bonusValue)
        {
            from.RemoveResistanceMod(new ResistanceMod(ResistanceType.Poison, bonusValue));
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