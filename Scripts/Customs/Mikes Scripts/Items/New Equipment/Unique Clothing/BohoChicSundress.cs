using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BohoChicSundress : FancyDress
{
    [Constructable]
    public BohoChicSundress()
    {
        Name = "Boho Chic Sundress";
        Hue = Utility.Random(600, 2600);
        Attributes.RegenMana = 2;
        Attributes.CastRecovery = 1;
        SkillBonuses.SetValues(0, SkillName.Meditation, 20.0);
        Resistances.Energy = 15;
        Resistances.Poison = 5;
		XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BohoChicSundress(Serial serial) : base(serial)
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
