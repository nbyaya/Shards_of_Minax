using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class PsychedelicWizardsHat : WizardsHat
{
    [Constructable]
    public PsychedelicWizardsHat()
    {
        Name = "Psychedelic Wizard's Hat";
        Hue = Utility.Random(100, 2100);
        Attributes.BonusInt = 20;
        Attributes.SpellDamage = 10;
        SkillBonuses.SetValues(0, SkillName.Magery, 20.0);
        SkillBonuses.SetValues(1, SkillName.Spellweaving, 10.0);
        Resistances.Chaos = 15;
        Resistances.Energy = 15;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public PsychedelicWizardsHat(Serial serial) : base(serial)
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
