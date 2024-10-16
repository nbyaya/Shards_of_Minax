using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class StarlightWozardsHat : WizardsHat
{
    [Constructable]
    public StarlightWozardsHat()
    {
        Name = "Starlight Wizard's Hat";
        Hue = Utility.Random(1, 2500);
        Attributes.SpellChanneling = 1;
        Attributes.BonusMana = 10;
        SkillBonuses.SetValues(0, SkillName.Spellweaving, 15.0);
        Resistances.Energy = 20;
        Resistances.Cold = 10;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public StarlightWozardsHat(Serial serial) : base(serial)
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
