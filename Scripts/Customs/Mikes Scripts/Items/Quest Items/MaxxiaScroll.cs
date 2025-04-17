using System;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Items
{
    public class MaxxiaScroll : Item
    {
        [Constructable]
        public MaxxiaScroll() : this(1)
        {
        }

        [Constructable]
        public MaxxiaScroll(int amount) : base(0xE34)  // 0xE34 = Scroll
        {
            Stackable = true;
            Amount = amount;
            Name = "Maxxia Scroll";
            Hue = 1150;  // Scroll color
        }

        public MaxxiaScroll(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from is PlayerMobile player)
            {
                // Ensure the player is alive and can see the item
                if (!IsChildOf(player.Backpack))
                {
                    player.SendMessage("This must be in your backpack to use.");
                    return;
                }

                // Award one Talent Point
                var profile = player.AcquireTalents();
                if (!profile.Talents.TryGetValue(TalentID.AncientKnowledge, out var talent))
                {
                    talent = new Talent(TalentID.AncientKnowledge);
                    profile.Talents[TalentID.AncientKnowledge] = talent;
                }
                talent.Points++;

                // Inform the player
                player.SendMessage("You have consumed a Maxxia Scroll and gained a Talent Point!");

                // Consume one scroll
                if (Amount > 1)
                {
                    Amount--; // Reduce stack by one
                }
                else
                {
                    Delete(); // Remove the item if it's the last one
                }
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
