using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ForestersPlateBoots : PlateGloves
{
    [Constructable]
    public ForestersPlateBoots()
    {
        Name = "Forester's Plate Boots";
        Hue = Utility.Random(1, 3000);
        BaseArmorRating = Utility.RandomMinMax(70, 90);
        ArmorAttributes.LowerStatReq = 40;
        ArmorAttributes.DurabilityBonus = 80;
        Attributes.BonusDex = 20;
        Attributes.LowerManaCost = 15;
        SkillBonuses.SetValues(0, SkillName.Archery, 50.0);
        SkillBonuses.SetValues(1, SkillName.Stealth, 30.0);
        SkillBonuses.SetValues(2, SkillName.Tracking, 30.0);
        PhysicalBonus = 20;
        ColdBonus = 10;
        FireBonus = 10;
        EnergyBonus = 20;
        PoisonBonus = 10;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ForestersPlateBoots(Serial serial) : base(serial)
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
