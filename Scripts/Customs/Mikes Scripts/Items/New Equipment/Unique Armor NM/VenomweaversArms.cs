using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class VenomweaversArms : BoneArms
{
    [Constructable]
    public VenomweaversArms()
    {
        Name = "Venomweaver's Arms";
        Hue = Utility.Random(111, 444);
        BaseArmorRating = Utility.RandomMinMax(55, 75);
        ArmorAttributes.LowerStatReq = 30;
        Attributes.BonusDex = 20;
        Attributes.EnhancePotions = 25;
        Attributes.LowerManaCost = 10;
        SkillBonuses.SetValues(0, SkillName.Poisoning, 50.0);
        SkillBonuses.SetValues(1, SkillName.Alchemy, 30.0);
        PhysicalBonus = 15;
        ColdBonus = 10;
        FireBonus = 15;
        EnergyBonus = 20;
        PoisonBonus = 35;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public VenomweaversArms(Serial serial) : base(serial)
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
