using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Mobiles;
using Server.Prompts;

namespace Server.Items
{
    public class GenderChangeDeed : Item
    {
        [Constructable]
        public GenderChangeDeed() : base(0x14F0) // Using the default deed graphic
        {
            Name = "a gender change deed";
            Weight = 1.0;
        }

        public GenderChangeDeed(Serial serial) : base(serial)
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

        public override void OnDoubleClick(Mobile from)
        {
            if (!IsChildOf(from.Backpack))
            {
                from.SendMessage("The deed must be in your backpack.");
                return;
            }

            if (from is PlayerMobile)
            {
                from.SendMessage("Are you sure you want to change your gender? Type 'yes' to confirm.");
                from.Prompt = new ConfirmGenderChangePrompt(this);
            }
            else
            {
                from.SendMessage("This can only be used by players.");
            }
        }

        private class ConfirmGenderChangePrompt : Prompt
        {
            private readonly GenderChangeDeed m_Deed;

            public ConfirmGenderChangePrompt(GenderChangeDeed deed)
            {
                m_Deed = deed;
            }

            public override void OnResponse(Mobile from, string text)
            {
                if (text.ToLower() == "yes")
                {
                    from.BodyValue = (from.BodyValue == 400 ? 401 : 400); // 400 is male, 401 is female
                    from.SendMessage("Your gender has been changed.");
                    m_Deed.Delete();
                }
                else
                {
                    from.SendMessage("You choose not to change your gender.");
                }
            }
        }
    }
}
