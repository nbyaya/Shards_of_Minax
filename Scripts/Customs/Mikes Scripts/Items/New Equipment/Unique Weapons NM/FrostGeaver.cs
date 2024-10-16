using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FrostGeaver : WarAxe
{
    [Constructable]
    public FrostGeaver()
    {
        Name = "Frost Reaver";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(30, 60);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 25;
        Attributes.WeaponSpeed = 30;
        Slayer = SlayerName.ArachnidDoom;
        Slayer2 = SlayerName.FlameDousing;
        WeaponAttributes.HitColdArea = 60;
        WeaponAttributes.DurabilityBonus = 50;
        SkillBonuses.SetValues(0, SkillName.Blacksmith, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FrostGeaver(Serial serial) : base(serial)
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
