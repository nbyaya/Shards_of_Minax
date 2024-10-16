using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class ArtisanalCheeses : Food
    {
        [Constructable]
        public ArtisanalCheeses() : base(0x15FD)
        {
            Weight = 1.0;
            FillFactor = 3;
            Name = "Artisanal Cheeses";
			Hue = 2324;
        }

        public ArtisanalCheeses(Serial serial) : base(serial)
        {
        }

        public override bool Eat(Mobile from)
        {
            if (from.Skills[SkillName.TasteID].Base < 60.0)
            {
                from.SendMessage("You need at least 60 Taste Identification skill to enjoy this.");
                return false;
            }

            int bonusValue = 10 + (int)((from.Skills[SkillName.TasteID].Value - 60) / 4);
            from.AddSkillMod(new TimedSkillMod(SkillName.Cooking, true, bonusValue, TimeSpan.FromMinutes(25)));
            from.SendMessage("You feel your Cooking skill improving!");

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
