using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class StarlightWizardsHat : WizardsHat
{
    [Constructable]
    public StarlightWizardsHat()
    {
        Name = "Starlight Wizard's Hat";
        Hue = Utility.Random(1100, 2130);
        Attributes.BonusInt = 20;
        Attributes.SpellDamage = 10;
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
        SkillBonuses.SetValues(1, SkillName.Meditation, 15.0);
        Resistances.Energy = 25;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public StarlightWizardsHat(Serial serial) : base(serial)
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
