using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class KaomsMaul : Maul
{
    [Constructable]
    public KaomsMaul()
    {
        Name = "Kaom's Maul";
        Hue = Utility.Random(500, 2900);
        MinDamage = Utility.RandomMinMax(30, 90);
        MaxDamage = Utility.RandomMinMax(90, 130);
        Attributes.BonusHits = 25;
        Attributes.LowerRegCost = 15;
        Slayer = SlayerName.DragonSlaying;
        WeaponAttributes.HitFireArea = 40;
        WeaponAttributes.BattleLust = 20;
        SkillBonuses.SetValues(0, SkillName.Lumberjacking, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public KaomsMaul(Serial serial) : base(serial)
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
