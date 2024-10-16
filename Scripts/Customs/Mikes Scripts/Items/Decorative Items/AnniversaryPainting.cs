using System;
using Server;
using Server.Items;
using Server.Network;

public class AnniversaryPainting : Item
{
    private static int[] Graphics = new int[]
    {
        0xAED9, 0xAEDA, 0xAEDB, 0xAEDC, 0xAEDD, 0xAEDE, 0xAEE0, 0xAEE1, 0xAEE2, 0xAEE3, 0xAEE4, 0xAEE5,0xAEE6, 0xAEE7, 0xAEE8, 0xAEE9, 0xAEEA, 0xAEEB, 0xAEEC, 0xAEED, 0xAEEE, 0xAEEF, 0xAEF0, 0xAEF1, 0xAEF2, 0xAEF3, 0xAEF4, 0xAEF5, 0xAEF6, 0xAEF7, 0xAEF8, 0xAEF9, 0xAEFB, 0xAEFA, 0xAEFC, 0xAEFD, 0xAEFE, 0xAEFF, 0xAF00, 0xAF01, 0xAF02, 0xAF03,0xAF04, 0xAF06, 0xAF07, 0xAF08
    };

    [Constructable]
    public AnniversaryPainting() : base(Graphics[Utility.Random(Graphics.Length)])
    {
        Name = "Anniversary Painting";
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

    public AnniversaryPainting(Serial serial) : base(serial)
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
