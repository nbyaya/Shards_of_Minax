using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Network;

namespace Server.Items
{
    public class BanishingRod : Item
    {
        [Constructable]
        public BanishingRod() : base(0xE81)
        {
            Name = "Banishing Rod";
            Weight = 2.0;
        }

        public BanishingRod(Serial serial) : base(serial)
        {
        }

        public override void OnDoubleClick(Mobile from)
        {
            if (from == null || !(from is PlayerMobile)) return;

            PlayerMobile player = (PlayerMobile)from;

            if (player.Followers <= 0)
            {
                player.SendMessage("You have no pets to banish.");
                return;
            }

            // Create a temporary list to hold pets that need to be released.
            List<BaseCreature> petsToRelease = new List<BaseCreature>();

            foreach (Mobile m in player.AllFollowers)
            {
                if (m != null && m is BaseCreature)
                {
                    BaseCreature pet = (BaseCreature)m;
                    if (pet.ControlMaster == player)
                    {
                        petsToRelease.Add(pet);
                    }
                }
            }

            // Loop over the temporary list to release the pets.
            int releasedCount = 0;
            foreach (BaseCreature pet in petsToRelease)
            {
                pet.ControlTarget = null;
                pet.ControlOrder = OrderType.Release;
                pet.Internalize();
                pet.SetControlMaster(null);
                pet.SummonMaster = null;
                pet.Delete();

                releasedCount++;
            }

            if (releasedCount > 0)
            {
                player.SendMessage("All your pets have been banished.");
            }
            else
            {
                player.SendMessage("Failed to banish pets.");
            }

            // Do not delete this item as it's unlimited use.
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
