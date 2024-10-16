using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ScriptoriumMastersRobe : Robe
{
    [Constructable]
    public ScriptoriumMastersRobe()
    {
        Name = "Scriptorium Master's Robe";
        Hue = Utility.Random(450, 2450);
        ClothingAttributes.SelfRepair = 4;
        Attributes.BonusInt = 15;
        Attributes.SpellChanneling = 1;
        SkillBonuses.SetValues(0, SkillName.Inscribe, 25.0);
        SkillBonuses.SetValues(1, SkillName.Magery, 10.0);
        Resistances.Energy = 20;
        Resistances.Fire = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ScriptoriumMastersRobe(Serial serial) : base(serial)
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
