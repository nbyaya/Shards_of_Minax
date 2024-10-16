using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class ArchersYewBow : Bow
{
    [Constructable]
    public ArchersYewBow()
    {
        Name = "Archer's Yew Bow";
        Hue = Utility.Random(150, 2300);
        MinDamage = Utility.RandomMinMax(35, 55);
        MaxDamage = Utility.RandomMinMax(55, 85);
        Attributes.WeaponSpeed = 10;
        Attributes.LowerRegCost = 15;
        Slayer = SlayerName.OrcSlaying;
        WeaponAttributes.HitMagicArrow = 25;
        SkillBonuses.SetValues(0, SkillName.Archery, 25.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public ArchersYewBow(Serial serial) : base(serial)
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
