using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class WrestlersGrippingGloves : LeatherGloves
{
    [Constructable]
    public WrestlersGrippingGloves()
    {
        Name = "Wrestler's Gripping Gloves";
        Hue = Utility.Random(10, 250);
        BaseArmorRating = Utility.RandomMinMax(35, 55);
        ArmorAttributes.MageArmor = 1;
        Attributes.WeaponDamage = 10;
        Attributes.RegenHits = 5;
        SkillBonuses.SetValues(3, SkillName.Wrestling, 10.0);
        ColdBonus = 5;
        EnergyBonus = 5;
        FireBonus = 5;
        PhysicalBonus = 15;
        PoisonBonus = 5;
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public WrestlersGrippingGloves(Serial serial) : base(serial)
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
