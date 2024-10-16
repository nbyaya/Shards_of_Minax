using System;
using Server;
using Server.Items;
using Server.Network;

public class CityBanner : Item
{
    private static int[] Graphics = new int[]
    {
        0x4B62, 0x4B63, 0x4B64, 0x4B65, 0x4B66, 0x4B67, 0x4B68, 0x4B69, 0x4B6A, 0x4B6B, 0x4B6C, 0x4B6D, 0x4B6E, 0x4B6F, 0x4B70, 0x4B71, 0x4B72, 0x4B73
    };

    [Constructable]
    public CityBanner() : base(Graphics[Utility.Random(Graphics.Length)])
    {
        Name = "City Banner";
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

    public CityBanner(Serial serial) : base(serial)
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
