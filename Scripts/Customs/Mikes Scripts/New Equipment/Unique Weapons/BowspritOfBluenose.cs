using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class BowspritOfBluenose : Bow
{
    [Constructable]
    public BowspritOfBluenose()
    {
        Name = "Bowsprit of Bluenose";
        Hue = Utility.Random(350, 2550);
        MinDamage = Utility.RandomMinMax(20, 60);
        MaxDamage = Utility.RandomMinMax(60, 85);
        Attributes.Luck = 100;
        Attributes.AttackChance = 5;
        Slayer = SlayerName.WaterDissipation;
        WeaponAttributes.HitColdArea = 20;
        WeaponAttributes.HitLightning = 15;
        SkillBonuses.SetValues(0, SkillName.Cartography, 20.0); // or similar skill
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public BowspritOfBluenose(Serial serial) : base(serial)
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
