using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class StarfallGallet : WarHammer
{
    [Constructable]
    public StarfallGallet()
    {
        Name = "Starfall Mallet";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(40, 80);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 30;
        Attributes.WeaponSpeed = 25;
        Slayer = SlayerName.Repond;
        WeaponAttributes.HitLightning = 50;
        WeaponAttributes.BattleLust = 20;
        SkillBonuses.SetValues(0, SkillName.Tactics, 25.0);
        SkillBonuses.SetValues(1, SkillName.Macing, 20.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public StarfallGallet(Serial serial) : base(serial)
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
