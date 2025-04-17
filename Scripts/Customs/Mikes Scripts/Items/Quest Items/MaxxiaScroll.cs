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
				if (!IsChildOf(player.Backpack))
				{
					player.SendMessage("This must be in your backpack to use.");
					return;
				}

				int consumed = this.Amount;

				if (consumed <= 0)
				{
					player.SendMessage("There are no scrolls to consume.");
					return;
				}

				// Award one Talent Point per scroll
				var profile = player.AcquireTalents();
				if (!profile.Talents.TryGetValue(TalentID.AncientKnowledge, out var talent))
				{
					talent = new Talent(TalentID.AncientKnowledge);
					profile.Talents[TalentID.AncientKnowledge] = talent;
				}

				talent.Points += consumed;

				player.SendMessage($"You have consumed {consumed} Maxxia Scroll{(consumed > 1 ? "s" : "")} and gained {consumed} Talent Point{(consumed > 1 ? "s" : "")}!");

				// Delete the entire stack
				Delete();
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
