using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class PopStarsGlitteringCap : Cap
{
    [Constructable]
    public PopStarsGlitteringCap()
    {
        Name = "Pop Star's Glittering Cap";
        Hue = Utility.Random(500, 2500);
        Attributes.BonusDex = 5;
        Attributes.IncreasedKarmaLoss = -5;
        SkillBonuses.SetValues(0, SkillName.Musicianship, 20.0);
        SkillBonuses.SetValues(1, SkillName.Provocation, 10.0);
        Resistances.Energy = 10;
        Resistances.Cold = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public PopStarsGlitteringCap(Serial serial) : base(serial)
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
