using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class StarfallMallet : Maul
{
    [Constructable]
    public StarfallMallet()
    {
        Name = "Starfall Mallet";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(30, 70);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 30;
        Attributes.BonusHits = 40;
        Attributes.WeaponSpeed = 25;
        Slayer = SlayerName.DragonSlaying;
        WeaponAttributes.HitLightning = 45;
        WeaponAttributes.HitDispel = 35;
        SkillBonuses.SetValues(0, SkillName.Wrestling, 20.0);
        SkillBonuses.SetValues(1, SkillName.Tactics, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public StarfallMallet(Serial serial) : base(serial)
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
