using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class StormsEdge : Bardiche
{
    [Constructable]
    public StormsEdge()
    {
        Name = "Storm's Edge";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(50, 100);
        MaxDamage = Utility.RandomMinMax(210, 250);
        Attributes.BonusDex = 35;
        Attributes.WeaponSpeed = 30;
        Slayer2 = SlayerName.SummerWind;
        WeaponAttributes.HitLightning = 60;
        WeaponAttributes.BattleLust = 35;
        SkillBonuses.SetValues(0, SkillName.Anatomy, 20.0);
        SkillBonuses.SetValues(1, SkillName.Healing, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public StormsEdge(Serial serial) : base(serial)
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
