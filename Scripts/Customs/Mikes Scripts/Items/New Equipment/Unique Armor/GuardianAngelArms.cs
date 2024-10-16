using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GuardianAngelArms : PlateArms
{
    [Constructable]
    public GuardianAngelArms()
    {
        Name = "Guardian Angel Arms";
        Hue = Utility.Random(300, 700);
        BaseArmorRating = Utility.RandomMinMax(35, 75);
        Attributes.DefendChance = 20;
        Attributes.EnhancePotions = 15;
        ColdBonus = 25;
        EnergyBonus = 20;
        FireBonus = 25;
        PhysicalBonus = 10;
        PoisonBonus = 20;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GuardianAngelArms(Serial serial) : base(serial)
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
