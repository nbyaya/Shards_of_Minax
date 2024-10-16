using System;
using Server;
using Server.Items;
using Server.Network;

public class DrapedBlanket : Item
{
    private static int[] Graphics = new int[]
    {
        0xAD81, 0xAD82, 0xAD83, 0xAD84, 0xAD85, 0xAD86, 0xAD87, 0xAD88, 0xA9CE, 0xA9CF, 0xA9D0, 0xA9D1, 0xA9D2, 0xA9D3, 0xA9D4, 0xA9D5
    };

    [Constructable]
    public DrapedBlanket() : base(Graphics[Utility.Random(Graphics.Length)])
    {
        Name = "Blanket";
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

    public DrapedBlanket(Serial serial) : base(serial)
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
