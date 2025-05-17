using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class VigilantLayering : StuddedChest
{
    [Constructable]
    public VigilantLayering()
    {
        Name = "Vigilant Layering";
        Hue = Utility.Random(1500, 2300); // Darker hues to represent stealth and vigilance.
        BaseArmorRating = Utility.RandomMinMax(30, 80); // Strong but lighter than full plate.

        // Attributes focused on agility, defense, and enhancing perception:
        Attributes.BonusDex = 15;
        Attributes.BonusInt = 10;
        Attributes.BonusStam = 15;
        Attributes.DefendChance = 10;  // Helps the wearer to avoid hits.
        Attributes.Luck = 50;  // Improves chances of finding rare items during adventures.

        // Skills that align with stealth and evasion:
        SkillBonuses.SetValues(0, SkillName.Stealth, 20.0);
        SkillBonuses.SetValues(1, SkillName.Hiding, 20.0);
        SkillBonuses.SetValues(2, SkillName.DetectHidden, 15.0);

        // Elemental resistances:
        ColdBonus = 10; // Protects from cold-based attacks (fitting for stealthy environments).
        FireBonus = 10; // Helps defend against fire-based magic or attacks.
        
        XmlAttach.AttachTo(this, new XmlLevelItem()); // Attach the unique XML level item attribute.
    }

    public VigilantLayering(Serial serial) : base(serial)
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
