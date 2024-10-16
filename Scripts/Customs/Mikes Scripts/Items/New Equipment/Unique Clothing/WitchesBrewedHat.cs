using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WitchesBrewedHat : WizardsHat
{
    [Constructable]
    public WitchesBrewedHat()
    {
        Name = "Witch's Brewed Hat";
        Hue = Utility.Random(200, 2900);
        Attributes.RegenMana = 3;
        Attributes.EnhancePotions = 15;
        SkillBonuses.SetValues(0, SkillName.Alchemy, 20.0);
        Resistances.Poison = 25;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WitchesBrewedHat(Serial serial) : base(serial)
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
