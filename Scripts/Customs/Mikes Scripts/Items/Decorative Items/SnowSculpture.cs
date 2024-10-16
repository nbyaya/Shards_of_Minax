using System;
using Server;
using Server.Items;
using Server.Network;

public class SnowSculpture : Item
{
    private static int[] Graphics = new int[]
    {
        0xADD4, 0xADD5, 0xADD6, 0xADD7, 0xADD8, 0xADD9, 0xADEB, 0xADDA, 0xADDB, 0xADDC, 0xADDD, 0xADDE, 0xADDF, 0xADE0, 0xADE1, 0xADE2, 0xADE3
    };

    [Constructable]
    public SnowSculpture() : base(Graphics[Utility.Random(Graphics.Length)])
    {
        Name = "Snow Sculpture";
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

    public SnowSculpture(Serial serial) : base(serial)
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
