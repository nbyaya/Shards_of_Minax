using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class TruffleRisotto : Food
    {
        [Constructable]
        public TruffleRisotto() : base(0x15FA)
        {
            Weight = 1.0;
            FillFactor = 5;
            Name = "Truffle Risotto";
			Hue = 2714;
        }

        public TruffleRisotto(Serial serial) : base(serial)
        {
        }

        public override bool Eat(Mobile from)
        {
            if (from.Skills[SkillName.TasteID].Base < 85.0)
            {
                from.SendMessage("You need at least 85 Taste Identification skill to enjoy this.");
                return false;
            }

            int bonusValue = 15 + (int)((from.Skills[SkillName.TasteID].Value - 85) / 3);
            from.AddSkillMod(new TimedSkillMod(SkillName.Cooking, true, bonusValue, TimeSpan.FromMinutes(45)));
            from.AddSkillMod(new TimedSkillMod(SkillName.TasteID, true, bonusValue, TimeSpan.FromMinutes(45)));
            from.SendMessage("You feel your Cooking and Taste Identification skills improving!");

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
