using System;
using Server;
using Server.Mobiles;
using Server.Network;
using System.Collections.Generic;

namespace Server.Items
{
    public class BanishingOrb : Item
    {
        [Constructable]
        public BanishingOrb() : base(0xE2E)
        {
            Name = "Banishing Orb";
            Weight = 1.0;
        }

        public BanishingOrb(Serial serial) : base(serial)
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

    this.Delete();
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
