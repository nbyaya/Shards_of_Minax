using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ChefsHatOfFocus : LeatherCap
{
    [Constructable]
    public ChefsHatOfFocus()
    {
        Name = "Chef's Hat of Focus";
        Hue = Utility.Random(50, 550);
        BaseArmorRating = Utility.RandomMinMax(20, 50);
        AbsorptionAttributes.CastingFocus = 15;
        ArmorAttributes.SelfRepair = 3;
        Attributes.BonusInt = 10;
        Attributes.RegenMana = 3;
        SkillBonuses.SetValues(0, SkillName.Cooking, 20.0);
        ColdBonus = 5;
        EnergyBonus = 5;
        FireBonus = 10;
        PhysicalBonus = 5;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ChefsHatOfFocus(Serial serial) : base(serial)
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
