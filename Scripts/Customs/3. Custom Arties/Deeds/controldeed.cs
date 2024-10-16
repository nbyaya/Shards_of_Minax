using System;
using Server.Network;
using Server.Prompts;
using Server.Mobiles;
using Server.Items;

namespace Server.Items
{
    public class AnimalControlDeed : Item
    {
        [Constructable]
        public AnimalControlDeed() : base(0x14F0)
        {
            Name = "a +1 animal control slot deed";
            Hue = 0x480;
            LootType = LootType.Blessed;
            Weight = 1.0;
        }

        public AnimalControlDeed(Serial serial) : base(serial) { }

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
                from.SendMessage("The deed must be in your backpack to use.");
                return;
            }

            PlayerMobile player = from as PlayerMobile;
            if (player != null)
            {
                if (player.FollowersMax < 10) // Assuming 10 is the max, adjust as needed
                {
                    player.FollowersMax += 1;
                    from.SendMessage("You have gained +1 animal control slot.");
                    this.Delete();
                }
                else
                {
                    from.SendMessage("You've already reached the maximum animal control slots.");
                }
            }
        }
    }
}
