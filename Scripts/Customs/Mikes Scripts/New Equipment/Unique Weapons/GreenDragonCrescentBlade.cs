using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class GreenDragonCrescentBlade : Halberd
{
    [Constructable]
    public GreenDragonCrescentBlade()
    {
        Name = "Green Dragon Crescent Blade";
        Hue = Utility.Random(200, 2300);
        MinDamage = Utility.RandomMinMax(35, 75);
        MaxDamage = Utility.RandomMinMax(75, 120);
        Attributes.BonusStr = 20;
        Attributes.RegenHits = 5;
        Slayer = SlayerName.DragonSlaying;
        WeaponAttributes.HitFireArea = 40;
        SkillBonuses.SetValues(0, SkillName.Swords, 25.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public GreenDragonCrescentBlade(Serial serial) : base(serial)
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
