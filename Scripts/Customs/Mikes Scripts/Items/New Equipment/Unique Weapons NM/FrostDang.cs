using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class FrostDang : Longsword
{
    [Constructable]
    public FrostDang()
    {
        Name = "Frost Fang";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(20, 50);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusDex = 20;
        Attributes.WeaponSpeed = 25;
        Slayer = SlayerName.WaterDissipation;
        WeaponAttributes.HitColdArea = 45;
        WeaponAttributes.ResistColdBonus = 20;
        SkillBonuses.SetValues(0, SkillName.MagicResist, 20.0);
        SkillBonuses.SetValues(1, SkillName.Parry, 10.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public FrostDang(Serial serial) : base(serial)
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
