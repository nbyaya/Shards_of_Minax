using System;
using Server;
using Server.Items;
using Server.Network;

public class ShardCrest : Item
{
    private static int[] Graphics = new int[]
    {
        0x6380, 0x6381, 0x6382, 0x6383, 0x6384, 0x6385, 0x6386, 0x6387, 0x6388, 0x6389, 0x638A, 0x638B, 0x638C, 0x638D, 0x638E, 0x638F, 0x6390, 0x6391, 0x6392, 0x6393, 0x6394, 0x6395, 0x6396, 0x6397, 0x6398, 0x6399, 0x639A, 0x639B, 0x639C, 0x639D, 0x639E, 0x639F, 0x63A0, 0x63A1, 0x63A2, 0x63A3, 0x63A4, 0x63A5, 0x63A6, 0x63A7, 0x63A8, 0x63A9, 0x63AA, 0x63AB, 0x63AC, 0x63AD, 0x63AE, 0x63AF, 0x63B0, 0x63B1, 0x63B2, 0x63B3, 0x63B4, 0x63B5
    };

    [Constructable]
    public ShardCrest() : base(Graphics[Utility.Random(Graphics.Length)])
    {
        Name = "Shard Crest";
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

    public ShardCrest(Serial serial) : base(serial)
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
