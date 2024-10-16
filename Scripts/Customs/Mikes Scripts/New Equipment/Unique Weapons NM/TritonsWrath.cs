using System;
using Server;
using Server.Items;
using Server.Engines.XmlSpawner2;

public class TritonsWrath : Pitchfork
{
    [Constructable]
    public TritonsWrath()
    {
        Name = "Triton's Wrath";
        Hue = Utility.Random(1, 3000);
        MinDamage = Utility.RandomMinMax(50, 85);
        MaxDamage = Utility.RandomMinMax(100, 250);
        Attributes.BonusStr = 20;
        Attributes.BonusMana = 15;
        Attributes.WeaponDamage = 30;
        Slayer = SlayerName.WaterDissipation;
        WeaponAttributes.HitColdArea = 45;
        WeaponAttributes.BattleLust = 20;
        SkillBonuses.SetValues(0, SkillName.Macing, 20.0);
        SkillBonuses.SetValues(1, SkillName.MagicResist, 15.0);
        XmlAttach.AttachTo(this, new XmlLevelItem());
    }

    public TritonsWrath(Serial serial) : base(serial)
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
