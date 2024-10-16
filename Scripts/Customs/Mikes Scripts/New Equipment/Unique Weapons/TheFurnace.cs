using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TheFurnace : Maul
{
    [Constructable]
    public TheFurnace()
    {
        Name = "The Furnace";
        Hue = Utility.Random(300, 2900);
        MinDamage = Utility.RandomMinMax(35, 85);
        MaxDamage = Utility.RandomMinMax(85, 125);
        Attributes.BonusStr = 10;
        Attributes.LowerManaCost = 5;
        Slayer = SlayerName.DragonSlaying;
        Slayer2 = SlayerName.DaemonDismissal;
        WeaponAttributes.HitFireArea = 30;
        SkillBonuses.SetValues(0, SkillName.Macing, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TheFurnace(Serial serial) : base(serial)
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
