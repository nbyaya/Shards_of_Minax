using System;
using Server;
using Server.Items;
using Server.Network;

public class Meteorite : Item
{
    private static int[] Graphics = new int[]
    {
        0xA1CE, 0xA1CF, 0xA1D0, 0xA1D1, 0xA1D2, 0xA1D3, 0xA1D4, 0xA1D5, 0xA1D6, 0xA1D7, 0xA1D8, 0xA1D9
    };

    [Constructable]
    public Meteorite() : base(Graphics[Utility.Random(Graphics.Length)])
    {
        Name = "Meteorite";
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
            from.SendMessage("You find the smell wonderful!");
            from.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
            from.PlaySound(0x1E0); // Coin sound
        }
    }

    public Meteorite(Serial serial) : base(serial)
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
