using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class PsychedelicTieDyeShirt : FancyShirt
{
    [Constructable]
    public PsychedelicTieDyeShirt()
    {
        Name = "Psychedelic Tie-Dye Shirt";
        Hue = Utility.Random(100, 2500);
        Attributes.BonusInt = 10;
        Attributes.RegenMana = 3;
        SkillBonuses.SetValues(0, SkillName.Magery, 15.0);
        Resistances.Energy = 20;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public PsychedelicTieDyeShirt(Serial serial) : base(serial)
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
