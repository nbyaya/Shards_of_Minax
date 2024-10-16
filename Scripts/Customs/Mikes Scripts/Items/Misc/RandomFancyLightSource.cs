using System;
using Server;
using Server.Items;
using Server.Network;

public class RandomFancyLightSource : Item
{
    private static int[] Graphics = new int[]
    {
        0x0B1A, 0x0B1D, 0x0B20, 0x0B22, 0x0B24, 0x0B26, 0x0E31, 0x0A07, 0x0A15
    };

	private static string[] Part1 = new string[]
	{
		"Golden", "Silver", "Crystal", "Ruby", "Sapphire", "Emerald",
		"Amethyst", "Topaz", "Pearl", "Onyx", "Jade", "Obsidian",
		"Moonstone", "Sunstone", "Starstone", "Amber", "Garnet", "Lapis",
		"Tourmaline", "Opal", "Quartz", "Coral", "Turquoise", "Agate",
		"Jasper", "Malachite", "Tiger's Eye", "Bloodstone", "Jet", "Citrine",
		"Glowing", "Radiant", "Shimmering", "Gleaming", "Luminous", "Bright",
		"Flickering", "Twinkling", "Sparkling", "Beaming", "Shining", "Dazzling",
		"Lustrous", "Glistening", "Burnished", "Polished", "Iridescent", "Scintillating",
		"Resplendent", "Incandescent", "Effulgent", "Radiant", "Shining", "Gleaming",
		"Fluorescent", "Bioluminescent", "Phosphorescent", "Aurora", "Stellar",
		"Cosmic", "Supernova", "Galactic", "Nebulaic", "Meteorite", "Interstellar",
		"Comet", "Astro", "Celestial", "Zodiacal", "Lunar", "Solar",
		"Planetary", "Satellite", "Asteroid", "Meteor", "Constellation", "Orbital"
	};

	private static string[] Part2 = new string[]
	{
		"Lamp", "Lantern", "Candle", "Torch", "Chandelier", "Sconce",
		"Candelabra", "Hurricane Lamp", "Oil Lamp", "Gas Lamp", "Wall Lamp", "Table Lamp",
		"Floor Lamp", "Pendant Light", "Flameless Candle", "Spotlight", "Fairy Light", "String Light",
		"Arc Light", "Neon Light", "LED Light", "Halogen Light", "Incandescent Light", "Fluorescent Light",
		"High-Intensity Discharge Light", "Laser Light", "Plasma Light", "Fiber Optic Light", "Lighthouse", "Beacon",
		"Flame", "Glow", "Illuminate", "Luminary", "Radiance", "Brilliance",
		"Spark", "Glimmer", "Shine", "Ray", "Beam", "Illuminate",
		"Glowstick", "Firework", "Campfire", "Bonfire", "Fireplace", "Starlight",
		"Moonlight", "Sunlight", "Firefly", "Bioluminescence", "Strobe Light", "Disco Ball",
		"UV Light", "Black Light", "Xenon Light", "Kerosene Lamp", "Solar Light", "Glowworm",
		"Zap", "Glitter", "Flash", "Flare", "Ignition", "Radiation"
	};

    public static string GenerateLightName()
    {
        return Part1[Utility.Random(Part1.Length)] + " " + Part2[Utility.Random(Part2.Length)];
    }

    [Constructable]
    public RandomFancyLightSource() : base(Graphics[Utility.Random(Graphics.Length)])
    {
        Name = GenerateLightName();
        Hue = Utility.Random(1, 3000);
        Light = LightType.Circle300;

        // Setting the weight to be consistent with light sources
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
            from.SendMessage("You enjoy the light, feeling comforted.");
            from.FixedParticles(0x376A, 9, 20, 5012, EffectLayer.Waist);
            from.PlaySound(0x1F5); // Light sound
        }
    }

    public RandomFancyLightSource(Serial serial) : base(serial)
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
