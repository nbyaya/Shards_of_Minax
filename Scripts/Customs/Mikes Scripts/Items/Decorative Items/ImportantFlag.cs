using System;
using Server;
using Server.Items;
using Server.Network;

public class ImportantFlag : Item
{
    private static int[] Graphics = new int[]
    {
        0xABF1, 0xABF7, 0xAC0E, 0xAC47, 0xAC14, 0xAC2B, 0xAC31, 0xAB84, 0xAB6D, 0xAB67, 0xAB50, 0xAB4A, 0xAB33, 0xAB2D, 0xAB16, 0xAB10, 0xAAF9, 0xAAF3, 0xAADC, 0xAAD6, 0xAABF, 0xAAB9, 0xAAA2, 0xAA9C, 0xAA85, 0xAA7F, 0xAA68, 0xAA62, 0xAA4B, 0xAA45
    };

    [Constructable]
    public ImportantFlag() : base(Graphics[Utility.Random(Graphics.Length)])
    {
        Name = "Important Flag";
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

    public ImportantFlag(Serial serial) : base(serial)
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
