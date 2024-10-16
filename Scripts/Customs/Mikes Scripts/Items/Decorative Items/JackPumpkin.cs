using System;
using Server;
using Server.Items;
using Server.Network;

public class JackPumpkin : Item
{
    private static int[] Graphics = new int[]
    {
        0xACBC, 0xACBD, 0xACBE, 0xACC1, 0xACC2, 0xACC5, 0xACC6, 0xACC9, 0xACCA, 0xACCD, 0xACCE, 0xAD6E, 0xACD1, 0xACD2, 0xACD5, 0xACD6, 0xACD9, 0xACDA, 0xACDD, 0xAD6E, 0xACDE, 0xACE1, 0xACE2, 0xA9AA, 0xA9AB, 0xA9AE, 0xA9AF, 0xA9B2, 0xA9B3, 0xA9BE, 0xA9BF, 0xA9C0
    };

    [Constructable]
    public JackPumpkin() : base(Graphics[Utility.Random(Graphics.Length)])
    {
        Name = "Jack Pumpkin";
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

    public JackPumpkin(Serial serial) : base(serial)
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
