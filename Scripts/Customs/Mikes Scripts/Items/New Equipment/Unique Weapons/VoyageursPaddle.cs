using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class VoyageursPaddle : Club
{
    [Constructable]
    public VoyageursPaddle()
    {
        Name = "Voyageur's Paddle";
        Hue = Utility.Random(300, 2500);
        MinDamage = Utility.RandomMinMax(15, 55);
        MaxDamage = Utility.RandomMinMax(55, 70);
        Attributes.BonusDex = 5;
        Attributes.LowerRegCost = 10;
        Slayer = SlayerName.TrollSlaughter;
        WeaponAttributes.HitDispel = 20;
        WeaponAttributes.HitFatigue = 20;
        SkillBonuses.SetValues(0, SkillName.Fishing, 15.0);
        SkillBonuses.SetValues(1, SkillName.Herding, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public VoyageursPaddle(Serial serial) : base(serial)
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
