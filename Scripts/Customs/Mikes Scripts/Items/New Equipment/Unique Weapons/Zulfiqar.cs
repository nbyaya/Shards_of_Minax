using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class Zulfiqar : Scimitar
{
    [Constructable]
    public Zulfiqar()
    {
        Name = "Zulfiqar";
        Hue = Utility.Random(500, 2700);
        MinDamage = Utility.RandomMinMax(30, 60);
        MaxDamage = Utility.RandomMinMax(60, 90);
        Attributes.BonusStr = 15;
        Attributes.DefendChance = 10;
        Slayer = SlayerName.DragonSlaying;
        WeaponAttributes.HitLightning = 40;
        WeaponAttributes.BattleLust = 30;
        SkillBonuses.SetValues(0, SkillName.Swords, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public Zulfiqar(Serial serial) : base(serial)
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
