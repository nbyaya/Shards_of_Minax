using System;
using Server;
using Server.Items;
using Server.Network;

public class ExoticFish : Item
{
    private static int[] Graphics = new int[]
    {
        0xA35F, 0xA360, 0xA361, 0xA362, 0xA363, 0xA364, 0xA365, 0xA366, 0xA367, 0xA368, 0xA369, 0xA36A, 0xA36B, 0xA36C, 0xA36D, 0xA36E, 0xA36F, 0xA370, 0xA371, 0xA372, 0xA373, 0xA374, 0xA375, 0xA376, 0xA377, 0xA378, 0xA379, 0xA37A, 0xA37B, 0xA37C, 0xA37D, 0xA37E, 0xA37F, 0xA380, 0xA381, 0xA382, 0xA383, 0xA384, 0xA385, 0xA386, 0xA387, 0xA388, 0xA389, 0xA38A, 0xA38B, 0xA38C, 0xA38D, 0xA38E, 0xA38F, 0xA390, 0xA391, 0xA392
    };

    [Constructable]
    public ExoticFish() : base(Graphics[Utility.Random(Graphics.Length)])
    {
        Name = "Exotic Fish";
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

    public ExoticFish(Serial serial) : base(serial)
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
