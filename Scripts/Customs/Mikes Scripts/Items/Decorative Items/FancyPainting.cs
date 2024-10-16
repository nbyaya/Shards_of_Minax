using System;
using Server;
using Server.Items;
using Server.Network;

public class FancyPainting : Item
{
    private static int[] Graphics = new int[]
    {
        0xA2DC, 0xA2DD, 0xA2DE, 0xA2DF, 0xA2E0, 0xA2E1, 0xA2E4, 0xA2E5, 0xA2E6, 0xA2E7, 0xA2E8, 0xA2E9, 0xA2EA, 0xA2EB, 0xA2EC, 0xA2ED, 0xA2EE, 0xA2EF, 0xA2F0, 0xA2F1, 0xA2F2, 0xA2F3, 0xA2F4, 0xA2F5
    };

    [Constructable]
    public FancyPainting() : base(Graphics[Utility.Random(Graphics.Length)])
    {
        Name = "Fancy Painting";
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

    public FancyPainting(Serial serial) : base(serial)
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
