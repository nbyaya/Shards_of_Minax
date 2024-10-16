using System;
using Server;
using Server.Items;
using Server.Network;

public class WallFlowers : Item
{
    private static int[] Graphics = new int[]
    {
        0xABEB, 0xABEC, 0xABED, 0xABEE
    };

    [Constructable]
    public WallFlowers() : base(Graphics[Utility.Random(Graphics.Length)])
    {
        Name = "Wall Flowers";
        Hue = Utility.Random(1, 3000);

        // Setting the weight to be consistent with other items
        Weight = 1.0;
    }

    public override void OnDoubleClick(Mobile from)
    {
        if (!from.InRange(GetWorldLocation(), 1))
        {
            from.SendLocalizedMessage(500446); // That is too far away.
        }
        else
        {
            from.SendMessage("You find the art wonderful!");
            from.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
            from.PlaySound(0x1E0); // Coin sound
        }
    }

    public WallFlowers(Serial serial) : base(serial)
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
}
