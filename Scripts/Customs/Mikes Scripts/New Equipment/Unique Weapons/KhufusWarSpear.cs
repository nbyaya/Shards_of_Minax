using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class KhufusWarSpear : Spear
{
    [Constructable]
    public KhufusWarSpear()
    {
        Name = "Khufu's WarSpear";
        Hue = Utility.Random(100, 2350);
        MinDamage = Utility.RandomMinMax(30, 80);
        MaxDamage = Utility.RandomMinMax(80, 120);
        Attributes.BonusHits = 10;
        Attributes.LowerRegCost = 5;
        Slayer = SlayerName.TrollSlaughter;
        WeaponAttributes.HitLightning = 75;
        WeaponAttributes.DurabilityBonus = 20;
        SkillBonuses.SetValues(0, SkillName.Lumberjacking, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public KhufusWarSpear(Serial serial) : base(serial)
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
