using System;
using Server;
using Server.Items;
using Server.Network;

public class HolidayPillow : Item
{
    private static int[] Graphics = new int[]
    {
        0xADE5, 0xADE6, 0xADE7, 0xADE8, 0xADE9, 0xADEA, 0xADEB, 0xADEC, 0xA491, 0xA492, 0xA493, 0xA494, 0xA495, 0xA496, 0xA497, 0xA498
    };

    [Constructable]
    public HolidayPillow() : base(Graphics[Utility.Random(Graphics.Length)])
    {
        Name = "Holiday Pillow";
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
            from.SendMessage("You find the pillow wonderful!");
            from.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
            from.PlaySound(0x1E0); // Coin sound
        }
    }

    public HolidayPillow(Serial serial) : base(serial)
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
