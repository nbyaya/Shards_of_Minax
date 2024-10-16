using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class AlchemistsConduit : PlateHelm
{
    [Constructable]
    public AlchemistsConduit()
    {
        Name = "Alchemist's Conduit";
        Hue = Utility.Random(1, 500);
        BaseArmorRating = Utility.RandomMinMax(60, 85);
        AbsorptionAttributes.CastingFocus = 20;
        ArmorAttributes.SelfRepair = 10;
        Attributes.BonusInt = 25;
        Attributes.RegenMana = 10;
        SkillBonuses.SetValues(0, SkillName.Alchemy, 30.0);
        ColdBonus = 15;
        EnergyBonus = 20;
        FireBonus = 15;
        PhysicalBonus = 15;
        PoisonBonus = 15;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public AlchemistsConduit(Serial serial) : base(serial)
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
