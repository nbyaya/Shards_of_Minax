using System;
using Server;
using Server.Items;
using Server.Network;
using Server.Targeting;

namespace Server.Items
{
    public class TrailJournal : Item
    {
        [Constructable]
        public TrailJournal() : base(0xFF4) // Adjust the item ID based on your desired graphic
        {
            Weight = 1.0;
            Name = "Trail Journal";
        }

        public TrailJournal(Serial serial) : base(serial)
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
                from.SendLocalizedMessage(1042001); // That must be in your pack for you to use it.
                return;
            }

            if (from.Skills.Camping.Value < 50)
            {
                from.SendMessage("You must have at least 50 in camping skill to use this.");
                return;
            }

            from.BeginTarget(-1, false, TargetFlags.None, new TargetCallback(OnTarget));
            from.SendMessage("Which rune do you wish to use this on?");
        }

        private void OnTarget(Mobile from, object targeted)
        {
            if (targeted is RecallRune)
            {
                RecallRune rune = (RecallRune)targeted;

                if (!rune.IsChildOf(from.Backpack))
                {
                    from.SendMessage("The rune must be in your backpack.");
                    return;
                }

                if (rune.Marked)
                {
                    from.Location = rune.Target;
                    from.Map = rune.TargetMap;
                    Delete(); // Destroy the journal after use
                }
                else
                {
                    rune.Mark(from);
                    rune.Marked = true;
                    rune.Target = from.Location;
                    rune.TargetMap = from.Map;
                    from.SendMessage("The rune has been marked with your current location.");
                    Delete(); // Destroy the journal after use
                }
            }
            else
            {
                from.SendMessage("That is not a valid target. You must target a blank or marked recall rune.");
            }
        }
    }
}
