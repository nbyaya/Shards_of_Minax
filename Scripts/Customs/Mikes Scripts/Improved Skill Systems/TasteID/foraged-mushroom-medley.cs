using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class ForagedMushroomMedley : Food
    {
        [Constructable]
        public ForagedMushroomMedley() : base(0x15F9)
        {
            Weight = 1.0;
            FillFactor = 5;
            Name = "Foraged Mushroom Medley";
			Hue = 2744;
        }

        public ForagedMushroomMedley(Serial serial) : base(serial)
        {
        }

        public override bool Eat(Mobile from)
        {
            if (from.Skills[SkillName.TasteID].Base < 40.0)
            {
                from.SendMessage("You need at least 40 Taste Identification skill to enjoy this.");
                return false;
            }

            int bonusValue = 35 + (int)((from.Skills[SkillName.TasteID].Value - 85) / 1.5);
            from.AddStatMod(new StatMod(StatType.Str, "ForagedMushroomMedley", bonusValue, TimeSpan.FromMinutes(50)));
            from.SendMessage("You feel your strength increasing!");

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
