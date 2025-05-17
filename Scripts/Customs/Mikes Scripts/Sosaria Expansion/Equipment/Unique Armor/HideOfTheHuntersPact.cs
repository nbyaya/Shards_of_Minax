using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class HideOfTheHuntersPact : HideGloves
{
    [Constructable]
    public HideOfTheHuntersPact()
    {
        Name = "Hide of the Hunter’s Pact";
        Hue = Utility.Random(2300, 2500); // Earthy, forest-like hues
        BaseArmorRating = Utility.RandomMinMax(15, 50);

        Attributes.BonusStr = 5;
        Attributes.BonusDex = 15;
        Attributes.BonusStam = 10;
        Attributes.Luck = 50;

        SkillBonuses.SetValues(0, SkillName.Tracking, 20.0);   // Helps with finding creatures and enemies
        SkillBonuses.SetValues(1, SkillName.AnimalLore, 15.0);  // Knowledge about animals and beasts
        SkillBonuses.SetValues(2, SkillName.Tactics, 10.0);     // Boosts combat effectiveness

        ColdBonus = 5;
        PhysicalBonus = 10;
        PoisonBonus = 5;

        XmlAttach.AttachTo(this, new XmlLevelItem());  // Associates the item with the XML leveling system

    }

    public HideOfTheHuntersPact(Serial serial) : base(serial)
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
